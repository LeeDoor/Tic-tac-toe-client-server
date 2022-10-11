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

        public void SendField(char[] field)
        {
            StringBuilder sb = new();
            foreach(char c in field)
            {
                sb.Append(c);
            }
            Stream.Write(Encoding.UTF8.GetBytes(sb.ToString()));
        }

        public int GetPlayerMove()
        {
            byte[] step = new byte[1];
            Stream.Read(step);
            return step[0];
        }

        public void SendSingleNumber(int id)
        {
            Stream.Write(new byte[] { (byte)(id) });
        }
    }
}
