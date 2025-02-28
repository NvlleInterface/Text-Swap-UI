using GalaSoft.MvvmLight.Command;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
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
//public class HomeViewModel : ViewModelBase
//{
//    private string _text;
//    private string _shortcutTrigger;
//    private string _shortcutReplacement;
//    private readonly ShortcutService1 _shortcutService;

//    public HomeViewModel()
//    {
//        _shortcutService = new ShortcutService1();
//        Text = "";
//        AddShortcutCommand = new ViewModelCommand(AddShortcut);
//    }

//    public string Text
//    {
//        get => _text;
//        set
//        {
//            _text = value;
//            OnPropertyChanged();
//        }
//    }

//    public string ShortcutTrigger
//    {
//        get => _shortcutTrigger;
//        set { _shortcutTrigger = value; OnPropertyChanged(); }
//    }

//    public string ShortcutReplacement
//    {
//        get => _shortcutReplacement;
//        set { _shortcutReplacement = value; OnPropertyChanged(); }
//    }

//    public ICommand AddShortcutCommand { get; }

//    private void AddShortcut(object obj)
//    {
//        if (!string.IsNullOrWhiteSpace(ShortcutTrigger) && !string.IsNullOrWhiteSpace(ShortcutReplacement))
//        {
//            _shortcutService.AddShortcut(ShortcutTrigger, ShortcutReplacement);
//            ShortcutTrigger = "";
//            ShortcutReplacement = "";
//        }
//    }

//    public void HandleKeyPress(Key key)
//    {
//        if (key == Key.Space)
//        {
//            Text = _shortcutService.CheckForShortcut(Text);
//        }
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected void OnPropertyChanged([CallerMemberName] string name = null) =>
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

//}

// HomeView.xaml.cs

//public class HomeViewModel : ViewModelBase
//{
//    private string _shortcutInput = string.Empty;
//    private string _fullExpression = string.Empty;
//    private string _selectedCollection;

//    private Stopwatch _stopwatch = new Stopwatch();
//    private TimeSpan _timeSaved = TimeSpan.Zero;

//    public string TimeSaved => _timeSaved.ToString(@"hh\:mm\:ss");
//    public string ShortcutInput
//    {
//        get => _shortcutInput;
//        set
//        {
//            _shortcutInput = value;
//            OnPropertyChanged();
//            OnPropertyChanged(nameof(IsShortcutEmpty));
//        }
//    }

//    public string FullExpression
//    {
//        get => _fullExpression;
//        set
//        {
//            _fullExpression = value;
//            OnPropertyChanged();
//        }
//    }

//    public string SelectedCollection
//    {
//        get => _selectedCollection;
//        set
//        {
//            _selectedCollection = value;
//            OnPropertyChanged();
//        }
//    }

//    public bool IsShortcutEmpty => string.IsNullOrWhiteSpace(ShortcutInput);

//    public ObservableCollection<string> RecentShortcuts { get; set; }
//    public ObservableCollection<string> Collections { get; set; }

//    public ICommand OpenShortcutCommand { get; }
//    public ICommand SelectCollectionCommand { get; }

//    public HomeViewModel()
//    {
//        RecentShortcuts = new ObservableCollection<string>();
//        Collections = new ObservableCollection<string> { "Réclamation", "FAQ", "Support client", "Retard commande" };

//        OpenShortcutCommand = new ViewModelCommand(OpenShortcut);
//        SelectCollectionCommand = new ViewModelCommand(SelectCollection);

//        LoadRecentShortcuts();
//    }

//    private void LoadRecentShortcuts()
//    {
//        var shortcuts = ShortcutService.LoadShortcuts();
//        RecentShortcuts.Clear();
//        foreach (var shortcut in shortcuts)
//        {
//            RecentShortcuts.Add(shortcut.Trigger);
//        }
//    }

//    public void UpdateExpression()
//    {
//        var shortcuts = ShortcutService.LoadShortcuts();
//        var found = shortcuts.Find(s => s.Trigger == ShortcutInput.Trim());

//        if (found != null)
//        {
//            _stopwatch.Stop(); // Arrêter le chrono quand la substitution est faite

//            FullExpression = found.Replacement;

//            // Calcul du temps estimé de frappe manuelle
//            double estimatedTypingTime = found.Replacement.Length * 0.25; // 0.25s par caractère

//            // Temps réellement pris avec le raccourci
//            double actualTimeUsed = _stopwatch.Elapsed.TotalSeconds;

//            // Calcul du temps économisé
//            double savedTime = estimatedTypingTime - actualTimeUsed;
//            if (savedTime > 0)
//            {
//                _timeSaved = _timeSaved.Add(TimeSpan.FromSeconds(savedTime));
//            }

//            OnPropertyChanged(nameof(TimeSaved));

//            // Met à jour la liste des derniers raccourcis
//            if (!RecentShortcuts.Contains(found.Trigger))
//            {
//                RecentShortcuts.Insert(0, found.Trigger);
//            }
//        }
//    }

//    public void StartShortcutTimer()
//    {
//        _stopwatch.Restart(); // Démarre le chronomètre quand l'utilisateur commence à taper
//    }

//    public void SelectRecentShortcut(string trigger)
//    {
//        var shortcuts = ShortcutService.LoadShortcuts();
//        var found = shortcuts.Find(s => s.Trigger == trigger);
//        if (found != null)
//        {
//            FullExpression = found.Replacement;
//        }
//    }

//    private void OpenShortcut(object obj)
//    {
//        System.Windows.MessageBox.Show("Fonction d'ouverture de raccourci en cours de développement !", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//    }

//    private void SelectCollection(object obj)
//    {
//        if (obj is string collection)
//        {
//            SelectedCollection = collection;
//            System.Windows.MessageBox.Show($"Collection '{collection}' sélectionnée.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
//        }
//    }

//    public event PropertyChangedEventHandler PropertyChanged;
//    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }
//}

public class HomeViewModel : ViewModelBase
{
    private string _shortcutInput;
    private string _expandedText;
    private double _timeSaved;
    private List<ShortcutModel> _shortcuts;
    private ObservableCollection<string> _recentShortcuts;

    public event PropertyChangedEventHandler PropertyChanged;

    public string ShortcutInput
    {
        get => _shortcutInput;
        set
        {
            _shortcutInput = value;
            OnPropertyChanged(nameof(ShortcutInput));
        }
    }

    public string ExpandedText
    {
        get => _expandedText;
        set
        {
            _expandedText = value;
            OnPropertyChanged(nameof(ExpandedText));
        }
    }

    public double TimeSaved
    {
        get => _timeSaved;
        set
        {
            _timeSaved = value;
            OnPropertyChanged(nameof(TimeSaved));
        }
    }

    public ObservableCollection<string> RecentShortcuts
    {
        get => _recentShortcuts;
        set
        {
            _recentShortcuts = value;
            OnPropertyChanged(nameof(RecentShortcuts));
        }
    }

    public ICommand ProcessShortcutCommand { get; }
    public ICommand SelectRecentShortcutCommand { get; }

    public HomeViewModel()
    {
        _shortcuts = LoadShortcuts();
        _recentShortcuts = new ObservableCollection<string>();
        ProcessShortcutCommand = new ViewModelCommand(ProcessShortcut);
        SelectRecentShortcutCommand = new RelayCommand<string>(SelectRecentShortcut);
    }

    private List<ShortcutModel> LoadShortcuts()
    {
        try
        {
            //    var config = new ConfigurationBuilder()
            //            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory) // Définit le répertoire de base
            //            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Charge le fichier JSON
            //            .Build();
            //    var json = config["ShortcutsFilePath"];
            //    return JsonSerializer.Deserialize<ObservableCollection<ShortcutModel>>(json) ?? new ObservableCollection<ShortcutModel>();
            //}
            return ShortcutService.LoadShortcuts();
        }
        catch (Exception ex)
        {
            return new List<ShortcutModel>();
        }
    }

    private void ProcessShortcut(object obj)
    {
        var match = _shortcuts.FirstOrDefault(s => s.Trigger == ShortcutInput.Trim());
        if (match != null)
        {
            ExpandedText = match.Replacement;
            TimeSaved += match.Replacement.Length * 0.2; // Supposons 0.2 sec par caractère
            if (!RecentShortcuts.Contains(ShortcutInput))
            {
                RecentShortcuts.Insert(0, ShortcutInput);
                if (RecentShortcuts.Count > 5) RecentShortcuts.RemoveAt(5);
            }
            ShortcutInput = string.Empty;
        }
    }

    private void SelectRecentShortcut(string shortcut)
    {
        var match = _shortcuts.FirstOrDefault(s => s.Trigger == shortcut);
        if (match != null)
        {
            ExpandedText = match.Replacement;
        }
    }

    protected void OnPropertyChanged(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}