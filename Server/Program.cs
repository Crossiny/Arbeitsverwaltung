using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Xsl;
using System.Net.Sockets;
using System.Threading.Tasks;
using Server.Database;

namespace Server
{
    internal class Program
    {
        private static bool _running;
        private static int _port = Settings.Default.Port;
        private static readonly List<ClientConnection> _connections = new List<ClientConnection>();

        private static void Main(string[] args)
        {
            Task t = new Task(AcceptClients);
            t.Start();
            _running = true;

            Break @break = new Break() {StartTime = new DateTime(1991, 1, 2), EndTime = new DateTime(1991, 1, 3)};
            Shift shift = new Shift() {StartTime = new DateTime(1991,1,1), EndTime = new DateTime(1991,1,4)};
            shift.Breaks.Add(@break);
            User user = new User("JonnyB", 13333);
            user.Shifts.Add(shift);
            Console.WriteLine(user.WorkedSpan);
            Console.WriteLine(user.BreakSpan);

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
                                Console.WriteLine("port get             Returns the current port.");
                                Console.WriteLine("port set [NewPort]   Sets the port. Needs restart.");
                            }
                            else
                            {
                                switch (inputStrings[1])
                                {
                                    case "get":
                                        Console.WriteLine($"Current port is {_port}");
                                        break;
                                    case "set":
                                        int port;
                                        if (int.TryParse(inputStrings[2], out port) && (port > 0) &&
                                            (port < short.MaxValue))
                                        {
                                            _port = port;
                                            Settings.Default.Port = port;
                                            Settings.Default.Save();
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{inputStrings[2]} is not a valid port!");
                                        }
                                        break;
                                    default:
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
            while (_running)
            {
                if (tcpListener.Pending())
                {
                    _connections.Add(new ClientConnection(tcpListener.AcceptTcpClient()));
                }
            }
        }
    }
}