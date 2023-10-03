using System;

namespace TicTacToeGameConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int currentPlayer = -1;
            char[] gameMarkers = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int gameStatus = 0;

            do
            {
                Console.Clear();
                currentPlayer = GetNextPlayer(currentPlayer);

                HeadsUpDisplay(currentPlayer);
                DrawGameboard(gameMarkers );
                GameEngine(gameMarkers, currentPlayer);



            } while (true);





        }

        private static void GameEngine(char[] gameMarkers, int currentPlayer)
        {
            bool notvalidMove = true;
            do
            {
                string userInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(userInput) &&
                   (userInput.Equals("1") ||
                   userInput.Equals("2") ||
                   userInput.Equals("3") ||
                   userInput.Equals("4") ||
                   userInput.Equals("5") ||
                   userInput.Equals("6") ||
                   userInput.Equals("7") ||
                   userInput.Equals("8") ||
                   userInput.Equals("9")))
                {
                    if(!notvalidMove)
                    {
                        Console.Clear();
                    }
                    int.TryParse(userInput, out var gamePlacementMarker);
                    char currentMarker = gameMarkers[gamePlacementMarker - 1];

                    if (currentMarker.Equals('X') || currentMarker.Equals('O'))
                    {
                        Console.WriteLine("Placement has already a marker please select another placement");
                    }
                    else
                    {
                        gameMarkers[gamePlacementMarker - 1] = GetPlayerMarker(currentPlayer);

                        notvalidMove = false;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid value please select another placemnt");
                }
            } while (notvalidMove);

            
        }

        private static char GetPlayerMarker(int player)
        {
            if (player % 2 ==0) 
            { 
                return 'O'; 
            }

            return 'X';
        }


        static void HeadsUpDisplay(int PlayerNumber)
        {
            Console.WriteLine("Welcome to tic tac toe!");
            Console.WriteLine("Player 1: X");
            Console.WriteLine("Player 2: O");
            Console.WriteLine();

            Console.WriteLine($"Player {PlayerNumber} to move, select 1 through 9 from the board game");
            Console.WriteLine();
        }

        static void DrawGameboard(char[]gameMarkers)
        {
            Console.WriteLine($" {gameMarkers[0]} | {gameMarkers[1]} | {gameMarkers[2]}");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {gameMarkers[3]} | {gameMarkers[4]} | {gameMarkers[5]}");
            Console.WriteLine("---+---+---");
            Console.WriteLine($" {gameMarkers[6]} | {gameMarkers[7]} | {gameMarkers[8]}");

        }

        static int GetNextPlayer(int player)
        {
            if (player.Equals(1))
            {
                return 2;
            }
            return 1;
            
        }
    }
}