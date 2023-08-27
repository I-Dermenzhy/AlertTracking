namespace AlertTracking.Abstractions.DataAccess.AzureKeyVaults;

public interface IKeyVaultsManager
{
    string GetSecret(string secretName);
}
