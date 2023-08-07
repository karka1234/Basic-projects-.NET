using Basic_ATM.Models;
using System.Security.Cryptography;

namespace Basic_ATM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //data init
            //passwordai 1111 2222 3333
            Guid[] existingUsers = { new Guid("93e56216-ab9e-419e-b80e-f225690851db"), new Guid("2897d2e0-34f0-42eb-9da9-8bfc8bae4bff"), new Guid("ede94898-6570-4f77-b2f5-fae40987ab14") };//galim is failu direktorijos nuskaityti manau kiek yra vartotoju
            string accountInfoPath = "C:\\Users\\karsi\\source\\repos\\NET_praktika_uzdaviniai_projektai_OOP\\Basic_ATM\\UserDataFiles\\AccountInfo\\";
            string identificationInfoPath = "C:\\Users\\karsi\\source\\repos\\NET_praktika_uzdaviniai_projektai_OOP\\Basic_ATM\\UserDataFiles\\Identification\\";
            string transactionInfoPath = "C:\\Users\\karsi\\source\\repos\\NET_praktika_uzdaviniai_projektai_OOP\\Basic_ATM\\UserDataFiles\\Transactions\\";
            int userId = GetUserIdFromConsoleInput(existingUsers);
            
            DataFileManager userDataFileManager = new DataFileManager(accountInfoPath, identificationInfoPath, transactionInfoPath, existingUsers[userId - 1]);
            UserInfo userInfo = OpenUserObject(userDataFileManager);
            MenuManager(userDataFileManager, userInfo);
        }



        private static void MenuManager(DataFileManager userDataFileManager, UserInfo userInfo)
        {
            int trancactionCounter = 10;
            ConsoleKeyInfo readConsole;
            do
            {
                Console.Clear();
                Console.WriteLine("ATM\r\n");
                userInfo.PrintUserInfo();
                PrintMenu();
                readConsole = Console.ReadKey();
                switch (readConsole.Key)
                {
                    case ConsoleKey.Q:
                        Console.Clear();
                        Console.WriteLine("Tikrinti likuti\r\n");
                        userInfo.PrintMoneyLeft();
                        Console.WriteLine("\r\n'ENTER'");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.W:
                        Console.Clear();
                        Console.WriteLine("Ideti pinigu\r\n");
                        if (trancactionCounter > 0)
                        {                            
                            UpdateMoneyBalanceAdd(userDataFileManager, userInfo);
                            trancactionCounter--;
                        }else
                            Console.WriteLine("Pasiektas transakciju limitas");
                        Console.WriteLine("\r\n'ENTER'");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.E:
                        Console.Clear();
                        Console.WriteLine("Isimti pinigu\r\n");
                        if (trancactionCounter > 0)
                        {
                            UpdateMoneyBalanceMinus(userDataFileManager, userInfo);
                            trancactionCounter--;
                        }else
                            Console.WriteLine("Pasiektas transakciju limitas");
                        Console.WriteLine("\r\n'ENTER'");
                        Console.ReadKey();
                        break;
                    case ConsoleKey.R:
                        Console.Clear();
                        Console.WriteLine("Buvusios tranzakcijos\r\n");
                        userInfo.PrintTransactions();
                        Console.WriteLine("\r\n'ENTER'");
                        Console.ReadKey();
                        break;
                }
            } while (readConsole.Key != ConsoleKey.Escape);
        }

        private static void UpdateMoneyBalanceAdd(DataFileManager userDataFileManager, UserInfo userInfo)
        {
            double inputMoney = Convert.ToDouble(Console.ReadLine());
            userInfo.MoneyLeft += inputMoney;
            userDataFileManager.UpdateMoneyLeft(userInfo.DataStringToFile());
            userDataFileManager.AddTransactionToFileAndObject(userInfo, "Pinigu inesimas", inputMoney);
        }

        private static void UpdateMoneyBalanceMinus(DataFileManager userDataFileManager, UserInfo userInfo)
        {
            double inputMoney = Convert.ToDouble(Console.ReadLine());
            if (inputMoney > userInfo.MoneyLeft)
            {
                Console.WriteLine("Neturite tiek pinigu");

            }
            else if (inputMoney > 1000)
            {
                Console.WriteLine("Maksimali isemimo suma yra 1000");
            }
            else
            {
                userInfo.MoneyLeft -= inputMoney;
                userDataFileManager.UpdateMoneyLeft(userInfo.DataStringToFile());
                userDataFileManager.AddTransactionToFileAndObject(userInfo, "Pinigu isemimas", inputMoney);
            }
        }

        private static UserInfo OpenUserObject(DataFileManager dataFileManager)
        {
            int passAttempts = 0;
            string inputPass;
            do
            {
                if (passAttempts > 0)
                {
                    Console.WriteLine($"Iveskite slaptazodi : XXXX. Liko : {3 - passAttempts} bandymai/u");
                }
                Console.WriteLine($"Iveskite slaptazodi : XXXX");
                inputPass = Console.ReadLine().Trim();
                passAttempts++;
                if (passAttempts >= 3)
                    Environment.Exit(0);
            } while (!(dataFileManager.CheckPassword(inputPass)));
            UserInfo userInfo = new UserInfo();
            userInfo = dataFileManager.GetUserDataFromFile();
            return userInfo;
        }

        private static int GetUserIdFromConsoleInput(Guid[] existingUsers)
        {
            Console.WriteLine("Galimi vartotojai :");
            Console.WriteLine();
            for (int i = 0; i < existingUsers.Length; i++)
            {
                Console.WriteLine((i + 1) + ". Kortelės ID : " + existingUsers[i].ToString());
            }
            Console.WriteLine();
            Console.WriteLine("Pasirinkite vartotoją ir spauskite ENTER");
            Console.WriteLine();
            int userId = Convert.ToInt32(Console.ReadLine());
            return userId;
        }

        private static void PrintMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Q - Tikrinti pinigu likuti");
            Console.WriteLine("W - Ideti pinigu i banka");
            Console.WriteLine("E - Isimti pinigu is banko");
            Console.WriteLine("R - Buvusios transakcijos");
            Console.WriteLine();
            Console.WriteLine("ESC - išeiti");
            Console.WriteLine();
            Console.WriteLine("Paspuaskite norimos operacijos simbolį");
        }




        


    }
}