using System;
using System.Data.SqlTypes;
using System.Windows;
using System.Windows.Controls;
using Server.Database;

namespace Arbeitsverwaltung
{
    /// <summary>
    ///     Interaktionslogik für UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private Shift _shift;
        private Break _break;

        public UserPage()
        {
            InitializeComponent();
        }

        private void StartShiftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shift == null)
            {
                _shift.StartTime = DateTime.Now;

                MainWindow.PrintStatus("Shift is started.");
            }
            else if (_shift != null)
            {
                MainWindow.PrintStatus("Shift can't started because you already having a Shift.");
            }
        }

        private void StopShiftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shift != null)
            {
                _shift.EndTime = DateTime.Now;

                MainWindow.PrintStatus("Shift is stopped.");
            }
            else if (_shift == null)
            {
                MainWindow.PrintStatus("You can't stop a Shift before you have no Shift.");
            }
        }

        private void StartBreakButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_break == null && _shift != null)
            {
                _break.StartTime = DateTime.Now;
                MainWindow.PrintStatus("Break is started.");
            }
            else if (_break != null)
            {
                MainWindow.PrintStatus("Break can't started because you already having a Break.");
            }
            else if (_shift == null)
            {
                MainWindow.PrintStatus("You can't start a break before you have no Shift.");
            }

        }

        private void StopBreakButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_break != null && _shift != null)
            {
                _break.EndTime = DateTime.Now;
                _shift.Breaks.Add(_break);
                _break = null;

                MainWindow.PrintStatus("Break is stopped.");
            }
            else if (_break == null)
            {
                MainWindow.PrintStatus("You can't stop a break before you have no Break.");
            }
            else if (_shift == null)
            {
                MainWindow.PrintStatus("You can't stop a break before you have no Shift and no Break.");
            }
        }
    }
}