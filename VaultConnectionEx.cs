using MFiles.VAF.Configuration.Logging.Targets;
using MFilesAPI;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Net;

namespace Fusionneur_de_pratiques
{
    internal class VaultConnectionEx : IDisposable
    {
        private MFilesServerApplication _application;
        private readonly VaultConnectionSettings _vaultSettings;
        public string ServerVersion { get; private set; }
        public bool Connected { get; private set; }
        private Vault _vault;
        private bool _disposed = false;

        public Vault GetVault()
        {
            try
            {
                _vault.TestConnectionToVault();
            }
            catch
            {
                Connected = false;
                _application.Disconnect();
                ConnectToVault();
                ServerVersion = _application.GetServerVersion().Display;
                Connected = true;
            }

            return _vault;
        }

        public VaultConnectionEx(VaultConnectionSettings vaultSettings)
        {
            try
            {
                Connected = false;
                _vaultSettings = vaultSettings;
                _application = new MFilesServerApplication();
                _vault = ConnectToVault();
                ServerVersion = _application.GetServerVersion().Display;
                Connected = true;
            }
            catch (Exception ex)
            {
                Dispose();
                throw new Exception("Unable to connect to vault (check connection.json configuration).", ex);
            }
        }

        private Vault ConnectToVault()
        {
            MFServerConnection mfServerCon;

            switch (_vaultSettings.AuthenticationType)
            {
                case AuthenticationType.CurrentWindowsUser:
                    mfServerCon = _application.Connect(
                        MFAuthType.MFAuthTypeLoggedOnWindowsUser,
                        String.Empty,
                        String.Empty,
                        String.Empty,
                        _vaultSettings.Protocol,
                        String.Empty,
                        _vaultSettings.Port);
                    break;
                case AuthenticationType.SpecificWindowsUser:
                    mfServerCon = _application.Connect(
                        MFAuthType.MFAuthTypeSpecificWindowsUser,
                        _vaultSettings.UserAccount.Username,
                        _vaultSettings.UserAccount.Password,
                        _vaultSettings.UserAccount.Domain,
                        _vaultSettings.Protocol,
                        _vaultSettings.Server,
                        _vaultSettings.Port);
                    break;
                case AuthenticationType.MFilesUser:

                    mfServerCon = _application.Connect(
                         MFAuthType.MFAuthTypeSpecificMFilesUser,
                        _vaultSettings.UserAccount.Username,
                        _vaultSettings.UserAccount.Password,
                        String.Empty,
                        _vaultSettings.Protocol,
                        _vaultSettings.Server,
                        _vaultSettings.Port);
                    break;
                case AuthenticationType.ApplicationAccount:
                    mfServerCon = ConnectApplicationAccount();
                    break;
                default:
                    throw new InvalidOperationException("Unsupported authentication type.");
            }

            if (MFServerConnection.MFServerConnectionAuthenticated != mfServerCon)
            {
                throw new Exception("Unable to authenticate to the M-Files server.");
            }


            return _application.LogInToVault(_vaultSettings.VaultGUID);
        }

        private MFServerConnection ConnectApplicationAccount()
        {
            var entraInfo = _vaultSettings.ApplicationAccount.EntraApplication;
            IConfidentialClientApplication val = (
                (AbstractApplicationBuilder<ConfidentialClientApplicationBuilder>)(object)ConfidentialClientApplicationBuilder
                .Create(entraInfo.ClientId)
                .WithClientSecret(entraInfo.ClientSecret))
                .WithAuthority(new Uri("https://login.microsoftonline.com/" + entraInfo.Tenant), true).Build();


            string[] array = new string[1] { $"{entraInfo.ApiIdUri}/.default" };
            Microsoft.Identity.Client.AuthenticationResult result = ((BaseAbstractAcquireTokenParameterBuilder<AcquireTokenForClientParameterBuilder>)(object)val.AcquireTokenForClient((IEnumerable<string>)array)).ExecuteAsync().Result;
            var accessToken = result.AccessToken;

            var faConfig = _vaultSettings.ApplicationAccount.FederatedAuthentication;
            var pluginInfo = new PluginInfo();
            pluginInfo.Name = faConfig.ConfigurationName;
            pluginInfo.ConfigurationSource = new NamedValues();
            pluginInfo.ConfigurationSource["VaultGUID"] = Guid.Parse(_vaultSettings.VaultGUID).ToString("B");
            pluginInfo.ConfigurationSource["Scope"] = faConfig.ScopeName + ":";

            var authData = new NamedValues();
            authData["AuthType"] = "Client";
            authData["UpdateMetadata"] = "true";
            authData["AccountName"] = _vaultSettings.ApplicationAccount.AccountName;
            authData["Token"] = accessToken;

            var timeZoneInfo = new TimeZoneInformation();
            timeZoneInfo.LoadWithCurrentTimeZone();


            MFilesAPI.AuthenticationResult authenticationResult = _application.ConnectWithAuthenticationDataEx2(
                pluginInfo,
                authData,
                String.Empty,
                timeZoneInfo,
               _vaultSettings.Protocol,
               _vaultSettings.Server,
               _vaultSettings.Port,
               EncryptedConnection: true);
            if (MFServerConnection.MFServerConnectionAuthenticated != authenticationResult.ServerFinal.ServerConnection)
            {
                throw new Exception("Wasn't able to authenticate to the server with ApplicationAccount.");
            }


            return authenticationResult.ServerFinal.ServerConnection;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                try
                {
                    _vault?.LogOutSilent();
                }
                catch { /* ignore */ }
                try
                {
                    _application?.Disconnect();
                }
                catch { /* ignore */ }
                _disposed = true;
            }
        }
    }
}