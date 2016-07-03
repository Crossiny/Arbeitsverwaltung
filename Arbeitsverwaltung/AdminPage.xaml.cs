using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Arbeitsverwaltung.Classes;
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

        private void AdminPage_OnMouseEnter(object sender, MouseEventArgs e)
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
                GetUserDataPackage getUserDataPackage = new GetUserDataPackage()
                {
                    Username = user
                };
                binaryFormatter.Serialize(Client.TcpClient.GetStream(), getUserDataPackage);

                GetUserDataResponsePackage getUserDataResponsePackage = binaryFormatter.Deserialize(Client.TcpClient.GetStream()) as GetUserDataResponsePackage;

                MessageBox.Show(getUserDataResponsePackage.User.Username);
            }
        }
    }
}