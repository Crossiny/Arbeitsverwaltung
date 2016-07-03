using System;
using System.Data.SqlTypes;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using Arbeitsverwaltung.Classes;
using Server.Database;
using Server.Packages;

namespace Arbeitsverwaltung
{
    /// <summary>
    ///     Interaktionslogik für UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private Shift _shift;
        private Break _break;

        private BinaryFormatter _binaryFormatter = new BinaryFormatter();

        public UserPage()
        {
            InitializeComponent();
        }

        private void StartShiftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shift == null)
            {
                _shift = new Shift();
                _shift.StartTime = DateTime.Now;

                MainWindow.PrintStatus("Shift is started.");
            }
            else if (_shift != null)
            {
                MainWindow.PrintStatus("Shift can't be started because you already having a Shift.");
            }
        }

        private void StopShiftButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_shift != null)
            {
                _shift.EndTime = DateTime.Now;

                AddShiftPackage addShiftPackage = new AddShiftPackage()
                {
                    shift = _shift
                };

                //send
                _binaryFormatter.Serialize(Client.TcpClient.GetStream(), addShiftPackage);

                MainWindow.PrintStatus("Shift is stopped.");
            }
            else if (_shift == null)
            {
                MainWindow.PrintStatus("You can't stop a Shift that hasn´t started.");
            }
        }

        private void StartBreakButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_break == null && _shift != null)
            {
                _break = new Break();
                _break.StartTime = DateTime.Now;
                MainWindow.PrintStatus("Break is started.");
            }
            else if (_break != null)
            {
                MainWindow.PrintStatus("Break can't be started because you already having a Break.");
            }
            else if (_shift == null)
            {
                MainWindow.PrintStatus("You can't start a break if you haven´t started your shift.");
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
                MainWindow.PrintStatus("You can't stop a break if you don´t have a break.");
            }
            else if (_shift == null)
            {
                MainWindow.PrintStatus("You can't stop a break if you haven´t started your.");
            }
        }
    }
}