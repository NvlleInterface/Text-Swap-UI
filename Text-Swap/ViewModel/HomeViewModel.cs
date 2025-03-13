using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Text_Swap.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Text_Swap.Model;
using MVVMEssentials.ViewModels;
using Microsoft.Xaml.Behaviors.Input;
using System.Windows.Forms;
using System.Windows;
using Text_Swap.View;
using System.Windows.Media.Effects;
using System.Security.Principal;

namespace Text_Swap.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private string _searchText;
        private string _shortcutInput;
        private string _expandedText;
        private TimeSpan _timeSaved;
        private string _newTrigger;
        private string _newReplacement;

        private ObservableCollection<ShortcutModel> _filteredShortcuts;

        public ObservableCollection<ShortcutModel> Shortcuts { get; set; }
        public ObservableCollection<string> RecentShortcuts { get; set; }
        public ObservableCollection<ShortcutModel> FilteredShortcuts
        {
            get => _filteredShortcuts;
            set { _filteredShortcuts = value; OnPropertyChanged(); }
        }

        private string _welcome;

        public string Welcome
        {
            get { return _welcome; }
            set { _welcome = value; OnPropertyChanged(nameof(Welcome)); }
        }


        public string NewTrigger
        {
            get => _newTrigger;
            set { _newTrigger = value; OnPropertyChanged(nameof(NewTrigger)); }
        }
        
        public string NewReplacement
        {
            get => _newReplacement;
            set { _newReplacement = value; OnPropertyChanged(nameof(NewReplacement)); }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilterShortcuts();
            }
        }

        public string ShortcutInput
        {
            get => _shortcutInput;
            set
            {
                _shortcutInput = value;
                OnPropertyChanged(nameof(ShortcutInput));
                CheckForTrigger();
            }
        }

        public string ExpandedText
        {
            get => _expandedText;
            set { _expandedText = value; OnPropertyChanged(nameof(ExpandedText)); }
        }

        public TimeSpan TimeSaved
        {
            get => _timeSaved;
            set { _timeSaved = value; OnPropertyChanged(nameof(TimeSaved)); }
        }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }
        public List<string> LastTriggers { get; set; } = new List<string>();

        public ICommand SelectShortcutCommand { get; }
        public ICommand SelectRecentShortcutCommand { get; }

        public ICommand AddShortcutCommand { get; }

        public ICommand OpenShortcutCommand { get; }

        public HomeViewModel()
        {
            Shortcuts = new ObservableCollection<ShortcutModel>(
                ShortcutService.LoadShortcuts()
                .Select(kv => new ShortcutModel { Trigger = kv.Key, Replacement = kv.Value })
            );

            FilteredShortcuts = new ObservableCollection<ShortcutModel>();
            RecentShortcuts = new ObservableCollection<string>();

            SelectShortcutCommand = new ViewModelCommand(param => SelectShortcut(param as string));
            SelectRecentShortcutCommand = new ViewModelCommand(param => SelectRecentShortcut(param as string));

            AddShortcutCommand = new ViewModelCommand(_ => AddShortcut(), _ => CanAddShortcut());
            OpenShortcutCommand = new ViewModelCommand(ShowShortcutPopup, CanShowShortcutPopup);
        }

        private bool CanShowShortcutPopup(object obj)
        {
            return true;
        }

        private void ShowShortcutPopup(object obj)
        {
            var trigger = obj as string;
            Window mainWIndow = System.Windows.Application.Current.MainWindow;
            mainWIndow.Effect = new BlurEffect { Radius = 10 };
            AddShortcutView addShortcutPopup = new AddShortcutView(shircut =>
            Shortcuts.Add(shircut), trigger, true);
            addShortcutPopup.Owner = mainWIndow;
            addShortcutPopup.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            addShortcutPopup.ShowDialog();
        }

        private void AddShortcut()
        {
            Shortcuts.Add(new ShortcutModel { Trigger = NewTrigger, Replacement = NewReplacement });
            var result = ShortcutService.AddShortcut(NewTrigger, NewReplacement);

            if (result)
            {

                // Ajouter aux derniers raccourcis utilisés
                if (!string.IsNullOrWhiteSpace(NewTrigger) && !LastTriggers.Contains(NewTrigger))
                {
                    LastTriggers.Add(NewTrigger);
                    RecentShortcuts.Insert(0, NewTrigger);
                }
                NewTrigger = string.Empty;
                NewReplacement = string.Empty;
            }
        }

        private bool CanAddShortcut()
        {
            return true;
        }

        private void FilterShortcuts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredShortcuts.Clear();
                return;
            }

            var filtered = Shortcuts.Where(s => s.Trigger.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
            FilteredShortcuts = new ObservableCollection<ShortcutModel>(filtered);
        }

        private void CheckForTrigger()
        {
          if (string.IsNullOrWhiteSpace(ShortcutInput)) return;

            // Vérifier si l'utilisateur tape un espace après un trigger existant
            if (Regex.IsMatch(ShortcutInput, @"\s$"))
            {
                string cleanTrigger = ShortcutInput.Trim();
                var shortcut = Shortcuts.FirstOrDefault(s => s.Trigger.Equals(cleanTrigger, StringComparison.OrdinalIgnoreCase));

                if (shortcut != null)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    ExpandedText = shortcut.Replacement;
                    stopwatch.Stop();

                    // Calcul du temps économisé (simulation : 100 ms * longueur du texte remplacé)
                    TimeSaved = TimeSaved.Add(TimeSpan.FromMilliseconds(shortcut.Replacement.Length * 100));

                    App.Current.Dispatcher.Invoke(() =>
                    {
                        OnPropertyChanged(nameof(ExpandedText));
                        OnPropertyChanged(nameof(TimeSaved));
                    });

                    // Ajouter aux derniers raccourcis utilisés
                    if (!LastTriggers.Contains(shortcut.Trigger))
                    {
                        LastTriggers.Add(shortcut.Trigger);
                        RecentShortcuts.Insert(0, shortcut.Trigger);
                    }
                }
            }
        }

        private void SelectShortcut(string trigger)
        {
            var shortcut = Shortcuts.FirstOrDefault(s => s.Trigger == trigger);
            if (shortcut != null)
            {
                ExpandedText = shortcut.Replacement;
            }
        }

        private void SelectRecentShortcut(string trigger)
        {
            SelectShortcut(trigger);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
