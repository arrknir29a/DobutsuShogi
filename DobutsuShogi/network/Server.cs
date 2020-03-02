using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DobutsuShogi.network
{
    class Server
    {
        public int port { get; set; }
        public IPAddress ip { get; set; }
        public void Create()
        {
            IPEndPoint ipep = new IPEndPoint(ip, port);
            Socket s = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.IP);
            s.Bind(ipep);
            s.Listen(-1);
            Socket client= s.Accept();
            MemoryStream ms = new MemoryStream();
            Dictionary<string,object> helloMsg=new Dictionary<string,object>();
            helloMsg.Add("ver", 1);
            helloMsg.Add("token", "mi");
            Bencode.write(helloMsg, ms);

        }
    }
}
