using Newtonsoft.Json;
using NLog.Targets;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace Fusionneur_de_pratiques
{
    public class VaultConnectionSettings
    {
#if DEBUG
    private readonly static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"connection.Development.json");
#else
        private readonly static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"connection.json");
#endif

        private static JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter(allowIntegerValues: false) }
        };

        private const string hiddenString = "******";
        public string Protocol { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string VaultGUID { get; set; } = string.Empty;
        public AuthenticationType AuthenticationType { get; set; }
        public ApplicationAccountSettings ApplicationAccount { get; set; } = new ApplicationAccountSettings();
        public UserAccountSettings UserAccount { get; set; } = new UserAccountSettings();

        public static VaultConnectionSettings LoadConnectionInfo()
        {
            var json = File.ReadAllText(_filePath);

            var info = System.Text.Json.JsonSerializer.Deserialize<VaultConnectionSettings>(json, _serializerOptions);
            if (info == null) return new VaultConnectionSettings();

            ProcessSecrets(info);
            return info;
        }

        private static void ProcessSecrets(VaultConnectionSettings info)
        {
            var savedEntraSecret = info.ApplicationAccount.EntraApplication.ClientSecret;
            var savedUserPassword = info.UserAccount.Password;

            var secrets = SecretManager.ReadSecrets();

            // Check if we need to update the stored secrets
            var updateNeeded = false;
            if (savedEntraSecret != hiddenString && savedEntraSecret != secrets.EntraClient)
            {
                secrets.EntraClient = savedEntraSecret;
                updateNeeded = true;
            }

            if (savedUserPassword != hiddenString && savedUserPassword != secrets.UserPassword)
            {
                secrets.UserPassword = savedUserPassword;
                updateNeeded = true;
            }

            if (updateNeeded)
            {
                SecretManager.WriteSecrets(secrets);

            }

            updateNeeded = false;
            if (savedEntraSecret != hiddenString && !String.IsNullOrEmpty(savedEntraSecret))
            {
                info.ApplicationAccount.EntraApplication.ClientSecret = hiddenString;
                updateNeeded = true;
            }

            if (savedUserPassword != hiddenString && !String.IsNullOrEmpty(savedUserPassword))
            {
                info.UserAccount.Password = hiddenString;
                updateNeeded = true;
            }

            if (updateNeeded)
            {
                var json = System.Text.Json.JsonSerializer.Serialize(info, _serializerOptions);
                File.WriteAllText(_filePath, json);
            }

            info.ApplicationAccount.EntraApplication.ClientSecret = secrets.EntraClient;
            info.UserAccount.Password = secrets.UserPassword;
        }
    }


    public class ApplicationAccountSettings
    {
        public string AccountName { get; set; } = string.Empty;
        public FederatedAuthenticationSettings FederatedAuthentication { get; set; } = new FederatedAuthenticationSettings();
        public EntraApplicationSettings EntraApplication { get; set; } = new EntraApplicationSettings();
    }

    public class FederatedAuthenticationSettings
    {
        public string ScopeName { get; set; } = string.Empty;
        public string ConfigurationName { get; set; } = string.Empty;
    }

    public class EntraApplicationSettings
    {
        public string Tenant { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string ApiIdUri { get; set; } = string.Empty;
    }

    public class UserAccountSettings
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
    }

    public enum AuthenticationType
    {
        CurrentWindowsUser,
        SpecificWindowsUser,
        MFilesUser,
        ApplicationAccount
    }
}