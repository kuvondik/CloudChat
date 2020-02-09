using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for PasswordEntryControl.xaml
    /// </summary>
    public partial class PasswordEntryControl : UserControl
    {
        public PasswordEntryControl()
        {
            InitializeComponent();
        }

        public GridLength LabelWidth
        {
            get => (GridLength)GetValue(LabelWidthProperty);
            set => SetValue(LabelWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for LabelWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LabelWidthProperty =
            DependencyProperty.Register("LabelWidth", typeof(GridLength), typeof(PasswordEntryControl), new PropertyMetadata(GridLength.Auto, LabelWidthChangedCallback));


        #region Dependency Callbacks
        public static void LabelWidthChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                (d as PasswordEntryControl).LabelColumnDefinition.Width = (GridLength)e.NewValue;
            }
            catch
            {
                Debugger.Break();
                (d as PasswordEntryControl).LabelColumnDefinition.Width = GridLength.Auto;

            }

        }
        #endregion

        private void CurrentPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PasswordEntryViewModel passwordEntryViewModel)
                passwordEntryViewModel.CurrentPassword = currentPassword.SecurePassword;
        }

        private void NewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PasswordEntryViewModel passwordEntryViewModel)
                passwordEntryViewModel.NewPassword = newPassword.SecurePassword;
        }

        private void ConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is PasswordEntryViewModel passwordEntryViewModel)
                passwordEntryViewModel.ConfirmPassword = confirmPassword.SecurePassword;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            currentPassword.Password = "";
            newPassword.Password = "";
            confirmPassword.Password = "";
        }
    }
}
