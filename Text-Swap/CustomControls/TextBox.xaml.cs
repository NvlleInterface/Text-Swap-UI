using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Text_Swap.CustomControls
{
    /// <summary>
    /// Logique d'interaction pour TextBox.xaml
    /// </summary>
    public partial class TextBox : UserControl
    {
        // ✅ Définition de la propriété Placeholder
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register(nameof(Placeholder), typeof(string), typeof(TextBox), new PropertyMetadata(""));

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        // ✅ Définition de la propriété Text (liaison avec la TextBox)
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(TextBox),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // ✅ Propriété Effect (ajout d'un effet visuel)
        public static readonly DependencyProperty EffectProperty =
            DependencyProperty.Register(nameof(Effect), typeof(Effect), typeof(TextBox), new PropertyMetadata(null, OnEffectChanged));
        public Effect Effect
        {
            get => (Effect)GetValue(EffectProperty);
            set => SetValue(EffectProperty, value);
        }

        // ✅ Propriété CornerRadius pour arrondir les coins
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(TextBox),
                new PropertyMetadata(new CornerRadius(0), OnCornerRadiusChanged));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        // ✅ Propriété VerticalAlignment
        public static readonly DependencyProperty VerticalAlignmentProperty =
            DependencyProperty.Register(nameof(VerticalAlignment), typeof(VerticalAlignment), typeof(TextBox),
                new PropertyMetadata(VerticalAlignment.Stretch, OnVerticalAlignmentChanged));

        public new VerticalAlignment VerticalAlignment
        {
            get => (VerticalAlignment)GetValue(VerticalAlignmentProperty);
            set => SetValue(VerticalAlignmentProperty, value);
        }

        private static void OnVerticalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox customTextBox)
            {
                customTextBox.borderContainer.VerticalAlignment = (VerticalAlignment)e.NewValue;
            }
        }

        // ✅ Propriété HorizontalAlignment
        public static readonly DependencyProperty HorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(HorizontalAlignment), typeof(HorizontalAlignment), typeof(TextBox),
                new PropertyMetadata(HorizontalAlignment.Stretch, OnHorizontalAlignmentChanged));

        public new HorizontalAlignment HorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(HorizontalAlignmentProperty);
            set => SetValue(HorizontalAlignmentProperty, value);
        }

        private static void OnHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox customTextBox)
            {
                customTextBox.borderContainer.HorizontalAlignment = (HorizontalAlignment)e.NewValue;
            }
        }

        // ✅ Propriété pour l'alignement vertical du Placeholder
        public static readonly DependencyProperty PlaceholderVerticalAlignmentProperty =
            DependencyProperty.Register(nameof(PlaceholderVerticalAlignment), typeof(VerticalAlignment), typeof(TextBox),
                new PropertyMetadata(VerticalAlignment.Center, OnPlaceholderVerticalAlignmentChanged));

        public VerticalAlignment PlaceholderVerticalAlignment
        {
            get => (VerticalAlignment)GetValue(PlaceholderVerticalAlignmentProperty);
            set => SetValue(PlaceholderVerticalAlignmentProperty, value);
        }

        private static void OnPlaceholderVerticalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox customTextBox)
            {
                customTextBox.txtPlaceholder.VerticalAlignment = (VerticalAlignment)e.NewValue;
            }
        }

        // ✅ Propriété pour l'alignement horizontal du Placeholder
        public static readonly DependencyProperty PlaceholderHorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(PlaceholderHorizontalAlignment), typeof(HorizontalAlignment), typeof(TextBox),
                new PropertyMetadata(HorizontalAlignment.Left, OnPlaceholderHorizontalAlignmentChanged));

        public HorizontalAlignment PlaceholderHorizontalAlignment
        {
            get => (HorizontalAlignment)GetValue(PlaceholderHorizontalAlignmentProperty);
            set => SetValue(PlaceholderHorizontalAlignmentProperty, value);
        }

        private static void OnPlaceholderHorizontalAlignmentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is    TextBox customTextBox)
            {
                customTextBox.txtPlaceholder.HorizontalAlignment = (HorizontalAlignment)e.NewValue;
            }
        }
        private static void OnCornerRadiusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox customTextBox)
            {
                customTextBox.CornerRadius = (CornerRadius)e.NewValue;
            }
        }

        private static void OnEffectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox customTextBox)
            {
                customTextBox.Effect = e.NewValue as Effect;
            }
        }

        // ✅ Propriété pour la marge de la TextBox
        public static readonly DependencyProperty TextBoxMarginProperty =
            DependencyProperty.Register(nameof(TextBoxMargin), typeof(Thickness), typeof(TextBox),
                new PropertyMetadata(new Thickness(0)));

        public Thickness TextBoxMargin
        {
            get => (Thickness)GetValue(TextBoxMarginProperty);
            set => SetValue(TextBoxMarginProperty, value);
        }


        public TextBox()
        {
            InitializeComponent();
            txtInput.TextChanged += OnTextChanged;
            txtInput.SizeChanged += OnSizeChanged;
            txtInput.SetBinding(TextBox.TextProperty, new System.Windows.Data.Binding("Text")
            {
                Source = this,
                Mode = System.Windows.Data.BindingMode.TwoWay
            });

            UpdatePlaceholderVisibility();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();
        }
        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            txtPlaceholder.MaxWidth = txtInput.ActualWidth; // Mise à jour du placeholder
        }


        private void UpdatePlaceholderVisibility()
        {
            txtPlaceholder.Visibility = string.IsNullOrEmpty(txtInput.Text) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
