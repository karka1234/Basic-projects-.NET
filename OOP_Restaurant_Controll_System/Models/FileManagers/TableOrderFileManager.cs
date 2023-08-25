using OOP_Restaurant_Controll_System.Models.Constructors;

namespace OOP_Restaurant_Controll_System.Models.FileManagers
{
    internal class TableOrderFileManager : FileManager
    {
        public List<TableItem> TableItems { get; set; } = new List<TableItem>();
        public void UpdateTableFiles()
        {
            foreach (var item in TableItems)
            {
                string tableInfoFilePath = filePathTables + item.Id + ".csv";
                using (StreamWriter writer = new StreamWriter(tableInfoFilePath))
                {
                    writer.WriteLine(item.TableFileFormat());
                    writer.WriteLine(item.Order.OrderFileFormat());
                    writer.WriteLine(item.Order.OrderMenuItemsId());
                }
            }
        }
        public void UpdateTableObjectFromFiles(MenuFileManager menu)
        {
            TableItems = new List<TableItem>();
            string[] activeTableList = Directory.GetFiles(filePathTables).Select(x => x.Split('\\').Last()).ToArray();
            foreach (var tableIdFile in activeTableList)
            {
                string tableInfoFilePath = filePathTables + tableIdFile;
                using (StreamReader reader = new StreamReader(tableInfoFilePath))
                {
                    string[] tableInfoLine = reader.ReadLine().Split(';');
                    string[] orderInfoLine = reader.ReadLine().Split(';');
                    Validation.TableAndORdersFileValidate(tableInfoFilePath, tableInfoLine, orderInfoLine);
                    string line;
                    List<MenuItem> menuItems = new List<MenuItem>();
                    while ((line = reader.ReadLine()) != null && line != "")
                    {
                        GetAndCheckMenuItemFromFile(menu, menuItems, line);
                    }
                    SetTableObject(tableInfoLine, orderInfoLine, menuItems);
                }
            }
        }

        private void SetTableObject(string[] tableInfoLine, string[] orderInfoLine, List<MenuItem> menuItems)
        {
            TableItem table = new TableItem(tableInfoLine[0].ToLower(), Convert.ToBoolean(tableInfoLine[1]), Convert.ToInt16(tableInfoLine[2]), Convert.ToInt16(tableInfoLine[3]));
            table.Order = new OrderItem(menuItems, orderInfoLine[0], Convert.ToDateTime(orderInfoLine[1]), Convert.ToDateTime(orderInfoLine[2]), Convert.ToDouble(orderInfoLine[3]));
            TableItems.Add(table);
        }

        private static void GetAndCheckMenuItemFromFile(MenuFileManager menu, List<MenuItem> menuItems, string line)
        {
            if (int.TryParse(line, out int menuId))
                menuItems.Add(menu.MenuItems.Where(x => x.Id == menuId).First());
            else
                throw new Exception($"{line} incorrect format. MenuId");
        }

        public TableItem GetCurrentTableObject()
        {
            Console.WriteLine("Type in table Id: ");
            TableItem tableObject;
            do
            {
                string inputId = Console.ReadLine().Trim();
                if (TableItems.Where(x => x.Id == inputId).Count() == 1)
                {
                    tableObject = TableItems.Where(x => x.Id == inputId).First();
                    break;
                }
                else
                    Console.WriteLine("Table not found");
            } while (true);
            return tableObject;
        }

    }
}
