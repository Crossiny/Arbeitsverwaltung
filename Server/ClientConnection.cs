using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Server.Database;
using Server.Packages;

namespace Server
{
    internal class ClientConnection
    {
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private readonly TcpClient _tcpClient;
        private string _clientName;
        private bool _isAdmin;
        private bool _loggedIn = false;
        public ClientConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            Task t = new Task(Start);
            t.Start();
        }

        private void Start()
        {
            while (_tcpClient.Connected)
            {
                object receivedObject = _binaryFormatter.Deserialize(_tcpClient.GetStream());

                if (receivedObject.GetType() == typeof(RegisterPackage))
                    SendRegisterResponse(receivedObject as RegisterPackage);

                if (receivedObject.GetType() == typeof(LoginPackage))
                    SendLoginResponse(receivedObject as LoginPackage);

                if (receivedObject.GetType() == typeof(RequestUserDataPackage))
                    SendRequestUserDataResponse(receivedObject as RequestUserDataPackage);

                if (receivedObject.GetType() == typeof(AddShiftPackage))
                    SendAddShiftResponsePackage(receivedObject as AddShiftPackage);

                if (receivedObject.GetType() == typeof(LogoutPackage))
                    SendLogoutResponse();

                if (receivedObject.GetType() == typeof(GetUserListPackage))
                    SendGetUserDataListResponsePackage(receivedObject as GetUserListPackage);
            }
        }

        private void SendGetUserDataListResponsePackage(GetUserListPackage getUserListPackage)
        {
            GetUserListResponsePackage getUserListResponsePackage = new GetUserListResponsePackage();
            if (Program.Database.GetIsAdmin(getUserListPackage.Username))
            {
                List<string> userList = new List<string>();
                foreach (KeyValuePair<string, User> keyValuePair in Program.Database.UserDictionary)
                {
                    userList.Add(keyValuePair.Key);
                }
                getUserListResponsePackage.UserList = userList;
            }
            _binaryFormatter.Serialize(_tcpClient.GetStream(), getUserListResponsePackage);
        }

        public void Close()
        {
            _tcpClient.Close();
        }

        private void SendLogoutResponse()
        {
            LogoutResponsePackage logoutResponsePackage = new LogoutResponsePackage {Success = true};

            _binaryFormatter.Serialize(_tcpClient.GetStream(), logoutResponsePackage);
            _tcpClient.Close();
            Console.CursorLeft = 0;
            Console.Write($"User {_clientName} disconnected!\n>");
        }

        private void SendAddShiftResponsePackage(AddShiftPackage addShiftPackage)
        {
            AddShiftResponsePackage addShiftResponsePackage = new AddShiftResponsePackage();
            if (_loggedIn && Program.Database.UserExists(_clientName))
            {
                Program.Database.AddShift(_clientName, addShiftPackage.shift);
                addShiftResponsePackage.Success = true;
            }
            else
            {
                addShiftResponsePackage.NewUser = null;
                addShiftResponsePackage.Success = false;
            }
            _binaryFormatter.Serialize(_tcpClient.GetStream(), addShiftResponsePackage);
            Console.WriteLine("Added shift!");
        }

        private void SendRequestUserDataResponse(RequestUserDataPackage requestUserDataPackage)
        {
            RequestUserDataResponsePackage responsePackage = new RequestUserDataResponsePackage();

            // Erfolgreich wenn der User eingeloggt ist, der angeforderte User existiert
            // und der User entweder seine eigenen Daten abfragt oder Adminrechte hat.
            if (_loggedIn && Program.Database.UserExists(requestUserDataPackage.Username)
                && ((_clientName == requestUserDataPackage.Username) || _isAdmin))
            {
                responsePackage.Success = true;
                responsePackage.UserData = Program.Database.GetUser(requestUserDataPackage.Username);
            }
            else
            {
                responsePackage.Success = false;
                responsePackage.UserData = null;
            }
            _binaryFormatter.Serialize(_tcpClient.GetStream(), responsePackage);
        }

        private void SendLoginResponse(LoginPackage loginPackage)
        {
            LoginResponsePackage loginResponsePackage = new LoginResponsePackage();
            if (Program.Database.UserExists(loginPackage.Username))
            {
                loginResponsePackage.Username = loginPackage.Username;
                loginResponsePackage.Success = Program.Database.CheckPassword(loginPackage.Username,
                    loginPackage.Password);
                loginResponsePackage.IsAdmin = Program.Database.GetIsAdmin(loginPackage.Username);

                Console.CursorLeft = 0;
                Console.WriteLine($"{loginPackage.Username} logged in!");
                Console.Write(">");

                _clientName = loginResponsePackage.Username;
                _loggedIn = true;
                _isAdmin = loginResponsePackage.IsAdmin;
            }
            else
                loginResponsePackage.Success = false;

            _binaryFormatter.Serialize(_tcpClient.GetStream(), loginResponsePackage);
        }

        private void SendRegisterResponse(RegisterPackage registerPackage)
        {
            RegisterResponsePackage registerResponsePackage = new RegisterResponsePackage();
            if (Program.Database.UserExists(registerPackage.Username))
                registerResponsePackage.Success = false;
            else
            {
                registerResponsePackage.Success = Program.Database.AddUser(registerPackage.Username, registerPackage.Password);
                Console.CursorLeft = 0;
                Console.Write($"User {registerPackage.Username} registered!\n>");
            }
            _binaryFormatter.Serialize(_tcpClient.GetStream(), registerResponsePackage);
        }
    }
}