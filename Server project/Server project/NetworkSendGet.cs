﻿using System;
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
            try
            {
                stream.Write(arr);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Console.WriteLine("send " + arr.Length);
            return true;
        }

        public static bool GetCharArray(Player player, out byte[] arr, int size) => GetCharArray(player.Stream, out arr, size);
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
