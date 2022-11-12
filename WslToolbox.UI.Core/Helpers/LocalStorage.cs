using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace WslToolbox.UI.Core.Helpers;

public static class LocalStorage
{
    private static readonly string AppDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    private static readonly string AppData = @$"{AppDirectory}\data";
    
    public const string UpdateManifest = "update.json";
    
    public static void WriteStorage(string file, string content)
    {
        if (!Directory.Exists(AppData))
        {
            Directory.CreateDirectory(AppData);
        }
        
        File.WriteAllText($@"{AppData}\{file}", content);
    }
    
    public static T ReadStorage<T>(string file) where T : class
    {
        if (!Directory.Exists(AppData) || !File.Exists($@"{AppData}\{file}"))
        {
            throw new FileNotFoundException();
        }
        
        var contents = File.ReadAllText($@"{AppData}\{file}");
        return JsonConvert.DeserializeObject<T>(contents);
    }
    
    public static T ReadStorageOrDefault<T>(string file) where T : class
    {
        if (!Directory.Exists(AppData) || !File.Exists($@"{AppData}\{file}"))
        {
            return null;
        }
        
        var contents = File.ReadAllText($@"{AppData}\{file}");
        return JsonConvert.DeserializeObject<T>(contents);
    }
    
    public static string ReadStorage(string file)
    {
        return !Directory.Exists(AppData) || !File.Exists($@"{AppData}\{file}") 
            ? null 
            : File.ReadAllText($@"{AppData}\{file}");
    }
}