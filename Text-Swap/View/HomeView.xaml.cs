using FontAwesome.Sharp;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Text_Swap.Model;
using Text_Swap.Services;
using Text_Swap.ViewModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Text_Swap.View
{
    /// <summary>
    /// Logique d'interaction pour HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void textBox_KeyUp(object sender, KeyEventArgs e)
        {
            string searchText = textBox.Text.Trim();

            
            var shortcuts = ShortcutHelper.ConvertToObservableCollection(ShortcutService.LoadShortcuts());
            resultStack.Children.Clear();

            var filteredResults = shortcuts.Where(item => item.Trigger.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();

            if (filteredResults.Any())
            {
                popupResults.IsOpen = true;

                foreach (var item in filteredResults)
                {
                    StackPanel itemPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(5) };

                    // Icône FontAwesome pour chaque item
                    IconImage icon = new IconImage
                    {
                        Icon = IconChar.FontAwesome,
                        Foreground = Brushes.Green,
                        Width = 16,
                        Height = 16,
                        Margin = new Thickness(0, 0, 5, 0)
                    };

                    TextBlock resultItem = new TextBlock
                    {
                        Text = item.Trigger,
                        Padding = new Thickness(5),
                        Cursor = Cursors.Hand
                    };
                    resultItem.MouseLeftButtonUp += (s, args) =>
                    {
                        textBox.Text = ((TextBlock)s).Text;
                        popupResults.IsOpen = false;

                        
                        //viewModel.SelectedShortcut = item;

                        ShowPopupWithShortcut(item.Trigger);

                    };

                    itemPanel.Children.Add(icon);
                    itemPanel.Children.Add(resultItem);
                    resultStack.Children.Add(itemPanel);
                }
            }
            else
            {
                popupResults.IsOpen = false;
            }
        }

        private void ShowPopupWithShortcut(string trigger)
        {
            var popup = new AddShortcutView(shortcut =>
            {
                ShortcutHelper.ConvertToObservableCollection(ShortcutService.LoadShortcuts()).Add(shortcut as ShortcutModel);
            });

            popup.DataContext = new AddShortcutViewModel(shortcut =>
            {

                popup.Close();
            }, trigger, true);
            
            popup.Owner = Application.Current.MainWindow;
            popup.ShowDialog();
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is string selectedShortcut)
            {
                ShowPopupWithShortcut(selectedShortcut);
            }
        }

        private void OpenAddShortcutView(string trigger)
        {
            throw new NotImplementedException();
        }

    }
}
