﻿// Arbeitsverwaltung/Arbeitsverwaltung/Client.cs
// by Christoph Schimpf, Jonathan Boeckel
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
        public static TcpClient TcpClient;

        public void Connect(string ip, int port)
        {
            TcpClient = new TcpClient();
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    MainWindow.PrintStatus("No ip-address specified! Please write down an Ip-address that exists!");
                }
                else
                {

                    TcpClient.Connect(ip, port);
                    MainWindow.PrintStatus("Connection to Server successfully!");
                }
            }
            catch (Exception)
            {
                MainWindow.PrintStatus("Connection to Server failed!");
            }
        }

        public void Register(string username, string password)
        {
            TcpClient = new TcpClient();

            if (TcpClient != null && !TcpClient.Connected)
            {
                Connect(Settings.Default.IP, Settings.Default.Port);
            }

            RegisterPackage registerPackage = new RegisterPackage()
            {
                Password = password,
                Username = username
            };
            if (TcpClient.Connected)
            {
                _binaryFormatter.Serialize(TcpClient.GetStream(), registerPackage);

                object receivedObject = _binaryFormatter.Deserialize(TcpClient.GetStream());

                if (receivedObject.GetType() == typeof(RegisterResponsePackage))
                {
                    RegisterResponsePackage registerResponsePackage = receivedObject as RegisterResponsePackage;

                    if (registerResponsePackage.Success)
                        MainWindow.PrintStatus("Registered successfully!");
                    else
                        MainWindow.PrintStatus("Registration failed!");
                }
            }
            TcpClient.Close();
        }

        /// <summary>
        /// true = admin, false = normalUser, null = error
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool? Login(string username, string password)
        {
            TcpClient = new TcpClient();

            if (!TcpClient.Connected)
                Connect(Settings.Default.IP, Settings.Default.Port);

            LoginPackage loginPackage = new LoginPackage
            {
                Password = password,
                Username = username
            };
            if (TcpClient.Connected)
            {
                _binaryFormatter.Serialize(TcpClient.GetStream(), loginPackage);

                object receivedObject = _binaryFormatter.Deserialize(TcpClient.GetStream());

                if (receivedObject.GetType() == typeof(LoginResponsePackage))
                {
                    LoginResponsePackage loginResponsePackage = receivedObject as LoginResponsePackage;

                    if (loginResponsePackage != null && !loginResponsePackage.Success)
                    {
                        return null;
                    }
                    else if (loginResponsePackage != null && loginResponsePackage.IsAdmin)
                    {
                        return true;
                    }
                    else if (loginResponsePackage != null && !loginResponsePackage.IsAdmin)
                    {
                        return false;
                    }
                }
            }

            return null;
        }
    }
}