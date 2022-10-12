using Client_project.ViewModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System;

namespace Client_project.Model
{
    public class GameManager
    {
        private MainWindowVM _vm;

        private const string IP_SERVER = "192.168.1.57";
        private const int PORT = 8000;
        private IPEndPoint point = new IPEndPoint(IPAddress.Parse(IP_SERVER), PORT);
        public NetworkStream stream;
        private bool isGoingNow;

        public async Task StartAsync(MainWindowVM vm)
        {
            _vm = vm;
            TcpClient tcpClient = new TcpClient();
            //try to connect
            tcpClient.Connect(point);
            MessageBox.Show("connected!");
            stream = tcpClient.GetStream();

            await Task.Run(() =>
            {
                try
                {
                    while (WaitForBegin());
                }
                catch (Exception ex)
                {
                    if (ex is IOException)
                    {
                        Disconnect();
                    }
                    else throw;
                }
            });
        }

        private bool WaitForBegin()
        {
            NetworkSendGet.GetCharArray(stream, out byte[] answer, 1);
            if (answer[0] == byte.MaxValue) return false;
            GetStep();
            Loop();
            return true;
        }

        private void Loop()
        {
            while (true)
            {
                UpdateField();
                //receive game state
                NetworkSendGet.GetCharArray(stream, out byte[] arr, 1);
                int state = arr[0];
                if (state != 0)
                {
                    ResultNotification(state);
                    break;
                }
                if (isGoingNow)
                {
                    _vm.IsActive = true;
                    int result;
                    do
                    {
                        NetworkSendGet.GetCharArray(stream, out arr, 1);
                        result = arr[0];
                    }
                    while (result != 1); // while not confirmed
                    _vm.IsActive = false;
                }

                isGoingNow = !isGoingNow;
            }
        }

        private void UpdateField()
        {
            NetworkSendGet.GetCharArray(stream, out byte[] field, 9);
            string fieldStr = Encoding.UTF8.GetString(field);
            _vm.UpdateField(fieldStr);
        }

        private void GetStep()
        {
            NetworkSendGet.GetCharArray(stream, out byte[] step, 1);
            if (step[0] == 1)
                isGoingNow = true;
            else
                isGoingNow = false;
        }

        private void ResultNotification(int gameState)
        {
            if (gameState == 1)
                MessageBox.Show("you lose!");
            else if (gameState == 2)
                MessageBox.Show("you win!");
            else
                MessageBox.Show("tie!");
        }

        private void Disconnect()
        {
            MessageBox.Show("your enemy disconnected. technical win");
        }
    }
}
