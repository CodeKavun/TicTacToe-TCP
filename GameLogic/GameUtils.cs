using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public static class GameUtils
    {
        public static async Task SendData(NetworkStream stream, int position)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(position.ToString());
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task<int> ReadData(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            await stream.ReadAsync(buffer, 0, buffer.Length);

            return int.Parse(Encoding.UTF8.GetString(buffer));
        }

        public static void DisplayBoard()
        {
            char[] board = Game.Board;

            Console.WriteLine($" {board[0]} | {board[1]} | {board[2]} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {board[3]} | {board[4]} | {board[5]} ");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {board[6]} | {board[7]} | {board[8]} ");
        }

        public static bool CheckGameEnd()
        {
            if (Game.CheckWinner())
            {
                Console.Clear();
                Console.WriteLine($"{Game.Winner} won this game!");
                DisplayBoard();
                return true;
            }
            if (Game.IsDraw)
            {
                Console.Clear();
                Console.WriteLine("It's a draw!");
                DisplayBoard();
                return true;
            }
            return false;
        }
    }
}
