using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Fusionneur_de_pratiques
{
    internal class SecretManager
    {

        private readonly static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.bin");

        public static AppSecrets ReadSecrets()
        {
            if (!File.Exists(_filePath)) return new AppSecrets();
            var encryptedContent = File.ReadAllText(_filePath);
            var json = Decrypt(encryptedContent);
            return JsonSerializer.Deserialize<AppSecrets>(json);
        }

        public static void WriteSecrets(AppSecrets secrets)
        {
            var json = JsonSerializer.Serialize(secrets);
            var encryptedContent = Encrypt(json);
            File.WriteAllText(_filePath, encryptedContent);
        }

        private static string Encrypt(string input)
        {
            var entropy = Encoding.Unicode.GetBytes(Environment.MachineName);
            var data = Encoding.Unicode.GetBytes(input);
            var encryptedData = ProtectedData.Protect(data, entropy, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        private static string Decrypt(string encryptedData)
        {
            var entropy = Encoding.Unicode.GetBytes(Environment.MachineName);
            var data = Convert.FromBase64String(encryptedData);
            var decryptedData = ProtectedData.Unprotect(data, entropy, DataProtectionScope.CurrentUser);
            return Encoding.Unicode.GetString(decryptedData);
        }

    }

    public struct AppSecrets
    {
        public string EntraClient { get; set; }
        public string UserPassword { get; set; }
    }
}