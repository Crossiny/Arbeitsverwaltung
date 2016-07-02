using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Server
{
    class ClientConnection
    {
        private TcpClient _tcpClient;
        public bool Connected;
        public ClientConnection(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            Task t = new Task(Start);
            Connected = true;
        }

        async void Start()
        {
            StreamReader streamReader = new StreamReader(_tcpClient.GetStream());
            StreamWriter streamWriter = new StreamWriter(_tcpClient.GetStream()) { AutoFlush = true };
            

        }
    }
}