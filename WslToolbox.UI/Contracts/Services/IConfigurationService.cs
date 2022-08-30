namespace WslToolbox.UI.Contracts.Services;

public interface IConfigurationService
{
    void Save<T>(T config) where T : class;
    T Read<T>();
}