using System.Windows;

namespace Arbeitsverwaltung
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainWindow _thisWindow;
        public MainWindow()
        {
            InitializeComponent();
            _thisWindow = this;
        }

        public static void PrintStatus(string status)
        {
            _thisWindow.InfoBarItem.Content = status;
        }
    }
}