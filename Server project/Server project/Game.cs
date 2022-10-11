using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

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
				'N','N','N','N','N','N','N','N','N'
			};
		}

		public void Start()
        {
			do
			{
				field = new char[9]
				{
					'N','N','N','N','N','N','N','N','N'
				};
				//randomizing first step
				playerGoing = new Random().Next(0, 1);

				//send to players has match found
				players[0].SendSingleNumber(1);
				players[1].SendSingleNumber(1);

				//send to players who is first 
				players[0].SendSingleNumber(playerGoing == 0 ? 1 : 2);
				players[1].SendSingleNumber(playerGoing == 1 ? 1 : 2);
				GameLoop();
			}
			while (players[0].Stream.Socket.Connected && players[1].Stream.Socket.Connected); // loop while both connected
            players[0].SendSingleNumber(2);// send to both players that game stopped
            players[1].SendSingleNumber(2);
        }

		public void GameLoop()
		{
			while (true)
			{
				//send field
				players[0].SendField(field);
				players[1].SendField(field);

				int winres = CheckIfWin();
				//send game state
				players[0].SendSingleNumber(winres != 0 && winres != 3 ? winres % 2 + 1 : winres);
				players[1].SendSingleNumber(winres);

				if (winres != 0) // if game ended
					break;

				int playerStep; 
				bool isVerified;
				do
				{
					playerStep = players[playerGoing].GetPlayerMove(); // gets player move choice
					isVerified = TryApplyMove(playerStep, playerGoing); // tries to apply player's move
					players[playerGoing].SendSingleNumber(isVerified ? 1 : 2); // sends result to player to be able to resend info if needed
				} while (!isVerified);

				playerGoing++; // change current player;
				playerGoing = playerGoing % 2;
			}
		}

		private bool TryApplyMove(int cell, int playerId)
		{
			if (field[cell] == 'N')
			{
				field[cell] = playerId == 0 ? 'X' : 'O';
				return true;
			}
			return false;
		}

		//0 nobody, 1 - first, 2 - second, 3 - tie
		private int CheckIfWin()
		{
			for (int i = 0; i < 3; i++)
			{
				if (field[i * 3] == field[i * 3 + 1] && field[i * 3 + 1] == field[i * 3 + 2] && field[i * 3 + 1] != 'N') // horizontal
				{
					if (field[i] == 'X')
						return 1;
					else
						return 2;
				}
				if (field[i] == field[i + 3] && field[i] == field[i + 6] && field[i] != 'N') // vertical
				{
					if (field[i] == 'X')
						return 1;
					else
						return 2;
				}
			}
			if (field[0] == field[4] && field[0] == field[8] && field[0] != 'N') // diagonal
			{
				if (field[0] == 'X')
					return 1;
				else
					return 2;
			}
			if (field[2] == field[4] && field[2] == field[6] && field[2] != 'N') // diagonal
			{
				if (field[2] == 'X')
					return 1;
				else
					return 2;
			}
			if (!field.Contains('N')) // tie
				return 3;
			return 0; // keep going
		}
	}
}
