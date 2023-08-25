using OOP_Restaurant_Controll_System.Models.Constructors;
using OOP_Restaurant_Controll_System.Models.FileManagers;

namespace OOP_Restaurant_Controll_System.Models
{
    internal static class UIManager
    {
        public static void MenuManager(MenuFileManager menu, TableOrderFileManager tables, StatisticsFileManager restaurantStats, double vat, string employeName)
        {
            Console.WriteLine();
            ConsoleKeyInfo readConsole;
            do
            {
                Console.Clear();
                Console.WriteLine($"\tEmployer {employeName}");
                PrintChooseMenu();
                readConsole = Console.ReadKey();
                switch (readConsole.Key)
                {
                    case ConsoleKey.F1://show menu/add menu item
                        Console.Clear();
                        PrintRestaurantMenu(menu);
                        ShowAndAddMenuItem(menu);
                        break;
                    case ConsoleKey.F2://TABLE show/make table busy/add orders/end order/
                        Console.Clear();
                        PrintTablesList(tables);
                        PrintTablesChooseMenu();
                        readConsole = Console.ReadKey();
                        TableItem tempTable;
                        switch (readConsole.Key)
                        {
                            case ConsoleKey.F1:
                                tempTable = CheckMakeTableBusy(tables, employeName);
                                CwEnterToContinue();
                                break;
                            case ConsoleKey.F2:
                                tempTable = CheckMAkeTableFree(tables);
                                CwEnterToContinue();
                                break;
                            case ConsoleKey.F3://add order
                                tempTable = tables.GetCurrentTableObject();
                                CheckAddOrder(menu, vat, tempTable);
                                CwEnterToContinue();
                                break;
                            case ConsoleKey.F4:
                                tempTable = tables.GetCurrentTableObject();
                                if (tempTable.IsFree == false && tempTable.Order.GetOrderSum() > 0)
                                {
                                    CheckEndOrder(restaurantStats, tempTable);
                                }
                                else
                                    Console.WriteLine("Table is not busy or no orders");
                                CwEnterToContinue();
                                break;
                        }
                        tables.UpdateTableFiles();
                        break;
                    case ConsoleKey.F3://Stats
                        Console.Clear();
                        Console.WriteLine(restaurantStats.GetStatisticsFromFiles());
                        CwEnterToContinue();
                        break;
                }
            } while (readConsole.Key != ConsoleKey.Escape);
        }

        private static void CwEnterToContinue()
        {
            Console.WriteLine("\r\nPress 'ENTER' to continue");
            Console.ReadKey();
        }

        private static void CheckAddOrder(MenuFileManager menu, double vat, TableItem tempTable)
        {
            if (tempTable.IsFree == false)
            {
                Console.Clear();
                Console.WriteLine($"\tTable ID: {tempTable.Id}\r\n");
                tempTable.StartOrder(vat);
                CheckGetOrderItems(menu, tempTable);
            }
            else
            {
                Console.WriteLine("Table not busy");
            }
        }

        private static void CheckEndOrder(StatisticsFileManager restaurantStats, TableItem tempTable)
        {
            Console.Clear();

            CheckPrintClientReceipt(tempTable);
            CheckGetTeaMoney(tempTable);

            CwEnterToContinue();

            tempTable.CloseOrderDate();

            StatisticItem statisticItem = tempTable.FillStatisticsItem();
            PrintRestaurantReceipt(statisticItem);

            restaurantStats.UpdateStatisticsFileFromGivenObject(tempTable.FillStatisticsItem());
            tempTable.CloseOrder();
        }

        private static void PrintRestaurantReceipt(StatisticItem statisticItem)
        {
            Console.WriteLine("\r\n\tRestaurant receipt : ");
            Console.WriteLine(statisticItem.GetStatisticRestaurantReceipt());
        }

        private static void CheckGetTeaMoney(TableItem tempTable)
        {
            ConsoleKeyInfo readConsole;
            Console.WriteLine("If client left tea money press 'F1'\r\nIf not press 'ENTER'");
            readConsole = Console.ReadKey();
            if (readConsole.Key == ConsoleKey.F1)
            {
                Console.WriteLine("Type in how much: ");
                double teaMoney = GetDoubleInputFromConsole();
                tempTable.Order.TeaMoney = teaMoney;
            }
        }

        private static void CheckPrintClientReceipt(TableItem tempTable)
        {
            ConsoleKeyInfo readConsole;
            Console.WriteLine("Client wants receipt ? If yes press F1\r\nIf not press 'ENTER'");
            readConsole = Console.ReadKey();
            if (readConsole.Key == ConsoleKey.F1)
            {
                Console.WriteLine(tempTable.GetReceiptForClient());
            }
        }

        private static void CheckGetOrderItems(MenuFileManager menu, TableItem tempTable)
        {
            ConsoleKeyInfo readConsole;
            PrintRestaurantMenu(menu);
            do
            {
                MenuItem tempMenuItem = menu.GetMenuObject();
                tempTable.Order.OrderItems.Add(tempMenuItem);
                Console.WriteLine("\r\nPress 'F1' to add item to order\r\n");
                readConsole = Console.ReadKey();
                if (!(readConsole.Key == ConsoleKey.F1))
                    break;
            } while (true);
        }

        private static TableItem CheckMAkeTableFree(TableOrderFileManager tables)
        {
            TableItem tempTable = tables.GetCurrentTableObject();
            if (tempTable.CheckIfCanBeFree())
                tempTable.IsFree = true;
            else
            {
                Console.WriteLine("Table cannot be free");
            }

            return tempTable;
        }

        private static TableItem CheckMakeTableBusy(TableOrderFileManager tables, string employeName)
        {
            TableItem tempTable;
            Console.WriteLine("How much people came in ?: ");
            int peopleCount = GetIntegerInputFromConsole();

            tempTable = tables.GetCurrentTableObject();
            if (tempTable.CheckIfCanBeBusy(peopleCount))
            {
                tempTable.SetTableBusy(employeName, peopleCount);
            }
            else
            {
                Console.WriteLine("Table cannot be busy");
            }
            return tempTable;
        }

        private static void ShowAndAddMenuItem(MenuFileManager menu)
        {
            ConsoleKeyInfo readConsole;
            Console.WriteLine("\r\nPress 'F1' to add new menu item\r\nPress 'ENTER' to continue");
            readConsole = Console.ReadKey();
            if (readConsole.Key == ConsoleKey.F1)
            {
                GetNewMenuItem(menu);
            }
        }

        private static void GetNewMenuItem(MenuFileManager menu)
        {
            bool isDrink = GetFoodOrDrink();
            Console.WriteLine("\r\nType in: \r\n\tProduct name: ");
            string productName = Console.ReadLine().Trim();
            Console.WriteLine("\tProduct description: ");
            string productDescription = Console.ReadLine().Trim();
            Console.WriteLine("\tProduct price: ");
            double productPrice = 0.00;
            productPrice = GetDoubleInputFromConsole();
            menu.MenuItems.Add(new MenuItem(menu.MenuItems.Count + 1, productName, productDescription, productPrice, DateTime.Now, isDrink));
            menu.UpdateMenuFiles();
            Console.WriteLine("Menu file updated");
        }

        private static bool GetFoodOrDrink()
        {
            bool isDrinkMethod;
            ConsoleKeyInfo readConsole;
            Console.WriteLine("\r\nProduct is Food (press 'F1'), is Drink (press 'F2')");
            readConsole = Console.ReadKey();
            if (readConsole.Key == ConsoleKey.F1)
                isDrinkMethod = false;
            else
                isDrinkMethod = true;
            return isDrinkMethod;
        }

        private static double GetDoubleInputFromConsole()
        {
            double inputDouble;
            do
            {
                double.TryParse(Console.ReadLine(), out inputDouble);
                if (inputDouble == 0.0)
                    Console.WriteLine("Type in numbers X.XX");
                else
                    break;
            } while (true);
            return inputDouble;
        }
        private static int GetIntegerInputFromConsole()
        {
            int peopleCount;
            do
            {
                int.TryParse(Console.ReadLine(), out peopleCount);
                if (peopleCount == 0.0)
                    Console.WriteLine("Type in numbers XX");
                else
                    break;
            } while (true);
            return peopleCount;
        }

        //iskelt i klase ??+
        private static void PrintTablesList(TableOrderFileManager tables)
        {
            Console.WriteLine("-----Tables-----\r\n");
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine($"| {"ID",-3} | {"Availability",-13} | {"Max seats",-10} | {"Sum",-6} |");//order sum pridet
            Console.WriteLine("---------------------------------------------");
            tables.TableItems.ForEach(x => Console.WriteLine(x.GetTableStatus()));
            Console.WriteLine();
        }

        private static void PrintRestaurantMenu(MenuFileManager menu)
        {
            Console.WriteLine("------Menu------");
            Console.WriteLine("\r\n\tFood meniu");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"| {"Id",-3} | {"Name",-20} | {"Description",-50} | {"Price",-6} |");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            menu.MenuItems.Where(x => x.IsDrink == false).ToList().ForEach(x => Console.WriteLine(x.GetFoodItem()));

            Console.WriteLine("\r\n\tDrinks meniu");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine($"| {"Id",-3} | {"Name",-20} | {"Description",-50} | {"Price",-6} |");
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            menu.MenuItems.Where(x => x.IsDrink).ToList().ForEach(x => Console.WriteLine(x.GetFoodItem()));
        }

        private static void PrintChooseMenu()
        {
            Console.WriteLine("-----Restaurant controll system-----");
            Console.WriteLine("\r\nPress your option\r\n");
            Console.WriteLine("'F1' - Menu");
            Console.WriteLine("'F2' - Tables");
            Console.WriteLine("\r\n'F3' - Statistics");
            Console.WriteLine("\r\n'ESC' - To exit");
        }

        private static void PrintTablesChooseMenu()
        {
            Console.WriteLine("\r\nPress your option\r\n");
            Console.WriteLine("'F1' - Make table busy");
            Console.WriteLine("'F2' - Make table free");
            Console.WriteLine("'F3' - Add order");
            Console.WriteLine("'F4' - End order");
            Console.WriteLine("\r\nPress 'ENTER' to continue");
        }

        public static string GetEmployerName(List<string> employers)
        {
            string employeName;
            do
            {
                Console.WriteLine("\r\n-----Restaurant controll system-----\r\n");
                Console.WriteLine("Type in registered Employe name: ");
                employeName = Console.ReadLine();
            } while (!employers.Contains(employeName));
            Console.Clear();
            return employeName;
        }
    }
}
