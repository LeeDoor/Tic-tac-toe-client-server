using System;
using System.IO;

namespace Client_project.Model
{
    public static class NetworkSendGet
    {
        public static bool SendByteArray(Stream stream, byte[] arr)
        {
            try
            {
                stream.Write(arr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("send " + arr.Length);
            return true;
        }

        public static bool GetCharArray(Stream stream, out byte[] arr, int size)
        {
            arr = new byte[size];
            try
            {
                stream.Read(arr);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("get " + arr.Length);
            return true;
        }
    }
}
