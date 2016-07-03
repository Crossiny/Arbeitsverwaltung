using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
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
        public static Database.Database Database = new Database.Database();

        private static void Main(string[] args)
        {
            Task t = new Task(AcceptClients);

            _running = true;
            t.Start();

            #region Command processing

            Console.Write(">");
            var input = Console.ReadLine().ToLower();
            while (input != "exit")
            {
                if (input != null)
                {
                    var inputStrings = input.Split(' ');

                    switch (inputStrings[0])
                    {
                        #region IP

                        case "ip":
                            if (inputStrings.Length == 1)
                            {
                                Console.CursorLeft = 0;
                                Console.WriteLine("Missing parameter!");
                                Console.WriteLine("ip get             Returns the local IP-Address of the server.");
                            }
                            else if (inputStrings[1] == "get")
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
                                switch (inputStrings[1])
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
                                            Database.SetIsAdmin(inputStrings[2], true);
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
                            Console.WriteLine("admin list               Shows a list of admins");
                            Console.WriteLine("Exit                     Closes the application.");
                            break;

                            #endregion
                    }
                }
                Console.Write(">");
                input = Console.ReadLine();
            }

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
                    Console.WriteLine($"User connected! \n>");
                }
            }
            foreach (ClientConnection clientConnection in _connections)
            {
                clientConnection.Close();
            }
            tcpListener.Stop();
        }
    }
}