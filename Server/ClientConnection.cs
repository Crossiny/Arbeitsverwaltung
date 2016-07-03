using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Server.Packages;

namespace Server
{
    internal class ClientConnection
    {
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private readonly TcpClient _tcpClient;
        private string _clientName;
        private bool _isAdmin;
        private bool _isConnected;

        public ClientConnection(TcpClient tcpClient)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (!File.Exists("DataBase.bin"))
                binaryFormatter.Serialize(new FileStream("DataBase.bin", FileMode.CreateNew), new Database.Database());

            Program.Database =
                binaryFormatter.Deserialize(new FileStream("DataBase.bin", FileMode.Open)) as Database.Database;

            _tcpClient = tcpClient;
            Task t = new Task(Start);
            t.Start();
        }

        private void Start()
        {
            object receivedObject = _binaryFormatter.Deserialize(_tcpClient.GetStream());

            if (receivedObject.GetType() == typeof(LoginPackage))
            {
                SendLoginResponse(receivedObject as LoginPackage);
            }
            if (receivedObject.GetType() == typeof(RequestUserDataPackage))
            {
                SendRequestUserDataResponse(receivedObject as RequestUserDataPackage);
            }
            if (receivedObject.GetType() == typeof(AddShiftPackage))
            {
                SendAddShiftResponsePackage(receivedObject as AddShiftPackage);
            }
        }

        private void SendAddShiftResponsePackage(AddShiftPackage addShiftPackage)
        {
            AddShiftResponsePackage addShiftResponsePackage = new AddShiftResponsePackage();
            if (Program.Database.UserExists(_clientName))
            {
                Program.Database.AddShift(_clientName, addShiftPackage.shift);
                addShiftResponsePackage.Success = true;
            }
            else
            {
                addShiftResponsePackage.Success = false;
            }
        }

        private void SendRequestUserDataResponse(RequestUserDataPackage requestUserDataPackage)
        {
            RequestUserDataResponsePackage responsePackage = new RequestUserDataResponsePackage();
            if (Program.Database.UserExists(requestUserDataPackage.Username))
            {
                responsePackage.Success = true;
                responsePackage.UserData = Program.Database.GetUser(requestUserDataPackage.Username);
            }
            else
            {
                responsePackage.Success = false;
                responsePackage.UserData = null;
            }
        }

        private void SendLoginResponse(LoginPackage loginPackage)
        {
            LoginResponsePackage loginResponsePackage = new LoginResponsePackage();
            if (Program.Database.UserExists(loginPackage.Username))
            {
                loginResponsePackage.Success = Program.Database.CheckPassword(loginPackage.Username,
                    loginPackage.Password);
                loginResponsePackage.IsAdmin = Program.Database.IsAdmin(loginPackage.Username);

                Console.CursorLeft = 0;
                Console.WriteLine($"{loginPackage.Username} logged in!");
                _clientName = loginResponsePackage.Username;
                _isConnected = true;
                _isAdmin = loginResponsePackage.IsAdmin;
            }
            else
                loginResponsePackage.Success = false;

            _binaryFormatter.Serialize(_tcpClient.GetStream(), loginResponsePackage);
        }
    }
}