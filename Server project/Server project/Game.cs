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
        Player[] players;
        private bool isFirst;


        public Game(Player player1, Player player2)
        {
            players = new Player[2] { player1, player2 };
        }
    }
}
