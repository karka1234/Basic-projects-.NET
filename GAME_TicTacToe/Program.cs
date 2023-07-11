namespace GAME_TicTacToe
{
    internal class Program
    {
        public static char player1 = '0';
        public static char player2 = 'X';
        static void Main(string[] args)
        {
            Console.WriteLine("TicTacToe zaidimas: ");
            string userInput = "";
            while (true)
            {
                PrintMenu();
                if (!ProccessSelection(GetUserSelectionMenu()))
                    break;
                Console.WriteLine("\r\nŽaidžiam vėl ? Y / N");
                userInput = GetUserSelectionMenu();
                if (userInput.Trim().ToLower() != "y")
                    break;
            }
        }
        static void StartNewGameSinglePlayer(int difficulty)
        {
            Console.WriteLine("Vieno žaidėjo, žaidimo pradžia\r\n");
            char[,] gameTable = ResetGameTable();
            PrintGameTable(gameTable);
            while (true)
            {
                ProcessUserInput(gameTable, player1);
                if (CheckWin(gameTable, player1))
                {
                    Console.WriteLine($"\tLaimejo {player1} zaidejas");
                    break;
                }
                ProcessAiInput(gameTable, player2);
                if (CheckWin(gameTable, player2))
                {
                    Console.WriteLine($"\tLaimejo {player2} zaidejas");
                    break;
                }                
            }
        }
        static void ProcessAiInput(char[,] gameTableNow, char player0orX)
        {
            Console.WriteLine($"{player0orX} zaidejo eilė.");
            CheckAndChangeGameTableValueAI(gameTableNow, player0orX);
            PrintGameTable(gameTableNow);
            Console.WriteLine();
        }
        static void CheckAndChangeGameTableValueAI(char[,] gameTableNow, char player0orX)
        {
            Random random = new Random();
            while (true)
            {
                int posX = random.Next(0, 3);
                int posY = random.Next(0, 3);
                if (gameTableNow[posY, posX] == '.')
                {
                    gameTableNow[posY, posX] = player0orX;
                    break;
                }
                break;
            }
        }

        static void StartNewGameTwoPlayers()////tikrint kada bu s draftas
        {
            Console.WriteLine("Dvieju žaidėju žaidimo pradžia\r\n");
            char[,] gameTable = ResetGameTable();
            PrintGameTable(gameTable);
            while (true)
            {
                ProcessUserInput(gameTable, player1);
                if (CheckWin(gameTable, player1))
                {
                    Console.WriteLine($"\tLaimejo {player1} zaidejas");
                    break;
                }
                ProcessUserInput(gameTable, player2);
                if (CheckWin(gameTable, player2))
                {
                    Console.WriteLine($"\tLaimejo {player2} zaidejas");
                    break;
                }
                if (CheckDraft(gameTable))
                {
                    Console.WriteLine($"\tLygiosios");
                    break;
                }
            }
        }
        static bool CheckWin(char[,] gameTableNow, char player)  // tikrinam po viena
        {
            List<string> winSymbols = new List<string>();
            winSymbols.Add($"{gameTableNow[0, 0]}{gameTableNow[0, 1]}{gameTableNow[0, 2]}");
            winSymbols.Add($"{gameTableNow[1, 0]}{gameTableNow[1, 1]}{gameTableNow[1, 2]}");
            winSymbols.Add($"{gameTableNow[2, 0]}{gameTableNow[2, 1]}{gameTableNow[2, 2]}");
            winSymbols.Add($"{gameTableNow[0, 0]}{gameTableNow[1, 0]}{gameTableNow[2, 0]}");
            winSymbols.Add($"{gameTableNow[0, 1]}{gameTableNow[1, 1]}{gameTableNow[2, 1]}");
            winSymbols.Add($"{gameTableNow[0, 2]}{gameTableNow[1, 2]}{gameTableNow[2, 2]}");
            winSymbols.Add($"{gameTableNow[0, 0]}{gameTableNow[1, 1]}{gameTableNow[2, 2]}");
            winSymbols.Add($"{gameTableNow[0, 2]}{gameTableNow[1, 1]}{gameTableNow[2, 1]}");
            string playerSymbols = $"{player}{player}{player}";
            return winSymbols.Contains(playerSymbols);
        }
        static bool CheckDraft(char[,] gameTableNow)
        {
            for (int i = 0; i < gameTableNow.GetLength(0); i++)
            {
                for (int j = 0; j < gameTableNow.GetLength(1); j++)
                {
                    if (gameTableNow[i, j] == '.')
                        return false;
                }
            }
            return true;
        }
        static void ProcessUserInput(char[,] gameTableNow, char player0orX)
        {
            Console.WriteLine($"{player0orX} zaidejo eilė.");
            Console.WriteLine();
            CheckAndChangeGameTableValue(gameTableNow, player0orX);
            PrintGameTable(gameTableNow);
            Console.WriteLine();
        }
        static void CheckAndChangeGameTableValue(char[,] gameTableNow, char player0orX)
        {
            while (true)
            {
                Console.WriteLine("Ivesk šūvio vietą: stulpeli");
                int posX = GetAndCheckUserinput();
                Console.WriteLine("Ivesk šūvio vietą: eilute");
                int posY = GetAndCheckUserinput();
                if (gameTableNow[posY, posX] == '.')
                {
                    gameTableNow[posY, posX] = player0orX;
                    break;
                }
                else
                {
                    Console.WriteLine("Langelis užimtas, Pakartok");
                }

            }
        }
        static int GetAndCheckUserinput()
        {
            int input = 0;
            bool check = false;
            while (!check)
            {
                if (int.TryParse(Console.ReadLine(), out input))
                {
                    if ((input >= 1 && input <= 3))
                        check = true;
                    else
                        Console.WriteLine("Įvestis turi buti nuo 1 iki 3");
                }
                else
                    Console.WriteLine("Bloga reikšmė. Įvestis turi buti 1 arba 2 arba 3");
            }
            return input - 1;
        }
        static char[,] ResetGameTable()
        {
            char[,] chars = new char[3, 3];
            for (int i = 0; i < chars.GetLength(0); i++)
            {
                for (int j = 0; j < chars.GetLength(1); j++)
                {
                    chars[i, j] = '.';
                }
            }
            return chars;
        }
        static void PrintGameTable(char[,] matrixToPrint)
        {
            Console.WriteLine("\t-------------------------");
            for (int i = 0; i < matrixToPrint.GetLength(0); i++)
            {
                for (int j = 0; j < matrixToPrint.GetLength(1); j++)
                {
                    Console.Write("\t" + " | " + matrixToPrint[i, j] + " | ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("\t-------------------------");
        }
        static bool ProccessSelection(string selection)
        {
            bool run = true;
            switch (selection)
            {
                case "n":
                    PrintMenu();
                    ProccessSelectionSecMenu(GetUserSelectionMenu());
                    run = true;
                    break;
                case "q":
                    Console.WriteLine("Baigiam");
                    run = false;
                    break;
                default:
                    Console.WriteLine("Baigiam");
                    run = false;
                    break;
            }
            return run;
        }
        static bool ProccessSelectionSecMenu(string selection)
        {
            bool run = true;
            switch (selection)
            {
                case "1":
                    Console.WriteLine("Vienas zaidejas pries kompiuteri : ");
                    Console.WriteLine("dar neparuosta");
                    StartNewGameSinglePlayer(1);
                    run = true;
                    break;
                case "2":
                    Console.WriteLine("Du zaidėjai");
                    StartNewGameTwoPlayers();
                    run = true;
                    break;
                default:
                    Console.WriteLine("Baigiam");
                    run = false;
                    break;
            }
            return run;
        }
        public static string GetUserSelectionMenu()
        {
            Console.WriteLine("Iveskite pasirinkimą : ");
            string? input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                return input.Trim().ToLower();
            }
            return "q";
        }
        static void PrintMenu()
        {
            Console.WriteLine("-> N - Naujas Zaidimas");
            Console.WriteLine("---> 1 - Vienas zaidejas");
            Console.WriteLine("---> 2 - Du zaidejai");
            Console.WriteLine("-> Q - exit");
        }



    }

}