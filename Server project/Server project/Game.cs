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
		}

        #region server manipulation
        private void ConnectionDeniedNotify()
        {// send to both players that game stopped
            try
            {
                NetworkSendGet.SendByteArray(players[0], new byte[] { byte.MaxValue });
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                    Console.WriteLine("user1 disconnected");
                else throw;
            }
            try
            {
                NetworkSendGet.SendByteArray(players[1], new byte[] { byte.MaxValue });
            }
            catch (Exception ex)
            {
                if (ex is IOException)
                    Console.WriteLine("user2 disconnected");
                else throw;
            }
        }
		private void MatchStartedNotify() 
		{

            //send to players has match found
            NetworkSendGet.SendByteArray(players[0], new byte[] { 1 });
            NetworkSendGet.SendByteArray(players[1], new byte[] { 1 });
        }
		private void StepPositionNotify()
		{
            //randomizing first step
            playerGoing = new Random().Next(0, 1);
            //send to players who is first 
            NetworkSendGet.SendByteArray(players[0], new byte[] { playerGoing == 0 ? (byte)1 : (byte)2 });
            NetworkSendGet.SendByteArray(players[1], new byte[] { playerGoing == 1 ? (byte)1 : (byte)2 });
        }
		private void FieldChangesNotify()
		{
            //send field
            NetworkSendGet.SendByteArray(players[0], field.Select(x => (byte)x).ToArray());
            NetworkSendGet.SendByteArray(players[1], field.Select(x => (byte)x).ToArray());
        }
		private void GameStateNotify(int winres)
		{
            //send game state
            NetworkSendGet.SendByteArray(players[0], new byte[] { (byte)(winres != 0 && winres != 3 ? winres % 2 + 1 : winres) });
            NetworkSendGet.SendByteArray(players[1], new byte[] { (byte)winres });
        }
        private void StepGetAndVerification(out bool isVerified)
        {
            int playerStep;
            NetworkSendGet.GetCharArray(players[playerGoing], out byte[] bytes, 1);
            playerStep = bytes[0]; // gets player move choice
            isVerified = TryApplyMove(playerStep, playerGoing); // tries to apply player's move
                                                                // sends result to player to be able to resend info if needed
            NetworkSendGet.SendByteArray(players[playerGoing], new byte[] { (byte)(isVerified ? 1 : 2) });
        }
        private void SwitchSides()
        {
            playerGoing++; // change current player;
            playerGoing = playerGoing % 2;
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
        #endregion

        private void GameLoop()
        {
            while (true)
            {
                FieldChangesNotify(); // send field to both players
                int winres = CheckIfWin();
                GameStateNotify(winres); // check if game stopped and notify players
                if (winres != 0) // if game ended
                    break;
                bool isVerified;
                do StepGetAndVerification(out isVerified); // prepare a step
                while (!isVerified);
                SwitchSides();
            }
        }

        public void Start()
        {
			do
			{
                try
                {
                    field = new char[9]
                    {
                    'N','N','N','N','N','N','N','N','N'
                    };
                    MatchStartedNotify();
                    StepPositionNotify();
                    GameLoop();
                }
                catch (Exception ex)
                {
                    if (ex is IOException)
                        break;
                    else throw;
                }
			}
			while (players[0].Stream.Socket.Connected && players[1].Stream.Socket.Connected); // loop while both connected
			ConnectionDeniedNotify();
        }
	}
}
