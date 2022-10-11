using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Server_project
{
    public class Game
    {
        private Player[] players;
        private bool isFirst;
        private char[] field;

        public Game(Player player1, Player player2)
        {
            players = new Player[2] { player1, player2 };

            field = new char[9]
            {
                'N','N','N','N','N','X','N','N','N'
            };
        }

        public void Start()
        {
            //randomizing first step
            isFirst = new Random().Next(0, 1) == 0 ? true : false;

            players[0].SendStep(isFirst);
            players[1].SendStep(!isFirst);
            GameLoop();
        }

        public void GameLoop()
        {
            players[0].SendField(field);
            players[1].SendField(field);


        }
    }
}
