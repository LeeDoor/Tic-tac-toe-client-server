using Client_project.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Documents;

namespace Client_project.Model
{
    public static class ServerManager
    {
        private static MainWindowVM _vm;

        private const string IP_SERVER = "192.168.1.57";
        private const int PORT = 8000;
        private static IPEndPoint point = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT);
        private static NetworkStream stream;
        private static bool isGoingNow;

        public static async Task StartAsync(MainWindowVM vm)
        {
            _vm = vm;

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
            Loop();
        }

        private static void GetStep()
        {
            byte[] step = new byte[1];
            stream.Read(step, 0, 1);
            if (step[0] == 1)
                isGoingNow = true;
            else
                isGoingNow = false;
        }

        private static void UpdateField()
        {
            byte[] field = new byte[9];
            stream.Read(field);
            string fieldStr = Encoding.UTF8.GetString(field);
            _vm.UpdateField(fieldStr);
        }

        public static void SendSingleNumber(int id)
        {
            stream.Write(new byte[1] { (byte)id });
        }


        //0 nothing 1 loose 2 win
        private static int ReceiveSingleNumber()
        {
            byte[] state = new byte[1];
            stream.Read(state);
            return state[0];
        }

        private static void ResultNotification(int gameState)
        {
            if (gameState == 1)
                MessageBox.Show("you lose!");
            else if (gameState == 2)
                MessageBox.Show("you win!");
            else
                MessageBox.Show("tie!");
        }

        private static void Loop()
        {
            while (true)
            {
                UpdateField();
                //receive game state
                int state = ReceiveSingleNumber();
                if (state != 0)
                {
                    ResultNotification(state);
                    break;
                }
                if (isGoingNow)
                {
                    _vm.IsActive = true;
                    int result;
                    do result = ReceiveSingleNumber();
                    while (result != 1); // while not confirmed
                    _vm.IsActive = false;
                }

                isGoingNow = !isGoingNow;
            }
        }
    }
}
