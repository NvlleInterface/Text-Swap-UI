using MVVMEssentials.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Text_Swap.Model;
using Text_Swap.Services;
using Text_Swap.View;

namespace Text_Swap.ViewModel
{
    public class AddShortcutViewModel : ViewModelBase
    {
        public ObservableCollection<ShortcutModel> Shortcuts { get; set; }
        private readonly Action<ShortcutModel> _onShorcutAdded;
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

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(nameof(ErrorMessage)); }
        }

        private bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value; OnPropertyChanged(nameof(IsEditing)); }
        }


        public ICommand AddShortcutCommand { get; }
        public ICommand CancelShortcutCommand { get; }

        public AddShortcutViewModel(Action<ShortcutModel> onShorcutAdded, string trigger = null, bool isEditing = false)
        {
            _onShorcutAdded = onShorcutAdded;
            NewTrigger = trigger;
            IsEditing = isEditing;
            Shortcuts = ShortcutHelper.ConvertToObservableCollection(ShortcutService.LoadShortcuts());
            NewReplacement = Shortcuts.Where(x => x.Trigger.Equals(trigger, StringComparison.OrdinalIgnoreCase)).FirstOrDefault()?.Replacement;
            AddShortcutCommand = new ViewModelCommand(_ => AddShortcut(), _ => CanAddShortcut());
            CancelShortcutCommand = new ViewModelCommand(_ => CancelShortcit(), _=> CanCancelShortcut());
        }

        private void CancelShortcit()
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Vos modifications ne seront pas pris en compte. Voulez vous quitter ?",
                "Confirmation", MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            Window window = System.Windows.Application.Current.Windows.OfType<AddShortcutView>().FirstOrDefault();
            window.Close();
        }

        private bool CanCancelShortcut()
        {
            return true;
        }

        private bool CanAddShortcut()
        {
            if (!IsEditing)
            {
                if (string.IsNullOrWhiteSpace(NewTrigger) || string.IsNullOrWhiteSpace(NewReplacement))
                {
                    ErrorMessage = string.Empty;
                    return false;
                }

                if (Shortcuts.Any(s => s.Trigger.Equals(NewTrigger, StringComparison.OrdinalIgnoreCase)))
                {
                    ErrorMessage = "Ce raccourci existe déjà.";
                    return false;
                }

                ErrorMessage = string.Empty;
            }
           
            return true;
        }

        private void AddShortcut()
        {
            _onShorcutAdded?.Invoke(new ShortcutModel { Trigger = NewTrigger, Replacement = NewReplacement });
            if (IsEditing)
            {
               
                var result = ShortcutService.UpdateShortcut(NewTrigger, NewReplacement);
                if (result)
                {
                    NewTrigger = string.Empty;
                    NewReplacement = string.Empty;
                }
            }
            else
            {
               
                var result = ShortcutService.AddShortcut(NewTrigger, NewReplacement);
                if (result)
                {
                    NewTrigger = string.Empty;
                    NewReplacement = string.Empty;
                }
            }
            
        }
    }
}
