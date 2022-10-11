using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_project
{
    public class Player
    {
        public NetworkStream Stream { get; }

        public Player(NetworkStream stream)
        {
            Stream = stream;
        }

        public void SendStep(bool isFirst)
        {
            Stream.Write(new byte[] { isFirst ? (byte)1 : (byte)2 });
        }
    }
}
