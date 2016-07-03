using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arbeitsverwaltung.Classes;
using Server.Database;
using Server.Packages;

namespace Arbeitsverwaltung
{
    /// <summary>
    ///     Interaktionslogik für AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
        }

        private void UserListBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserListBox.SelectedIndex == -1) return;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            GetUserDataPackage getUserDataPackage = new GetUserDataPackage()
            {
                Username = UserListBox.Items[UserListBox.SelectedIndex].ToString()
            };
            binaryFormatter.Serialize(Client.TcpClient.GetStream(), getUserDataPackage);

            GetUserDataResponsePackage getUserDataResponsePackage = binaryFormatter.Deserialize(Client.TcpClient.GetStream()) as GetUserDataResponsePackage;
            StackPanel.Children.Clear();
            foreach (Shift shift in getUserDataResponsePackage.User.Shifts)
            {
                StackPanel.Children.Add(new ShiftElement(shift, getUserDataResponsePackage.User.Loan));
            }
        }

        private void RefreshUserListButton_OnClick(object sender, RoutedEventArgs e)
        {
            GetUserListPackage getUserListPackage = new GetUserListPackage()
            {
                Username = MainWindow.Ui.UsernameTextBox.Text
            };
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(Client.TcpClient.GetStream(), getUserListPackage);

            GetUserListResponsePackage getUserListResponsePackage = binaryFormatter.Deserialize(Client.TcpClient.GetStream()) as GetUserListResponsePackage;
            foreach (string user in getUserListResponsePackage.UserList)
            {
                UserListBox.Items.Add(user);
            }
        }
    }
}