namespace Restaurant_Controll_System
{
    using System.Net.Mail;
    using System.Net;
    using System.Text;
    using System.Collections.Generic;

        internal class Program
        {
            static void Main(string[] args)
            {
                List<Dictionary<string, string>> foodMenu = new List<Dictionary<string, string>>();
                foodMenu = getDefaultFoodMenu();

                List<Dictionary<string, Dictionary<string, double>>> tableInfoList = new List<Dictionary<string, Dictionary<string, double>>>();
                tableInfoList = getDefaultTableInfoList();

                Dictionary<string, Dictionary<string, double>> allDayReceiptList = new Dictionary<string, Dictionary<string, double>>();
                allDayReceiptList = AllDayReceiptListAddDefaultOrders(tableInfoList);

                Console.WriteLine("Restaurant controll system");
                Console.ReadKey();
                while (true)
                {
                    Console.Clear();
                    PrintChooseMenu();
                    switch (GetUserSelectionString())
                    {
                        case "1":  //show menu
                            Console.Clear();
                            PrintFoodMenu(foodMenu);
                            GetAndAddNewMenuItem(foodMenu);
                            Console.ReadKey();
                            break;
                        case "2": // table list
                            Console.Clear();
                            PrintTableInfoList(tableInfoList);
                            TableManager(tableInfoList);
                            Console.ReadKey();
                            break;
                        case "3": // create order or add to order
                            Console.Clear();
                            PrintTableInfoList(tableInfoList);
                            AddOrderToTable(tableInfoList, foodMenu);
                            Console.ReadKey();
                            break;
                        case "4": // pay
                            Console.Clear();
                            PrintTableInfoList(tableInfoList);
                            TableOrderClosing(tableInfoList, allDayReceiptList, foodMenu);
                            Console.ReadKey();
                            break;
                        case "5": //dienos uzdarymo kvitas su visu usakymu suma
                            Console.Clear();
                            PrintAllDayReceipt(allDayReceiptList);
                            Console.ReadKey();
                            break;
                        default://iseiti
                            Console.WriteLine();
                            Environment.Exit(0);
                            break;
                    }
                }
            }

            private static void GetAndAddNewMenuItem(List<Dictionary<string, string>> foodMenu)
            {
                Console.WriteLine("\r\nAdd product ? ");
                Console.WriteLine("1 - YES, 0 - NO");
                int addProduct = GetUserSelectionInteger();
                if (addProduct.Equals(1))
                {
                    Console.WriteLine("Type in new product name");
                    string productName = GetUserSelectionString();
                    Console.WriteLine("Type in new pruduct price");
                    double productPrice = GetUserSelectionDouble();
                    Console.WriteLine("Type in new pruduct description");
                    string productDescription = GetUserSelectionString();
                    foodMenu.Add(new Dictionary<string, string> {
                                    {"Name", productName },
                                    {"Price", productPrice.ToString() },
                                    {"Description", productDescription }
                                });
                }
                //Console.WriteLine("\r\nPress 'ENTER' to go back");
            }

            static void PrintAllDayReceipt(Dictionary<string, Dictionary<string, double>> allDayReceiptList)
            {
                Console.WriteLine("Day closing receipt:\r\n");
                double sum = 0.0;
                Console.WriteLine("-------------------------");
                foreach (var receiptList in allDayReceiptList)
                {
                    Console.WriteLine($"'{receiptList.Key.ToUpper()}' table info :");
                    Console.WriteLine("\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/");
                    foreach (var item in receiptList.Value)
                    {
                        Console.WriteLine($" - {item.Key,-15} {item.Value} Eur");
                        sum += item.Value;
                    }
                    Console.WriteLine("/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\/\\");
                }
                Console.WriteLine("-------------------------");
                Console.WriteLine($"\r\nDay closing result: {sum:F2} Eur");
                Console.WriteLine("\r\nPress 'ENTER' to go back");
            }
            static void TableOrderClosing(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, Dictionary<string, Dictionary<string, double>> allDayReceiptList, List<Dictionary<string, string>> foodMenu)
            {
                Console.WriteLine("Which table wants to pay ? Type table ID and press 'ENTER'");
                string selectedTableID = GetUserSelectionString();
                if (CheckIfTableHasOrders(tableInfoList, selectedTableID) && CheckIfTableIsBusy(tableInfoList, selectedTableID))
                {
                    Console.Clear();
                    FormTableReceiptAndPrint(tableInfoList, allDayReceiptList, selectedTableID, foodMenu);//3
                    MakeTableFreeAndCleanTableOrders(tableInfoList, selectedTableID);//1.3
                }
                else
                    Console.WriteLine("Table dont have any orders");
            }
            private static void FormTableReceiptAndPrint(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, Dictionary<string, Dictionary<string, double>> allDayReceiptList, string selectedTableID, List<Dictionary<string, string>> foodMenu)
            {
                Dictionary<string, double> orderInfoUsedForSplit = new Dictionary<string, double>(); ///to reikia vien del quantity reiksmes nesaugojimo prie staliuku produktu
                StringBuilder sb = new StringBuilder();
                double sum = 0;
                sb.AppendLine($"  '{selectedTableID.ToUpper()}' table Receipt\r\n");
                sb.AppendLine("-------------------------");
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["ID"].ContainsKey(selectedTableID))
                    {
                        foreach (var item in tableInfo["OrderInfo"])
                        {
                            sum += item.Value;
                            AllDayReciptAddOrderInfo(allDayReceiptList, selectedTableID, item);
                            sb.AppendLine($"{item.Key,-15} {item.Value,5:F2} Eur");
                            SplitOrderReturSplitedDictionary(foodMenu, orderInfoUsedForSplit, item);
                        }
                    }
                }
                sb.AppendLine("\r\n-------------------------");
                sb.AppendLine($"{"Amount",-15} {sum,5:F2} Eur");
                Console.WriteLine(sb);
                Console.WriteLine("\r\nPay!");
                SplitOrder(selectedTableID, orderInfoUsedForSplit);
                Console.WriteLine($"\r\nPaid {sum,5:F2} Eur");
                sendReceiptToClient(sb);
                Console.WriteLine("\r\nPress 'ENTER' to continue\r\n");

            }

            private static void SplitOrder(string selectedTableID, Dictionary<string, double> orderInfoUsedForSplit)
            {
                Console.WriteLine("\r\nSplit order ? ");
                Console.WriteLine("1 - YES, 0 - NO");
                int splitOrder = GetUserSelectionInteger();
                if (splitOrder.Equals(1))
                {
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"  '{selectedTableID.ToUpper()}' table Receipt\r\n");
                        Console.WriteLine("-------------------------");
                        int splitIndex = 0;
                        foreach (var splitedFood in orderInfoUsedForSplit)
                        {
                            splitIndex++;
                            Console.WriteLine($"{splitIndex,3}. {splitedFood.Key,-15} {splitedFood.Value,5:F2}");
                        }
                        Console.WriteLine("-------------------------\r\n");
                        Console.WriteLine("Type in items ID's you want to pay for, and press 'ENTER'");
                        int userSelectedIndex = GetUserSelectionInteger() - 1;
                        Console.WriteLine($"\r\nPay: {orderInfoUsedForSplit.ElementAt(userSelectedIndex).Value} for your {orderInfoUsedForSplit.ElementAt(userSelectedIndex).Key}");
                        Console.WriteLine("\r\nPress 'ENTER' to continue");
                        Console.ReadKey();
                        orderInfoUsedForSplit.Remove(orderInfoUsedForSplit.ElementAt(userSelectedIndex).Key);

                    }
                    while (orderInfoUsedForSplit.Count > 0);
                }
            }

            private static void SplitOrderReturSplitedDictionary(List<Dictionary<string, string>> foodMenu, Dictionary<string, double> orderInfoUsedForSplit, KeyValuePair<string, double> item)
            {////to reikia nes nesaugau quantity reiksmiu prie stalo pridedamu komponentu. Del to reik isskaidyt
                foreach (var food in foodMenu)
                {
                    if (food["Name"].Contains(item.Key))
                    {
                        if (Convert.ToDouble(food["Price"]) == item.Value)
                        {
                            orderInfoUsedForSplit.Add(item.Key, item.Value);
                        }
                        else
                        {
                            double splitCount = item.Value / Convert.ToDouble(food["Price"]);
                            for (int i = 1; i <= splitCount; i++)
                            {
                                orderInfoUsedForSplit.Add(item.Key + " " + (i), item.Value); //(i) tam nes zodyne negali but vienodu reiksmiu
                            }
                        }
                    }
                }
            }

            static void sendReceiptToClient(StringBuilder sb)
            {
                var smtpClient = new SmtpClient("avokadas.serveriai.lt")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("sniuras@3dburst.com", "Sp5s3JPeA2Ae5Rgd"),
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                };
                Console.WriteLine("\r\nSend via email ? ");
                Console.WriteLine("1 - YES, 0 - NO");
                int sendViaEmail = GetUserSelectionInteger();
                if (sendViaEmail.Equals(1))
                {
                    Console.WriteLine("Type in client email: ");
                    string email = GetUserSelectionString();

                    smtpClient.Send("sniuras@3dburst.com", email, "Restauran controller app", "" + sb);
                    Console.WriteLine("Išsiuntėm laišką : " + email);
                }
            }

            private static void AllDayReciptAddOrderInfo(Dictionary<string, Dictionary<string, double>> allDayReceiptList, string selectedTableID, KeyValuePair<string, double> item)
            {
                if (!allDayReceiptList[selectedTableID].ContainsKey(item.Key))
                    allDayReceiptList[selectedTableID].Add(item.Key, item.Value);
                else
                    allDayReceiptList[selectedTableID][item.Key] += item.Value;
            }

            static void AddOrderToTable(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, List<Dictionary<string, string>> foodMenu)//2.1
            {
                Console.WriteLine("Which table wants to order ? Type table ID and press 'ENTER'");
                string selectedTableID = GetUserSelectionString();
                if (CheckIfTableIsBusy(tableInfoList, selectedTableID))
                {
                    Console.Clear();
                    PrintFoodMenu(foodMenu);
                    Console.WriteLine("Type menu INDEX of food you want to add, then press 'ENTER'");
                    Console.WriteLine("\r\nType 0 to exit, then press 'ENTER'\r\n");
                    foreach (var tableInfo in tableInfoList)
                    {
                        if (tableInfo["ID"].ContainsKey(selectedTableID))
                        {
                            GetUserInputAndAddOrder(foodMenu, tableInfo);
                        }
                    }
                    Console.WriteLine("\r\nPress 'ENTER' to continue");
                }
                else
                {
                    Console.WriteLine("Table is free");
                    Console.WriteLine("\r\nPress 'ENTER' to go back");
                    Console.ReadKey();
                }
            }

            private static void GetUserInputAndAddOrder(List<Dictionary<string, string>> foodMenu, Dictionary<string, Dictionary<string, double>> tableInfo)
            {
                do
                {
                    int selectedMenuInput = GetUserSelectionInteger();
                    if (selectedMenuInput <= 0)
                        break;
                    if (selectedMenuInput > foodMenu.Count)
                    {
                        Console.WriteLine("Wrong option");
                    }
                    AddFoodItemToTableOrderDictionary(foodMenu, tableInfo, selectedMenuInput);
                }
                while (true);
            }

            private static void AddFoodItemToTableOrderDictionary(List<Dictionary<string, string>> foodMenu, Dictionary<string, Dictionary<string, double>> tableInfo, int selectedMenuInput)
            {
                if (!tableInfo["OrderInfo"].ContainsKey(foodMenu[selectedMenuInput - 1]["Name"]))
                {
                    tableInfo["OrderInfo"].Add(foodMenu[selectedMenuInput - 1]["Name"], Convert.ToDouble(foodMenu[selectedMenuInput - 1]["Price"]));
                    Console.WriteLine($"Added : {foodMenu[selectedMenuInput - 1]["Name"]} : {foodMenu[selectedMenuInput - 1]["Price"]}");
                }
                else
                {
                    tableInfo["OrderInfo"][foodMenu[selectedMenuInput - 1]["Name"]] += Convert.ToDouble(foodMenu[selectedMenuInput - 1]["Price"]);
                    Console.WriteLine($"Added quantity : {foodMenu[selectedMenuInput - 1]["Name"]} : {foodMenu[selectedMenuInput - 1]["Price"]}");
                }
            }
            static bool CheckIfTableIsBusy(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, string selectedTableID)
            {
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["ID"].ContainsKey(selectedTableID))
                    {
                        return tableInfo["Availability"].ContainsKey("busy");
                    }
                }
                return false;
            }
            private static void TableManager(List<Dictionary<string, Dictionary<string, double>>> tableInfoList)
            {
                Console.WriteLine("Choose:\r\n");
                Console.WriteLine("1 - Came in");
                Console.WriteLine("2 - Came out");
                Console.WriteLine("Type 0 to exit");
                int tableMeniuActionSelection = GetUserSelectionInteger();
                if (tableMeniuActionSelection.Equals(1))
                {
                    TableManager_CameIn(tableInfoList);//1.2
                }
                else if (tableMeniuActionSelection.Equals(2))
                {
                    TableManager_CameOut(tableInfoList);//1.3
                }
                Console.WriteLine("\r\nPress enter 'ENTER' to go back");
            }
            private static void TableManager_CameOut(List<Dictionary<string, Dictionary<string, double>>> tableInfoList)
            {
                Console.WriteLine("Which table became free ? Type table ID and press 'ENTER'");
                string selectedTableID = GetUserSelectionString();
                if (!CheckIfTableHasOrders(tableInfoList, selectedTableID))  /// jei neturi uzsakymu isvalom viska
                    MakeTableFreeAndCleanTableOrders(tableInfoList, selectedTableID);
                else
                    Console.WriteLine("Table has an outstanging order");
            }
            private static void TableManager_CameIn(List<Dictionary<string, Dictionary<string, double>>> tableInfoList)
            {
                Console.WriteLine("How many people came ? Type people count and press 'ENTER'");
                double inputPeopleCount = GetUserSelectionDouble();
                if (CheckIfThereIsFreeTableWithRightAmountOfSeats(tableInfoList, inputPeopleCount))
                {
                    PrintFreeTableInfoList(tableInfoList, inputPeopleCount);
                    Console.WriteLine("Choose table: ");
                    string selectedTableID = GetUserSelectionString();
                    MakeTableBusy(tableInfoList, selectedTableID);
                }
                else
                    Console.WriteLine("No free tables");
            }

            static void PrintFreeTableInfoList(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, double inputPeopleCount)
            {
                Console.Clear();
                Console.WriteLine("Free tables info\r\n");
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["Availability"].ContainsKey("free") && (inputPeopleCount <= tableInfo["MaxSeats"].Values.Sum()))///sukeiciau if su foreachu kad suktusi daugiau kartu
                    {
                        foreach (var table in tableInfo)
                        {
                            if (table.Key == "ID")
                            {
                                Console.Write($"{table.Key,-20}");
                                Console.Write(tableInfo["ID"].First().Key);
                            }
                            if (table.Key == "Availability")
                            {
                                Console.Write($"{table.Key,-20}");
                                Console.Write(tableInfo["Availability"].First().Key);
                            }
                            if (table.Key == "MaxSeats")
                            {
                                Console.Write($"{table.Key,-20}");
                                Console.Write(tableInfo["MaxSeats"].First().Value);
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }

            static bool CheckIfTableHasOrders(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, string selectedTableID)
            {
                double sum = 0.0;
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["ID"].ContainsKey(selectedTableID))
                    {
                        sum = tableInfo["OrderInfo"].Values.Sum();
                    }
                }
                return sum > 0;
            }

            static void MakeTableBusy(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, string selectedTableID)
            {
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["ID"].ContainsKey(selectedTableID))
                    {
                        tableInfo["Availability"].Clear();
                        tableInfo["Availability"].Add("busy", 0);
                        break;//return; ////baigiam metoda
                    }
                }
            }
            static void MakeTableFreeAndCleanTableOrders(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, string selectedTableID)
            {
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["ID"].ContainsKey(selectedTableID))
                    {
                        tableInfo["Availability"].Clear();
                        tableInfo["Availability"].Add("free", 0);

                        tableInfo["OrderInfo"].Clear();
                        break;
                    }
                }
            }
            static bool CheckIfThereIsFreeTableWithRightAmountOfSeats(List<Dictionary<string, Dictionary<string, double>>> tableInfoList, double inputPeopleCount)
            {
                foreach (var tableInfo in tableInfoList)
                {
                    if (tableInfo["Availability"].ContainsKey("free") && (inputPeopleCount <= tableInfo["MaxSeats"].Values.Sum()))
                        return true;
                }
                return false;
            }

            static void PrintFoodMenu(List<Dictionary<string, string>> foodMenu)
            {
                StringBuilder stringBuilder = new StringBuilder();
                Console.WriteLine("Restaurant MENU\r\n");  ////atvaizduok stulpeliu
                int i = 0;
                foreach (var menuItems in foodMenu)
                {
                    i++;
                    Console.WriteLine($"ID : {i,3}.");
                    foreach (var itemList in menuItems)
                    {
                        Console.WriteLine($"{itemList.Key,-15} {itemList.Value} ");
                    }
                    Console.WriteLine();
                }
                Console.WriteLine("\r\nPress enter 'ENTER' to go back");
            }

            static void PrintTableInfoList(List<Dictionary<string, Dictionary<string, double>>> tableInfoList)
            {
                Console.WriteLine("Table info\r\n");
                foreach (var tableInfo in tableInfoList)//listas
                {
                    foreach (var table in tableInfo)//dictionary
                    {
                        if (table.Key == "ID")
                        {
                            Console.Write($"{table.Key,-20}");
                            Console.Write(tableInfo["ID"].First().Key); //is dictionary viduje esancio dictionary paimam prima reiksme
                        }
                        if (table.Key == "Availability")
                        {
                            Console.Write($"{table.Key,-20}");
                            Console.Write(tableInfo["Availability"].First().Key);
                        }
                        if (table.Key == "MaxSeats")
                        {
                            Console.Write($"{table.Key,-20}");
                            Console.Write(tableInfo["MaxSeats"].First().Value);
                        }
                        if (table.Key == "OrderInfo")
                        {
                            Console.Write($"{table.Key,-20}");
                            double sum = 0;
                            sum = tableInfo["OrderInfo"].Values.Sum();///skaiciuojam visu viduje esanciu values suma, key butu maisto pavadinimas, value kaina
                            Console.WriteLine($"{sum:F2}");
                        }
                        Console.WriteLine();
                    }
                }
            }

            static string GetUserSelectionString() ///skirtas gauti kazkokia reiksme ne tuscia
            {
                string? input;
                do
                {
                    input = Console.ReadLine();
                }
                while (input == null || input == "");
                return input.Trim().ToLower();
            }
            static double GetUserSelectionDouble()   ///vedam tol kol bus double
            {
                do
                {
                    double input;
                    if (double.TryParse(GetUserSelectionString(), out input))
                    {
                        return input;
                    }
                }
                while (true);
            }
            static int GetUserSelectionInteger()   ///vedam tol kol bus intas
            {
                do
                {
                    int input;
                    if (int.TryParse(GetUserSelectionString(), out input))
                    {
                        return input;
                    }
                }
                while (true);
            }
            static void PrintChooseMenu()
            {
                Console.WriteLine("Main Menu\r\n");
                Console.WriteLine("1 - Show Restauran Menu\r\n");
                Console.WriteLine("2 - Tables list");
                Console.WriteLine("3 - Create order or add to order");
                Console.WriteLine("4 - Pay");
                Console.WriteLine("\r\n5 - Closing receipt");
                Console.WriteLine("q - Quit");
                Console.WriteLine("\r\nType your choise and press enter ENTER");
            }

            //zemiau esantys metodai sudeda padaro default reiksmes testavimui ir vaizdui
            static Dictionary<string, Dictionary<string, double>> AllDayReceiptListAddDefaultOrders(List<Dictionary<string, Dictionary<string, double>>> tableInfoList)
            {
                Dictionary<string, Dictionary<string, double>> allDayReceiptList = new Dictionary<string, Dictionary<string, double>>();
                foreach (var tableInfo in tableInfoList)
                {
                    foreach (var item in tableInfo["ID"].Keys)
                    {
                        if (!allDayReceiptList.ContainsKey(item))
                        {
                            allDayReceiptList.Add(item, new Dictionary<string, double> { });
                        }
                    }
                }
                return allDayReceiptList;
            }
            static List<Dictionary<string, Dictionary<string, double>>> getDefaultTableInfoList()
            {
                List<Dictionary<string, Dictionary<string, double>>> foodMenu = new List<Dictionary<string, Dictionary<string, double>>>
            {
                new Dictionary<string, Dictionary<string, double>>()
                {
                    {
                        "ID", new Dictionary<string, double>
                        {
                            { "aa", 0.0 },
                        }
                    },
                    {
                        "MaxSeats", new Dictionary<string, double>
                        {
                            { "", 10.0 },
                        }
                    },
                    {
                        "Availability", new Dictionary<string, double>
                        {
                            { "free", 0.0 },
                        }
                    },
                    {
                        "OrderInfo", new Dictionary<string, double>
                        {
                        }
                    },
                },
                new Dictionary<string, Dictionary<string, double>>()
                {
                    {
                        "ID", new Dictionary<string, double>
                        {
                            { "bb", 0.0 },
                        }
                    },
                    {
                        "MaxSeats", new Dictionary<string, double>
                        {
                            { "", 5.0 },
                        }
                    },
                    {
                        "Availability", new Dictionary<string, double>
                        {
                            { "busy", 0.0 },
                        }
                    },
                    {
                        "OrderInfo", new Dictionary<string, double>
                        {
                            { "Pyragas", 56.99 },
                            { "Sriuba", 9.99 },
                        }
                    },
                },
                new Dictionary<string, Dictionary<string, double>>()
                {
                    {
                        "ID", new Dictionary<string, double>
                        {
                            { "cc", 0.0 },
                        }
                    },
                    {
                        "MaxSeats", new Dictionary<string, double>
                        {
                            { "", 2.0 },
                        }
                    },
                    {
                        "Availability", new Dictionary<string, double>
                        {
                            { "free", 0.0 },
                        }
                    },
                    {
                        "OrderInfo", new Dictionary<string, double>
                        {
                        }
                    },
                }
            };
                return foodMenu;
            }
            static List<Dictionary<string, string>> getDefaultFoodMenu()
            {
                List<Dictionary<string, string>> foodMenu = new List<Dictionary<string, string>>
            {
                new Dictionary<string, string>
                {
                    { "Name", "Pyragas" },
                    { "Price", "56.99" },
                    { "Description", "Alergenai ...."},
                },
                new Dictionary<string, string>
                {
                    { "Name", "Mėsainis" },
                    { "Price", "29.99" },
                    { "Description", "Mėsainis su bulvėmis ir padažu." },
                },
                new Dictionary<string, string>
                {
                    { "Name", "Sriuba" },
                    { "Price", "9.99" },
                    { "Description", "Sriuba su daržovėmis ir mėsa." },
                },
                new Dictionary<string, string>
                {
                    { "Name", "Pica" },
                    { "Price", "24.99" },
                    { "Description", "Pica su įvairiais ingredientais." },
                },
                new Dictionary<string, string>
                {
                    { "Name", "Lazanija" },
                    { "Price", "34.99" },
                    { "Description", "Mėsainio lazanija su pomidorų padažu ir sūriu." },
                },
            };
                return foodMenu;
            }
        }
    
}