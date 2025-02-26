using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Text_Swap.Model;
using Text_Swap.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Text_Swap.ViewModel;

//public class HomeViewModel : ViewModelBase
//{
//    private string _text;
//    private string _shortcut;
//    private string _replacement;
//    private Dictionary<string, string> _shortcuts;

//    public string Shortcut
//    {
//        get => _shortcut;
//        set
//        {
//            _shortcut = value;
//            OnPropertyChanged(nameof(Shortcut));
//            UpdateReplacement();
//        }
//    }

//    public string Text
//    {
//        get => _text;
//        set
//        {
//            _text = value;
//            OnPropertyChanged(nameof(Text));
//        }
//    }

//    public string Replacement
//    {
//        get => _replacement;
//        private set
//        {
//            _replacement = value;
//            OnPropertyChanged(nameof(Replacement));
//        }
//    }

//    public HomeViewModel()
//    {
//        _shortcuts = ShortcutService.LoadShortcuts().ToDictionary(s => s.Trigger, s => s.Replacement); ; // Charge les raccourcis depuis un fichier JSON
//        Text = "";
//        AddShortcutCommand = new ViewModelCommand(ExecuteAddShortcutCommand);
//    }

//    private void ExecuteAddShortcutCommand(object obj)
//    {
//        if (!string.IsNullOrWhiteSpace(ShortcutTrigger) && !string.IsNullOrWhiteSpace(ShortcutReplacement))
//        {
//            _shortcutService.AddShortcut(ShortcutTrigger, ShortcutReplacement);
//            ShortcutTrigger = "";
//            ShortcutReplacement = "";
//        }
//    }

//    public ICommand AddShortcutCommand { get; }

//    private void UpdateReplacement()
//    {
//        if (_shortcuts.TryGetValue(Shortcut, out var value))
//        {
//            Replacement = value;
//        }
//        else
//        {
//            Replacement = string.Empty;
//        }
//    }
//}
public class HomeViewModel : ViewModelBase
{
    private string _text;
    private string _shortcutTrigger;
    private string _shortcutReplacement;
    private readonly ShortcutService1 _shortcutService;

    public HomeViewModel()
    {
        _shortcutService = new ShortcutService1();
        Text = "";
        AddShortcutCommand = new ViewModelCommand(AddShortcut);
    }

    public string Text
    {
        get => _text;
        set
        {
            _text = value;
            OnPropertyChanged();
        }
    }

    public string ShortcutTrigger
    {
        get => _shortcutTrigger;
        set { _shortcutTrigger = value; OnPropertyChanged(); }
    }

    public string ShortcutReplacement
    {
        get => _shortcutReplacement;
        set { _shortcutReplacement = value; OnPropertyChanged(); }
    }

    public ICommand AddShortcutCommand { get; }

    private void AddShortcut(object obj)
    {
        if (!string.IsNullOrWhiteSpace(ShortcutTrigger) && !string.IsNullOrWhiteSpace(ShortcutReplacement))
        {
            _shortcutService.AddShortcut(ShortcutTrigger, ShortcutReplacement);
            ShortcutTrigger = "";
            ShortcutReplacement = "";
        }
    }

    public void HandleKeyPress(Key key)
    {
        if (key == Key.Space)
        {
            Text = _shortcutService.CheckForShortcut(Text);
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

}
