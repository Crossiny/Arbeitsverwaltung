using System;
using System.Net;
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
        public SettingsPage()
        {
            InitializeComponent();
            Settings.Default.Reload();

            IpTextBox.Text = Settings.Default.IP;
            PortTextBox.Text = Settings.Default.Port.ToString();
        }

        public void IpChanged(object sender, RoutedEventArgs e)
        {
            IPAddress ipAddress;
            if (IPAddress.TryParse(IpTextBox.Text, out ipAddress))
            {
                Settings.Default.IP = ipAddress.ToString();
                MainWindow.PrintStatus($"{ipAddress.ToString()} saved!");
            }
            else
            {
                MessageBox.Show($"{IpTextBox.Text} is not a valid IP-Address!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                IpTextBox.Text = Settings.Default.IP;
            }
        }

        public void PortChanged(object sender, RoutedEventArgs e)
        {
            int port;
            if (int.TryParse(PortTextBox.Text, out port) && (port < ushort.MaxValue) && (port > 0))
            {
                Settings.Default.Port = Convert.ToInt32(port);
                Settings.Default.Save();
                MainWindow.PrintStatus($"{port} saved!");
            }
            else
            {
                MessageBox.Show($"{PortTextBox.Text} is not a valid Port!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                PortTextBox.Text = Settings.Default.Port.ToString();
            }
        }
    }
}