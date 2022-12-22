using Newtonsoft.Json;

namespace WslToolbox.UI.Core.Helpers;

public static class LocalStorage
{
    public const string UpdateManifest = "update.json";

    public static void WriteStorage(string file, string content)
    {
        if (!Directory.Exists(Toolbox.AppData))
        {
            Directory.CreateDirectory(Toolbox.AppData);
        }

        File.WriteAllText(Path.Combine(Toolbox.AppData, file), content);
    }

    public static T ReadStorage<T>(string file) where T : class
    {
        if (!Directory.Exists(Toolbox.AppData) || !File.Exists(Path.Combine(Toolbox.AppData, file)))
        {
            throw new FileNotFoundException();
        }

        var contents = File.ReadAllText(Path.Combine(Toolbox.AppData, file));
        return JsonConvert.DeserializeObject<T>(contents);
    }

    public static T ReadStorageOrDefault<T>(string file) where T : class
    {
        if (!Directory.Exists(Toolbox.AppData) || !File.Exists(Path.Combine(Toolbox.AppData, file)))
        {
            return null;
        }

        var contents = File.ReadAllText(Path.Combine(Toolbox.AppData, file));
        return JsonConvert.DeserializeObject<T>(contents);
    }

    public static string ReadStorage(string file)
    {
        return !Directory.Exists(Toolbox.AppData) || !File.Exists(Path.Combine(Toolbox.AppData, file))
            ? null
            : File.ReadAllText(Path.Combine(Toolbox.AppData, file));
    }
}