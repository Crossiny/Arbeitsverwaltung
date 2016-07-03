using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using Arbeitsverwaltung.Properties;
using Server.Packages;

namespace Arbeitsverwaltung.Classes
{
    internal class Client
    {
        private BinaryFormatter _binaryFormatter = new BinaryFormatter();
        private TcpClient _tcpClient;

        public void Connect(string ip, int port)
        {
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    MainWindow.PrintStatus("No ip-address specified! Please write down an Ip-address that exists!");
                }
                else
                {

                    _tcpClient.Connect(ip, port);
                    MainWindow.PrintStatus("Connection to Server successfully!");
                }
            }
            catch (Exception)
            {
                MainWindow.PrintStatus("Connection to Server failed!");
                throw;
            }
        }

        public void Register(string username, string password)
        {
            if(_tcpClient == null)
                _tcpClient = new TcpClient();

            if (!_tcpClient.Connected)
            {
                Connect(Settings.Default.IP, Settings.Default.Port);
            }

            RegisterPackage registerPackage = new RegisterPackage()
            {
                Password = password,
                Username = username
            };

            _binaryFormatter.Serialize(_tcpClient.GetStream(), registerPackage);

            object receivedObject = _binaryFormatter.Deserialize(_tcpClient.GetStream());

            if (receivedObject.GetType() == typeof(RegisterResponsePackage))
            {
                RegisterResponsePackage registerResponsePackage = receivedObject as RegisterResponsePackage;
            }

            _tcpClient.Close();
        }

        /// <summary>
        /// true = admin, false = normalUser, null = error
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool? Login(string username, string password)
        {
                _tcpClient = new TcpClient();

            if (!_tcpClient.Connected)
                Connect(Settings.Default.IP, Settings.Default.Port);

            LoginPackage loginPackage = new LoginPackage
            {
                Password = password,
                Username = username
            };

            _binaryFormatter.Serialize(_tcpClient.GetStream(), loginPackage);

            object receivedObject = _binaryFormatter.Deserialize(_tcpClient.GetStream());

            if (receivedObject.GetType() == typeof(LoginResponsePackage))
            {
                LoginResponsePackage loginResponsePackage = receivedObject as LoginResponsePackage;
                MessageBox.Show(loginResponsePackage.IsAdmin.ToString());

                if (loginResponsePackage.IsAdmin)
                {
                    return true;
                }
                else if (!loginResponsePackage.IsAdmin)
                {
                    return false;
                }
            }

            return null;
        }
    }
}