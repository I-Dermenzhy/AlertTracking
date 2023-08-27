using AlertTracking.Abstractions.DataAccess.AzureKeyVaults;

using Azure.Security.KeyVault.Secrets;

namespace AlertTracking.Services.DataAccess.AzureKeyVaults;

public class KeyVaultsManager : IKeyVaultsManager
{
    private readonly SecretClient _secretClient;

    public KeyVaultsManager(SecretClient secretClient) => _secretClient = secretClient;

    public string GetSecret(string secretName)
    {
        KeyVaultSecret keyValueSecret = _secretClient.GetSecret(secretName);

        return keyValueSecret.Value;
    }
}
