using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server_project
{
    public static class NetworkSendGet
    {
        public static bool SendByteArray(Player player, byte[] arr) => SendByteArray(player.Stream, arr);
        public static bool SendByteArray(Stream stream, byte[] arr)
        {
            stream.Write(arr);
            return true;
        }

        public static void GetCharArray(Player player, out byte[] arr, int size) => GetCharArray(player.Stream, out arr, size);
        public static void GetCharArray(Stream stream, out byte[] arr, int size)
        {
            arr = new byte[size];
            stream.Read(arr);
        }
    }
}
