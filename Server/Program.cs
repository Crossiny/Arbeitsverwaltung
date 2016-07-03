using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Threading.Tasks;
using Server.Database;

namespace Server
{
    internal class Program
    {
        private static bool _running;
        private static int _port = Settings.Default.Port;
        private static List<ClientConnection> _connections = new List<ClientConnection>();
        public static Database.Database Database;
        private static void Main(string[] args)
        {
            FileStream fileStream;
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            if (!File.Exists("DataBase.bin"))
            {
                fileStream = new FileStream("DataBase.bin", FileMode.CreateNew);
                binaryFormatter.Serialize(fileStream, new Database.Database());
                fileStream.Close();
            }
            fileStream = fileStream = new FileStream("DataBase.bin", FileMode.Open);
            Database = binaryFormatter.Deserialize(fileStream) as Database.Database;
            fileStream.Close();
            Task t = new Task(AcceptClients);

            _running = true;
            t.Start();

            #region Command processing

            Console.Write(">");
            var input = Console.ReadLine();
            while (input != "exit")
            {
                if (input != null)
                {
                    var inputStrings = input.Split(' ');

                    switch (inputStrings[0].ToLower())
                    {
                        #region IP

                        case "ip":
                            if (inputStrings.Length == 1)
                            {
                                Console.CursorLeft = 0;
                                Console.WriteLine("Missing parameter!");
                                Console.WriteLine("ip get             Returns the local IP-Address of the server.");
                            }
                            else if (inputStrings[1].ToLower() == "get")
                            {
                                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                                foreach (IPAddress ip in host.AddressList)
                                {
                                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                                    {
                                        Console.WriteLine($"The server is running on {ip.ToString()}:{_port}");
                                    }
                                }
                            }
                            break;

                        #endregion

                        #region Port

                        case "port":
                            if (inputStrings.Length == 1)
                            {
                                Console.CursorLeft = 0;
                                Console.WriteLine("Missing parameter!");
                                Console.WriteLine("port get             Returns the current port.");
                                Console.WriteLine("port set [NewPort]   Sets the port. Needs restart.");
                            }
                            else
                            {
                                switch (inputStrings[1].ToLower())
                                {
                                    case "get":
                                        Console.CursorLeft = 0;
                                        Console.WriteLine($"Current port is {_port}");
                                        break;
                                    case "set":
                                        int port;
                                        if (inputStrings.Length == 3)
                                        {
                                            if (int.TryParse(inputStrings[2], out port) && (port > 0) &&
                                                (port < short.MaxValue))
                                            {
                                                _port = port;
                                                Settings.Default.Port = port;
                                                Settings.Default.Save();
                                                Console.WriteLine($"Port set to {_port}.");
                                                Console.WriteLine("Needs restart to update.");
                                            }
                                            else
                                            {
                                                Console.CursorLeft = 0;
                                                Console.WriteLine($"{inputStrings[2]} is not a valid port!");
                                            }
                                        }
                                        else
                                        {
                                            Console.CursorLeft = 0;
                                            Console.WriteLine("Port is missing.");
                                            Console.WriteLine("Usage: port set [NewPort]");
                                        }
                                        break;
                                    default:
                                        Console.CursorLeft = 0;
                                        Console.WriteLine("port get             Returns the current port.");
                                        Console.WriteLine("port set [NewPort]   Sets the port. Needs restart.");
                                        break;
                                }
                            }
                            break;

                        #endregion

                        #region Start
                        case "start":
                            if (_running == true)
                                Console.WriteLine("Server is already running");
                            else
                            {
                                t = new Task(AcceptClients);
                                _running = true;
                                t.Start();
                                Console.WriteLine("Server started!");
                            }
                            break;
                        #endregion

                        #region Stop 
                        case "stop":
                            if (_running == false)
                                Console.WriteLine("Server is not started!");
                            else
                            {
                                _running = false;
                                Console.WriteLine("Stopped Server!");
                            }
                            break;
                        #endregion

                        #region Restart

                        case "restart":
                            if (!_running)
                                Console.WriteLine("Server is not running!");
                            else
                            {
                                _running = false;
                                Console.WriteLine("Stopped Server!");
                                Thread.Sleep(100);
                                t = new Task(AcceptClients);
                                _running = true;
                                t.Start();
                                Console.WriteLine("Server restarted!");
                            }
                            break;

                        #endregion

                        #region Admin
                        case "admin":
                            if (inputStrings.Length == 1)
                            {
                                Console.WriteLine("Missing parameter!");
                                Console.WriteLine("admin promote [Username] Promotes user to admin.");
                                Console.WriteLine("admin demote [Username]  Demotes admin to user.");
                                Console.WriteLine("admin list               Shows a list of admins");
                            }
                            else
                            {
                                switch (inputStrings[1].ToLower())
                                {
                                    case "promote":
                                        if (inputStrings.Length == 3)
                                        {
                                            if (!Database.UserExists(inputStrings[2]))
                                            {
                                                Console.WriteLine($"User {inputStrings[2]} does not exist!");
                                                break;
                                            }
                                            if (Database.GetIsAdmin(inputStrings[2]))
                                            {
                                                Console.WriteLine($"User {inputStrings[2]} is already admin!");
                                                break;
                                            }
                                            Database.SetIsAdmin(inputStrings[2], true);
                                            Console.WriteLine($"User {inputStrings[2]} is now an admin!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Missing parameter!");
                                            Console.WriteLine("admin promote [Username] Promotes user to admin.");
                                        }
                                        break;
                                    case "demote":
                                        if (inputStrings.Length == 3)
                                        {
                                            if (!Database.UserExists(inputStrings[2]))
                                            {
                                                Console.WriteLine($"User {inputStrings[2]} does not exist!");
                                                break;
                                            }
                                            if (!Database.GetIsAdmin(inputStrings[2]))
                                            {
                                                Console.WriteLine($"User {inputStrings[2]} is not an admin!");
                                                break;
                                            }
                                            Database.SetIsAdmin(inputStrings[2], false);
                                            Console.WriteLine($"User {inputStrings[2]} is no longer an Admin!");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Missing parameter!");
                                            Console.WriteLine("admin demote [Username]  Demotes admin to user.");
                                        }

                                        break;
                                    case "list":
                                        foreach (KeyValuePair<string, User> user in Database.UserDictionary)
                                        {
                                            Console.WriteLine(user.Value.Username);
                                        }
                                        break;
                                }
                            }
                            break;
                        #endregion

                        #region User
                        case "user":
                            if (inputStrings.Length == 1)
                            {
                                Console.WriteLine("Missing parameter!");
                                Console.WriteLine("user list                Shows a list of all users");
                            }
                            if (inputStrings.Length == 2)
                            {
                                if (inputStrings[1].ToLower() == "list")
                                    foreach (string key in Database.UserDictionary.Keys)
                                    {
                                        Console.WriteLine(key);
                                    }
                                else
                                {
                                    Console.WriteLine("Wrong parameter!");
                                    Console.WriteLine("user list                Shows a list of all users");
                                }
                            }
                            if (inputStrings.Length == 3)
                            {
                                if (inputStrings[1] == "getwage")
                                {
                                    string username = inputStrings[2];
                                    if (Database.UserExists(username))
                                        Console.WriteLine($"{username} earns {Database.UserDictionary[username].Wage}€/hour");

                                    else Console.WriteLine($"User {username} does not exist!");
                                }
                                else
                                {
                                    Console.WriteLine("Wrong parameter!");
                                    Console.WriteLine("user getwage [Username]  Gets the wage of an user");
                                }
                            }
                            if (inputStrings.Length == 4)
                            {
                                if (inputStrings[1] == "setwage")
                                {
                                    string username = inputStrings[2];
                                    if (!Database.UserExists(username))
                                    {
                                        Console.WriteLine($"User {username} does not exist!");
                                        break;
                                    }
                                    else
                                    {
                                        double wage;
                                        if (double.TryParse(inputStrings[3], out wage) && wage > 0)
                                        {
                                            Database.UserDictionary[username].Wage = wage;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{wage}€ is not a valid wage!");
                                        }
                                    }
                                }
                            }
                            break;
                        #endregion

                        #region Save
                        case "save":
                            fileStream = new FileStream("DataBase.bin", FileMode.Open);
                            new BinaryFormatter().Serialize(fileStream, Database);
                            fileStream.Close();
                            Console.WriteLine("Config saved!");
                            break;
                        #endregion

                        #region Help

                        case "help":
                            Console.WriteLine("ip get                   Shows the local IP the server is running on");
                            Console.WriteLine("port get                 Shows the listening port.");
                            Console.WriteLine("port set [NewPort]       Changes the listening port.");
                            Console.WriteLine("start                    Starts the server.");
                            Console.WriteLine("stop                     Stops the server.");
                            Console.WriteLine("restart                  Restarts the listener.");
                            Console.WriteLine("help                     Shows this message");
                            Console.WriteLine("admin promote [Username] Promotes user to admin.");
                            Console.WriteLine("admin demote [Username]  Demotes admin to user.");
                            Console.WriteLine("admin list               Shows a list of all admins");
                            Console.WriteLine("user list                Shows a list of all users");
                            Console.WriteLine("user setwage [Username] [newWage]    Sets the payment of an User");
                            Console.WriteLine("user getwage [Username]  Gets the wage of an user");
                            Console.WriteLine("save                     Saves the database/config");
                            Console.WriteLine("exit                     Closes the application.");
                            break;

                            #endregion
                    }
                }
                Console.Write(">");
                input = Console.ReadLine();
            }

            fileStream = new FileStream("DataBase.bin", FileMode.Open);
            new BinaryFormatter().Serialize(fileStream, Database);
            fileStream.Close();

            #endregion
        }

        private static void AcceptClients()
        {
            TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, _port));
            tcpListener.Start();
            while (_running)
            {
                if (tcpListener.Pending())
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    _connections.Add(new ClientConnection(tcpClient));
                    Console.CursorLeft = 0;
                    Console.WriteLine($"User connected!");
                    Console.Write(">");
                }
                Thread.Sleep(10);
            }
            foreach (ClientConnection clientConnection in _connections)
            {
                clientConnection.Close();
            }
            tcpListener.Stop();
        }
    }
}