using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Text_Swap.Model;
using Text_Swap.Services;

namespace Text_Swap.ViewModel;

public class RaccourciViewModel : ViewModelBase
{
    public ObservableCollection<ShortcutModel> Shortcuts { get; set; }

    private string _newTrigger;
    public string NewTrigger
    {
        get => _newTrigger;
        set { _newTrigger = value; OnPropertyChanged(nameof(NewTrigger)); }
    }

    private string _newReplacement;
    public string NewReplacement
    {
        get => _newReplacement;
        set { _newReplacement = value; OnPropertyChanged(nameof(NewReplacement)); }
    }

    public ICommand AddShortcutCommand { get; }
    public ICommand RemoveShortcutCommand { get; }

    public RaccourciViewModel()
    {
        Shortcuts = new ObservableCollection<ShortcutModel>(ShortcutService.LoadShortcuts());

        AddShortcutCommand = new ViewModelCommand(_ => AddShortcut(), _ => CanAddShortcut());
        RemoveShortcutCommand = new ViewModelCommand(RemoveShortcut, _ => Shortcuts.Any());
    }

    private bool CanAddShortcut() => !string.IsNullOrWhiteSpace(NewTrigger) && !string.IsNullOrWhiteSpace(NewReplacement);

    private void AddShortcut()
    {
        Shortcuts.Add(new ShortcutModel { Trigger = NewTrigger, Replacement = NewReplacement });
        ShortcutService.SaveShortcuts(Shortcuts);
        NewTrigger = string.Empty;
        NewReplacement = string.Empty;
    }

    private void RemoveShortcut(object parameter)
    {
        if (parameter is ShortcutModel shortcut)
        {
            Shortcuts.Remove(shortcut);
            ShortcutService.SaveShortcuts(Shortcuts);
        }
    }
}