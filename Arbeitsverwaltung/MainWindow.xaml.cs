using System;
using System.CodeDom.Compiler;
using System.Windows;
using System.Windows.Media;
using Arbeitsverwaltung.Properties;

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
    }
}