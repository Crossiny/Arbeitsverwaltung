using System;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
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
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect("127.0.0.1", 1337);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(tcpClient.GetStream(), new LoginPackage
            {
                Username = UsernameTextBox.Text,
                Password = PasswordTextBox.Password
            });

            LoginResponsePackage response = binaryFormatter.Deserialize(tcpClient.GetStream()) as LoginResponsePackage;
            MessageBox.Show(response.IsAdmin.ToString());
        }
    }
}