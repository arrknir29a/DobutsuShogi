using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi.network
{
    class Client
    {
        Content content;
        public int port { get; set; }
        public void Connect( IPAddress ip,int port,bool useUdp) {
            IPEndPoint ipep = new IPEndPoint(ip, port);
            if (!useUdp)
            {
                Socket s = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.IP);
                s.Connect(ipep);
                MemoryStream ms = new MemoryStream();
                Bencode.write("hello", ms);
                s.Send(ms.ToArray(),SocketFlags.None);
            }
        }
    }
}
