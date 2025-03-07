using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Text_Swap.CustomControls;

/// <summary>
/// Logique d'interaction pour BindablePassword.xaml
/// </summary>
public partial class BindablePassword : UserControl
{
    public static readonly DependencyProperty PasswordProperty =
        DependencyProperty.Register("Password", typeof(SecureString), typeof(BindablePassword));

    public static readonly DependencyProperty PlaceholderProperty =
        DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(BindablePassword), new PropertyMetadata(""));

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public SecureString Password
    {
        get { return (SecureString)GetValue(PasswordProperty); }
        set { SetValue(PasswordProperty, value); }
    }
    public BindablePassword()
    {
        InitializeComponent();
        txtPassword.PasswordChanged += OnPasswordChanged;
        txtVisiblePassword.TextChanged += OnTextChanged;
        UpdatePlaceholderVisibility();
    }

    private void OnTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdatePlaceholderVisibility();
    }

    private void UpdatePlaceholderVisibility()
    {
        bool isEmpty = string.IsNullOrEmpty(txtPassword.Password) && string.IsNullOrEmpty(txtVisiblePassword.Text);
        txtPlaceholder.Visibility = isEmpty ? Visibility.Visible : Visibility.Collapsed;
    }

    private void OnPasswordChanged(object sender, RoutedEventArgs e)
    {
        Password = txtPassword.SecurePassword;
        UpdatePlaceholderVisibility();
    }

    private void btnToggleVisibility_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (txtPassword.Visibility == Visibility.Visible)
        {
            txtVisiblePassword.Text = txtPassword.Password;
            txtPassword.Visibility = Visibility.Collapsed;
            txtVisiblePassword.Visibility = Visibility.Visible;
            btnToggleVisibility.Icon = FontAwesome.Sharp.IconChar.EyeSlash;
        }
        else
        {
            txtPassword.Password = txtVisiblePassword.Text;
            txtPassword.Visibility = Visibility.Visible;
            txtVisiblePassword.Visibility = Visibility.Collapsed;
            btnToggleVisibility.Icon = FontAwesome.Sharp.IconChar.Eye;
        }

        UpdatePlaceholderVisibility();
    }
}

