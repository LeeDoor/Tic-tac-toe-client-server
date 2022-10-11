﻿using System;
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
        private int port = 8000;
        private IPEndPoint serverIpAdress;

        private List<Game> currentGames = new();

        public GameManager()
        {
            serverIpAdress = new IPEndPoint(IPAddress.Parse("192.168.1.57"), port);
        }

        public void StartServer()
        {
            TcpListener server = new TcpListener(serverIpAdress);
            server.Start();

            Queue<NetworkStream> queue = new();
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                queue.Enqueue(client.GetStream());
                Console.WriteLine("user connected");
                if(queue.Count >= 2)
                {
                    StartGame(queue.Dequeue(), queue.Dequeue());
                }
            }
        }

        private void StartGame(NetworkStream first, NetworkStream second)
        {
            Player player1 = new Player(first);
            Player player2 = new Player(second);

            MatchStartAlert(player1);
            MatchStartAlert(player2);

            Game game = new Game(player1, player2);
            game.Start();
            Console.WriteLine("game started!");
            currentGames.Add(game);
        }

        private void MatchStartAlert(Player player)
        {
            player.Stream.Write(new byte[] { 1 });
        }
    }
}
