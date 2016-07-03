using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;

namespace Arbeitsverwaltung.Classes
{
    internal class Client
    {
        private readonly Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        public void Connect(string ip, int port)
        {
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    Debug.Print("No ip-address specified! Please write down an Ip-address that exists!");
                }
                else
                {
                    TcpClient tc = new TcpClient(ip, port);
                    Debug.Print("Connection to Server successfully!");
                }
            }
            catch (Exception)
            {
                Debug.Print("Connection to Server failed!");
                throw;
            }
        }

        public void Register(string username, string password)
        {
            _dictionary.Add(username, password);
        }

        public void Login(string username, string password)
        {
            if (_dictionary.ContainsKey(username))
            {
                if (_dictionary[username] == password)
                {
                    Debug.Print("login details are correct!");
                }
            }
        }
    }
}