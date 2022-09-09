namespace WslToolbox.UI.Contracts.Services;

public interface IConfigurationService
{
    void Save<T>(T config) where T : class;
    void Restore<T>() where T : class;
    void Delete<T>() where T : class;
    void Delete();
    T Read<T>();
}