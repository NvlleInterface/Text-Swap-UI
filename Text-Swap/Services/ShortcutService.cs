using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Text_Swap.Model;

namespace Text_Swap.Services;

public static class ShortcutService
{
    private static readonly string _filePath = "C:\\shortcuts\\shortcuts.json";
    //private static readonly string _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Text-Swap", "shortcuts.json");


    public static List<ShortcutModel> LoadShortcuts()
    {
        string directoryPath = Path.GetDirectoryName(_filePath);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]"); // Écrire un JSON vide
        }

        string json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<ShortcutModel>>(json) ?? new List<ShortcutModel>();
    }

    public static void SaveShortcuts(IEnumerable<ShortcutModel> shortcuts)
    {
        string json = JsonSerializer.Serialize(shortcuts, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}