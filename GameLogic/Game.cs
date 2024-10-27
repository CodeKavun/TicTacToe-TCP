using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public static class Game
    {
        public static char[] Board { get; set; } = new char[9];
        public static char CurrentPlayer { get; private set; } = 'X'; // X (e.g. CPU is first)

        public static char Winner { get; private set; } = ' ';

        public static bool IsDraw => Board.All(cell => cell != ' ');

        public static void Init()
        {
            // Initializing empty spaces on the board
            for (int i = 0; i < Board.Length; i++)
            {
                Board[i] = ' ';
            }
        }

        // I made Move method as boolean to easiely check the correctness of move
        public static bool Move(int position)
        {
            // Checking if position is correct
            if (position < 0 || position >= Board.Length || Board[position] != ' ') return false;

            Board[position] = CurrentPlayer;

            Winner = CurrentPlayer;
            CurrentPlayer = CurrentPlayer == 'X' ? 'O' : 'X'; // Switching players (X - CPU, O - Client)

            return true;
        }

        public static bool CheckWinner()
        {
            int[,] winningCombos =
            {
                { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 },
                { 0, 3, 6 }, { 1, 4, 7 }, { 2, 5, 8 },
                { 0, 4, 8 }, { 2, 4, 6 }
            };

            for (int i = 0; i < winningCombos.GetLength(0); i++)
            {
                if (Board[winningCombos[i, 0]] != ' ' &&
                    Board[winningCombos[i, 0]] == Board[winningCombos[i, 1]] &&
                    Board[winningCombos[i, 1]] == Board[winningCombos[i, 2]])
                    return true;
            }

            return false;
        }
    }
}
