using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private int _port;
        static void Main(string[] args)
        {
            TcpListener tcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, _port));
        }
    }
}
