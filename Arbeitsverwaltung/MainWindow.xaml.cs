using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Arbeitsverwaltung.Classes;
using Arbeitsverwaltung.Properties;
using Server.Packages;

namespace Arbeitsverwaltung
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Ui;
        private Client _client = new Client();

        public MainWindow()
        {
            InitializeComponent();
            Ui = this;
            UsernameTextBox.Text = Settings.Default.DefaultUsername;
            PasswordTextBox.Password = Settings.Default.DefaultPassword;
        }

        public static void PrintStatus(string status)
        {
            if (Ui == null) return;
            Ui.InfoBarItem.Content = status;
        }

        private void OnClosing(object sender, EventArgs e)
        {
            SettingsPage.Save();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            bool? isAdmin = _client.Login(UsernameTextBox.Text, PasswordTextBox.Password);
            if (isAdmin == true)
            {
                AdminItem.IsEnabled = true;
                UserItem.IsEnabled = true;
            }
            else if (isAdmin == false)
            {
                UserItem.IsEnabled = true;
                AdminItem.IsEnabled = false;
            }
        }

        private void RegisterButton_OnClick(object sender, RoutedEventArgs e)
        {
            _client.Register(UsernameTextBox.Text, PasswordTextBox.Password);
        }
    }
}