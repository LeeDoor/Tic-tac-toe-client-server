using System;
using System.IO;

namespace Client_project.Model
{
    public static class NetworkSendGet
    {
        public static void SendByteArray(Stream stream, byte[] arr)
        {
            stream.Write(arr);
        }

        public static void GetCharArray(Stream stream, out byte[] arr, int size)
        {
            arr = new byte[size];
            stream.Read(arr);
            if (arr[0] == byte.MaxValue)
            {
                throw new IOException();
            }
        }
    }
}
