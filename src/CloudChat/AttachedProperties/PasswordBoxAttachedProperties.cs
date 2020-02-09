using System.Windows;
using System.Windows.Controls;

namespace CloudChat
{
    public class MonitorPasswordProperty : BaseAttachedProperties<MonitorPasswordProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;
            passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

            if ((bool)e.NewValue)
            {
                HasTextProperty.SetValue(passwordBox, passwordBox.SecurePassword.Length > 0);
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            HasTextProperty.SetValue((PasswordBox)sender);
        }
    }

    public class MonitorTextBoxProperty : BaseAttachedProperties<MonitorTextBoxProperty, bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox == null)
                return;
            textBox.TextChanged -= TextBox_TextChanged;

            if ((bool)e.NewValue)
            {
                HasTextProperty.SetValue(textBox, textBox.Text.Length > 0);
                textBox.TextChanged += TextBox_TextChanged;
            }
        }

        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            HasTextProperty.SetValue((TextBox)sender);
        }
    }




    public class HasTextProperty : BaseAttachedProperties<HasTextProperty, bool>
    {
        public static void SetValue(DependencyObject sender)
        {
            if (sender is PasswordBox passwordBox)
                SetValue(passwordBox, passwordBox.SecurePassword.Length > 0);
            else
            if (sender is TextBox textBox)
                SetValue(textBox, textBox.Text.Length > 0);

        }
    }
}