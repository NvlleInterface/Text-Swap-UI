using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Text_Swap.Model;

namespace Text_Swap.Services
{
    public static class ShortcutHelper
    {
        public static ObservableCollection<ShortcutModel> ConvertToObservableCollection(Dictionary<string, string> dictionary)
        {
            var collection = new ObservableCollection<ShortcutModel>();

            foreach (var kvp in dictionary)
            {
                collection.Add(new ShortcutModel { Trigger = kvp.Key, Replacement = kvp.Value });
            }

            return collection;
        }

        public static Dictionary<string, string> ConvertToDictionary(ObservableCollection<ShortcutModel> collection)
        {
            return collection.ToDictionary(s => s.Trigger, s => s.Replacement);
        }

    }
}
