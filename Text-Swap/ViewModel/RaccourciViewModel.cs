using MaterialDesignThemes.Wpf;
using MVVMEssentials.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Effects;
using Text_Swap.Model;
using Text_Swap.Services;
using Text_Swap.View;

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

    public ICommand ShowShortcutCommand { get; }
    public ICommand RemoveShortcutCommand { get; }

    public RaccourciViewModel()
    {
        Shortcuts = ShortcutHelper.ConvertToObservableCollection(ShortcutService.LoadShortcuts());
        //ShowShortcutCommand = new ViewModelCommand(_ => AddShortcut(), _ => CanAddShortcut());
        ShowShortcutCommand = new ViewModelCommand(ShowShortcutPopup, CanShowShortcutPopup);
        RemoveShortcutCommand = new ViewModelCommand(RemoveShortcut, _ => Shortcuts.Any());
    }

    private bool CanShowShortcutPopup(object obj)
    {
        return true;
    }

    private void ShowShortcutPopup(object obj)
    {
        Window mainWIndow = System.Windows.Application.Current.MainWindow;
        mainWIndow.Effect = new BlurEffect {Radius = 10 };
        AddShortcutView addShortcutPopup = new AddShortcutView(shircut =>
        Shortcuts.Add(shircut));
        addShortcutPopup.Owner = mainWIndow;
        addShortcutPopup.WindowStartupLocation = WindowStartupLocation.CenterScreen;

        addShortcutPopup.ShowDialog();
    }

    private bool CanAddShortcut() => !string.IsNullOrWhiteSpace(NewTrigger) && !string.IsNullOrWhiteSpace(NewReplacement);

    private void AddShortcut()
    {
        Shortcuts.Add(new ShortcutModel { Trigger = NewTrigger, Replacement = NewReplacement });
        ShortcutService.AddShortcut(NewTrigger,  NewReplacement );
        NewTrigger = string.Empty;
        NewReplacement = string.Empty;
    }

    private void RemoveShortcut(object shortcut)
    {
        if (shortcut is ShortcutModel shortcutModel && Shortcuts.Contains(shortcutModel))
        {
            Shortcuts.Remove(shortcutModel);
            ShortcutService.RemoveShortcut(shortcutModel.Trigger);
        }
    }
}