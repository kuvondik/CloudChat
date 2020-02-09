using System.Security;
using System.Windows;

namespace CloudChat
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class RegisterPage : BasePage<RegisterViewModel>, IHavePassword
    {
        public RegisterPage(RegisterViewModel specificViewModel) : base(specificViewModel)
        {
            InitializeComponent();
        }

        public RegisterPage() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The secure password for this login page
        /// </summary>
        public SecureString SecurePassword => PasswordText.SecurePassword;

        private void PasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (ViewModel as RegisterViewModel).Password = PasswordText.SecurePassword;
        }

        private void ConfirmPasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (ViewModel as RegisterViewModel).ConfirmPassword = ConfirmPasswordText.SecurePassword;
        }
    }
}