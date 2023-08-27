using AlertTracking.Abstractions.DataAccess.AzureKeyVaults;

namespace AlertTracking.Abstractions.Exceptions;

public class MissedKeyVaultSecretException : InvalidOperationException
{
    public MissedKeyVaultSecretException(IKeyVaultsManager keyVaultsManager, string secretName, string? message = null) : base(message)
    {
        if (string.IsNullOrEmpty(secretName))
            throw new ArgumentException($"'{nameof(secretName)}' cannot be null or empty.", nameof(secretName));

        KeyVaultsManager = keyVaultsManager ?? throw new ArgumentNullException(nameof(keyVaultsManager));
        SecretName = secretName;
    }

    public IKeyVaultsManager KeyVaultsManager { get; }
    public string SecretName { get; }
}
