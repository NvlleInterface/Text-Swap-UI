using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Swap.Model;

public class ShortcutModel
{
    public string Trigger { get; set; } = string.Empty;
    public string Replacement { get; set; } = string.Empty;
}

public class ShortcutCollection : ObservableCollection<ShortcutModel> { }
