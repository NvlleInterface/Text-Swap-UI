using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Text_Swap.Services;
using Text_Swap.Model;
using MVVMEssentials.ViewModels;

namespace Text_Swap.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        // Propriétés
        private string _shortcutInput;
        public string ShortcutInput
        {
            get => _shortcutInput;
            set
            {
                _shortcutInput = value;
                
                UpdateExpandedText();
                if (_shortcutInput.EndsWith(" "))
                {
                    // Si un espace est ajouté, mettre à jour ExpandedText
                    var input = _shortcutInput.Trim();
                    var shortcut = Shortcuts.FirstOrDefault(s => s.Trigger == input);
                    if (shortcut != null)
                    {
                        ExpandedText = shortcut.Replacement;
                        AddToRecentShortcuts(shortcut.Trigger);
                    }
                }
                OnPropertyChanged(nameof(ShortcutInput));
            }
        }

        private string _expandedText;
        public string ExpandedText
        {
            get => _expandedText;
            set
            {
                _expandedText = value;
                OnPropertyChanged(nameof(ExpandedText));
            }
        }

        private string _timeSaved;
        public string TimeSaved
        {
            get => _timeSaved;
            set
            {
                _timeSaved = value;
                OnPropertyChanged(nameof(TimeSaved));
            }
        }

        public ObservableCollection<ShortcutModel> Shortcuts { get; set; } = new ObservableCollection<ShortcutModel>();
        public ObservableCollection<ShortcutModel> FilteredShortcuts { get; set; } = new ObservableCollection<ShortcutModel>();
        public ObservableCollection<string> RecentShortcuts { get; set; } = new ObservableCollection<string>();
        public List<string> LastTriggers { get; set; } = new List<string>();

        private Stopwatch _stopwatch;
        private DateTime _lastKeyPressTime;

        public ICommand AddShortcutCommand { get; }
        public ICommand ClearSearchCommand { get; }

        public HomeViewModel()
        {
            // Initialisation des commandes
            AddShortcutCommand = new ViewModelCommand(AddShortcut);
            ClearSearchCommand = new ViewModelCommand(ClearSearch);

            // Charger les raccourcis du service
            var shortcutsFromFile = ShortcutService.LoadShortcuts();
            foreach (var kvp in shortcutsFromFile)
            {
                Shortcuts.Add(new ShortcutModel { Trigger = kvp.Key, Replacement = kvp.Value });
            }

            // Initialisation du stopwatch pour calculer le temps économisé
            _stopwatch = new Stopwatch();
        }

        // Méthode pour filtrer les raccourcis en fonction de la recherche
        public void FilterShortcuts(string searchText)
        {
            FilteredShortcuts.Clear();
            var filtered = Shortcuts.Where(s => s.Trigger.Contains(searchText)).ToList();
            foreach (var shortcut in filtered)
            {
                FilteredShortcuts.Add(shortcut);
            }

            // Démarrer le chronomètre si nécessaire
            if (!string.IsNullOrEmpty(searchText) && !_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                _lastKeyPressTime = DateTime.Now;
            }
            else if (string.IsNullOrEmpty(searchText) && _stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                TimeSaved = $"{_stopwatch.Elapsed.TotalSeconds:F2} sec";
                _stopwatch.Reset();
            }
        }

        // Met à jour ExpandedText avec le Replacement d'un trigger sélectionné
        public void UpdateExpandedText()
        {
            var input = ShortcutInput.Trim();
            var shortcut = Shortcuts.FirstOrDefault(s => s.Trigger == input);
            if (shortcut != null)
            {
                ExpandedText = shortcut.Replacement;
            }
        }

        // Ajouter un raccourci à la liste des raccourcis récents
        private void AddToRecentShortcuts(string trigger)
        {
            if (!LastTriggers.Contains(trigger))
            {
                LastTriggers.Add(trigger);
                if (LastTriggers.Count > 5) // Limiter la taille de la liste à 5
                {
                    LastTriggers.RemoveAt(0);
                }
            }

            RecentShortcuts.Clear();
            foreach (var triggerItem in LastTriggers)
            {
                RecentShortcuts.Add(triggerItem);
            }
        }

        // Ajouter un raccourci via la commande
        private void AddShortcut(object parameter)
        {
            if (!string.IsNullOrEmpty(ShortcutInput) && !string.IsNullOrEmpty(ExpandedText))
            {
                var success = ShortcutService.AddShortcut(ShortcutInput, ExpandedText);
                if (success)
                {
                    // Actualiser les raccourcis dans la vue
                    Shortcuts.Add(new ShortcutModel { Trigger = ShortcutInput, Replacement = ExpandedText });
                }
            }
        }

        // Vider la recherche
        private void ClearSearch(object parameter)
        {
            ShortcutInput = string.Empty;
            FilteredShortcuts.Clear();
        }

        // INotifyPropertyChanged pour notifier les mises à jour de la vue
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
