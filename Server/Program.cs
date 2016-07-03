using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    internal class Program
    {
        private static bool _running;
        private static int _port = Settings.Default.Port;
        private static readonly List<ClientConnection> _connections = new List<ClientConnection>();
        public static Database.Database Database;

        private static void Main(string[] args)
        {
            Task t = new Task(AcceptClients);
            t.Start();
            _running = true;

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
                            #region Help

                        case "help":
                            Console.WriteLine(@"port set [NewPort]     Changes the listening port.");
                            Console.WriteLine(@"restart         Restarts the listener.");
                            break;

                            #endregion

                            #region Port

                        case "port":
                            if (inputStrings.Length < 2)
                            {
                                Console.CursorLeft = 0;
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

                            #region Restart

                        case "restart":

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
                    Console.WriteLine($"User connected! IP: {tcpClient.Client.RemoteEndPoint}");
                }
            }
        }
    }
}