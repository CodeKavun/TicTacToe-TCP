using GameLogic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientSide
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunClient().Wait();
        }

        private static async Task RunClient()
        {
            using (TcpClient client = new TcpClient())
            {
                Game.Init();

                // Conecting to the server
                await client.ConnectAsync("127.0.0.1", 8000);
                Console.WriteLine("You're connected to the server!");

                using (NetworkStream stream = client.GetStream())
                {
                    while (true)
                    {
                        Console.Clear();
                        GameUtils.DisplayBoard();

                        int position = await GameUtils.ReadData(stream);
                        Game.Move(position);
                        if (GameUtils.CheckGameEnd()) break;

                        Console.Clear();
                        GameUtils.DisplayBoard();

                        Console.Write("Your turn. Enter a position number from 0 to 8: ");
                        position = int.Parse(Console.ReadLine()); // Client should enter the position value

                        if (Game.Move(position))
                        {
                            await GameUtils.SendData(stream, position);
                            if (GameUtils.CheckGameEnd()) break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong position number! Enter again.");
                        }
                    }
                }
            }
        }
    }
}
