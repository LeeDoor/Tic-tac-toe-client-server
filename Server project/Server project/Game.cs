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
        private int playerGoing;
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
            playerGoing = new Random().Next(0, 1);

            players[0].SendStep(playerGoing == 0);
            players[1].SendStep(playerGoing == 1);
            GameLoop();
        }

        public void GameLoop()
        {
            while (true)
            {
                players[0].SendField(field);
                players[1].SendField(field);

                int playerStep = players[playerGoing].WaitForStep();
                ApplyMove(playerStep, playerGoing);
                int winres = CheckIfWin();
                playerGoing++;
                playerGoing = playerGoing % 2;
            }
        }

        private void ApplyMove(int cell, int playerId)
        {
            field[cell] = playerId == 0 ? 'X' : 'O';
        }

        //-1 nobody, 0 - first, 1 - second
        private int CheckIfWin()
        {
            for (int i = 0; i < 3; i++)
            {
                if (field[i * 3] == field[i * 3 + 1] && field[i * 3 + 1] == field[i * 3 + 2] && field[i * 3 + 1] != 'N')
                {
                    if (field[i] == 'X')
                        return 0;
                    else
                        return 1;
                }
                if (field[i] == field[i + 3] && field[i] == field[i + 6] && field[i] != 'N')
                {
                    if (field[i] == 'X')
                        return 0;
                    else
                        return 1;
                }
            }
            if (field[0] == field[4] && field[0] == field[8] && field[0] != 'N')
            {
                if (field[0] == 'X')
                    return 0;
                else
                    return 1;
            }
            if (field[2] == field[4] && field[2] == field[6] && field[2] != 'N')
            {
                if (field[2] == 'X')
                    return 0;
                else
                    return 1;
            }
            return -1;
        }
    }
}
