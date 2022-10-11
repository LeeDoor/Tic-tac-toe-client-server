using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client_project.Model
{
    public static class ServerManager
    {
        private const string IP_SERVER = "192.168.1.57";
        private const int PORT = 8000;
        private static IPEndPoint point = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT);
        private static NetworkStream stream;
        private static bool isGoingNow;

        public static async Task StartAsync()
        {
            //try to connect
            await Task.Run(() =>
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect(point);
                MessageBox.Show("connected!");
                stream = tcpClient.GetStream();
                WaitForBegin();
            });
        }

        private static void WaitForBegin()
        {
            byte[] answer = new byte[1];
            stream.Read(answer, 0, answer.Length);
            GetStep();
        }

        private static void GetStep()
        {
            byte[] step = new byte[1];
            stream.Read(step, 0, 1);
            if (step[0] == 1)
                isGoingNow = true;
            else
                isGoingNow = false;
            MessageBox.Show(isGoingNow.ToString());
        }
    }
}
