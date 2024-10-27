using GameLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicTacToe
{
    internal class Program
    {
        private static TcpListener listener;

        private static bool isFirstTurn = true;

        static void Main(string[] args)
        {
            Start().Wait();
        }

        private static async Task Start()
        {
            Random rnd = new Random();

            listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
            listener.Start();

            Console.WriteLine("Server is started, but we need a player...");

            using (TcpClient client = await listener.AcceptTcpClientAsync())
            {
                Game.Init();

                using (NetworkStream stream = client.GetStream())
                {
                    while (true)
                    {
                        Console.Clear();
                        GameUtils.DisplayBoard();

                        Console.WriteLine("CPU makes its turn...");
                        int position = rnd.Next(0, 9);

                        // If CPU made invalid move, it will loop over and over again until there will be a correct move
                        if (Game.Move(position))
                        {
                            await GameUtils.SendData(stream, position);
                            if (GameUtils.CheckGameEnd()) break; // If game is over then break the loop and end the programm

                            Console.Clear();
                            GameUtils.DisplayBoard();

                            position = await GameUtils.ReadData(stream);
                            Game.Move(position);
                            if (GameUtils.CheckGameEnd()) break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid position! Try again.");
                        }
                    }
                }
            }
        }
    }
}
