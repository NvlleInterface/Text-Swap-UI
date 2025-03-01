using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Text_Swap.Services
{
    public static class ShortcutService
    {
        private static readonly string _filePath = GetLocalisationJson();

        static ShortcutService()
        {
            EnsureFileExists();
        }

        private static void EnsureFileExists()
        {
            string directoryPath = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "{}"); // JSON vide (objet)
            }
        }

        public static Dictionary<string, string> LoadShortcuts()
        {
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }

        public static bool AddShortcut(string trigger, string replacement)
        {
            var shortcuts = LoadShortcuts();

            if (shortcuts.ContainsKey(trigger))
                return false; // Clé déjà existante, ajout refusé

            shortcuts[trigger] = replacement;
            SaveShortcuts(shortcuts);
            return true;
        }

        public static bool RemoveShortcut(string trigger)
        {
            var shortcuts = LoadShortcuts();

            if (!shortcuts.ContainsKey(trigger))
                return false; // Clé inexistante

            shortcuts.Remove(trigger);
            SaveShortcuts(shortcuts);
            return true;
        }

        public static void SaveShortcuts(Dictionary<string, string> shortcuts)
        {
            string json = JsonSerializer.Serialize(shortcuts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }

        private static string GetLocalisationJson()
        {
            var config = new ConfigurationBuilder()
                        .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
                        .Build();
            return config["ShortcutsFilePath"];
        }
    }
}
