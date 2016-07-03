using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Arbeitsverwaltung.Properties;

namespace Arbeitsverwaltung
{
    /// <summary>
    ///     Interaktionslogik für SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private static SettingsPage _settingsPage;

        public SettingsPage()
        {
            InitializeComponent();

            _settingsPage = this;
            Settings.Default.Reload();

            DefaultUsernameTextBox.Text = Settings.Default.DefaultUsername;
            DefaultPasswordTextBox.Password = Settings.Default.DefaultPassword;
            AutoLoginCheckBox.IsChecked = Settings.Default.AutoLogin;
            IpTextBox.Text = Settings.Default.IP;
            PortTextBox.Text = Settings.Default.Port.ToString();
        }

        private void UsernameChanged(object sender, RoutedEventArgs e)
        {
            var input = ((TextBox) sender).Text;
            if (input == Settings.Default.DefaultUsername) return;

            // Überprüft ob der Username ungültige Zeichen enthält.
            if (new Regex("[^0-9a-z]", RegexOptions.IgnoreCase).IsMatch(input))
            {
                DefaultUsernameTextBox.Text = Settings.Default.DefaultUsername;
                MessageBox.Show($"{input} is not a valid username! \n(Only letters or digits!)", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            else
            {
                Settings.Default.DefaultUsername = input;
                Settings.Default.Save();
                MainWindow.PrintStatus($"Username: {input} saved!");
            }
        }

        private void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var input = ((PasswordBox) sender).Password;

            // Überprüft ob das Passwort ungültige Zeichen enthält.
            if (new Regex("[^0-9a-z]", RegexOptions.IgnoreCase).IsMatch(input))
            {
                DefaultPasswordTextBox.Password = Settings.Default.DefaultPassword;
                MessageBox.Show($"The entered password is not a valid password! \n(Only letters or digits!)", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (input == Settings.Default.DefaultPassword) return;

                Settings.Default.DefaultPassword = input;
                Settings.Default.Save();
                MainWindow.PrintStatus($"Password saved!");
            }
        }

        private void AutoLoginChanged(object sender, RoutedEventArgs e)
        {
            var autoLogin = (bool) ((CheckBox) sender).IsChecked;
            Settings.Default.AutoLogin = autoLogin;
            Settings.Default.Save();
            MainWindow.PrintStatus($"Autologin {autoLogin}");
        }

        private void IpChanged(object sender, RoutedEventArgs e)
        {
            var input = ((TextBox) sender).Text;
            if ((input == "") || (input == Settings.Default.IP)) return;

            // Überprüft ob eine gültige IP eingegeben wurde.
            IPAddress parsedIpAddress;
            if (IPAddress.TryParse(input, out parsedIpAddress))
            {
                // Bricht ab wenn die neue und die alte IP identisch sind.
                if (input == Settings.Default.IP) return;

                Settings.Default.IP = parsedIpAddress.ToString();
                MainWindow.PrintStatus($"IP-Address: {input} saved!");
            }
            else
            {
                MessageBox.Show($"{input} is not a valid IP-Address!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                IpTextBox.Text = Settings.Default.IP;
            }
        }

        private void PortChanged(object sender, RoutedEventArgs e)
        {
            var input = ((TextBox) sender).Text;
            if (input == "") return;

            // Überprüft ob die Eingabe ein int ist und in der Range von verfügbaren Ports liegt.
            int port;
            if (int.TryParse(PortTextBox.Text, out port) && (port < ushort.MaxValue) && (port > 0))
            {
                // Bricht ab wenn der neue und der alte Port identisch sind.
                if (port == Settings.Default.Port) return;

                Settings.Default.Port = Convert.ToInt32(port);
                Settings.Default.Save();
                MainWindow.PrintStatus($"Port: {port} saved!");
            }
            else
            {
                MessageBox.Show($"{PortTextBox.Text} is not a valid port!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                PortTextBox.Text = Settings.Default.Port.ToString();
            }
        }

        public static void Save()
        {
            _settingsPage.UsernameChanged(_settingsPage.DefaultUsernameTextBox, null);
            _settingsPage.PasswordChanged(_settingsPage.DefaultPasswordTextBox, null);
            _settingsPage.AutoLoginChanged(_settingsPage.AutoLoginCheckBox, null);
            _settingsPage.IpChanged(_settingsPage.IpTextBox, null);
            _settingsPage.PortChanged(_settingsPage.PortTextBox, null);
        }
    }
}