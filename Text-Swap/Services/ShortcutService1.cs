using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Text_Swap.Model;

namespace Text_Swap.Services
{
    public class ShortcutService1
    {
        private Dictionary<string, string> _shortcuts = new();
        private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Text-Swap", "shortcuts.json");
        public ShortcutService1()
        {
            LoadShortcuts();
        }

        public void LoadShortcuts()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                _shortcuts = JsonSerializer.Deserialize<List<ShortcutModel>>(json)
                    .ToDictionary(s => s.Trigger, s => s.Replacement);
            }
            else
            {
                _shortcuts = new Dictionary<string, string>();
            }
        }

        public void SaveShortcuts()
        {
            string json = JsonSerializer.Serialize(_shortcuts.Select(kv => new ShortcutModel { Trigger = kv.Key, Replacement = kv.Value }));
            File.WriteAllText(FilePath, json);
        }

        public void AddShortcut(string trigger, string replacement)
        {
            if (!_shortcuts.ContainsKey(trigger))
            {
                _shortcuts[trigger] = replacement;
                SaveShortcuts();
            }
        }

        public string CheckForShortcut(string inputText)
        {
            foreach (var shortcut in _shortcuts)
            {
                if (inputText.EndsWith(shortcut.Key + " "))  // Vérifie avec espace
                {
                    return inputText.Replace(shortcut.Key, shortcut.Value).TrimEnd();
                }
            }
            return inputText;
        }
    }
}
