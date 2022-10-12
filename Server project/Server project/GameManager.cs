using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;   
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_project
{
    public class GameManager
    {
        private string ipadressPath = "ServerIpPort.txt";
        private IPEndPoint serverIpAdress;

        private List<Game> currentGames = new();

        public GameManager()
        {
            string curDir = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            string[] ipAndPort = File.ReadAllLines(curDir + "\\" + ipadressPath);
            serverIpAdress = new IPEndPoint(IPAddress.Parse(ipAndPort[0]), int.Parse(ipAndPort[1]));
        }

        public void StartServer()
        {
            TcpListener server = new TcpListener(serverIpAdress);
            server.Start();
            Console.WriteLine("server started");

            Queue<NetworkStream> queue = new();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Task.Run(() =>
                {
                    queue.Enqueue(client.GetStream());
                    Console.WriteLine("user connected");
                    if (queue.Count >= 2)
                    {
                        NetworkStream s1 = queue.Dequeue();
                        NetworkStream s2 = queue.Dequeue();
                        StartGame(s1, s2);
                    }
                });
            }
        }

        private void StartGame(NetworkStream first, NetworkStream second)
        {
            Player player1 = new Player(first);
            Player player2 = new Player(second);

            Game game = new Game(player1, player2);
            Console.WriteLine("game started!");
            game.Start();
            currentGames.Add(game);
        }
    }
}
