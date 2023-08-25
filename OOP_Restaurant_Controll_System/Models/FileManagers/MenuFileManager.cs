using OOP_Restaurant_Controll_System.Models.Constructors;

namespace OOP_Restaurant_Controll_System.Models.FileManagers
{
    internal class MenuFileManager : FileManager //paveldet is filemanagerio reikia visas klases kuriose yra listai
    {
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

        public void UpdateMenuFiles()//su enumerable t traukti info bet ten dar daug klaustuku turiu... pass
        {
            WriteToFoodFile();
            WriteDrinksFile();
        }

        private void WriteDrinksFile()
        {
            using (StreamWriter writer = new StreamWriter(filePathMenuDrinks))
            {
                MenuItems.Where(item => item.IsDrink == true).ToList().ForEach(item => writer.WriteLine(item.FoodFileFormat()));
            }
        }

        private void WriteToFoodFile()
        {
            using (StreamWriter writer = new StreamWriter(filePathMenuFood))
            {
                MenuItems.Where(item => item.IsDrink == false).ToList().ForEach(item => writer.WriteLine(item.FoodFileFormat()));
            }
        }

        public void UpdateMenuObjectFromFile()
        {
            MenuItems = new List<MenuItem>();
            ReadMenuFile(filePathMenuFood);
            ReadMenuFile(filePathMenuDrinks);
        }
        private void ReadMenuFile(string foodFilePath)
        {
            using (StreamReader reader = new StreamReader(foodFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && line != "")
                {
                    string[] elements = line.Split(';');
                    Validation.FoodFilesValidation(foodFilePath, elements);
                    MenuItems.Add(new MenuItem(Convert.ToInt16(elements[0]), elements[1], elements[2], Convert.ToDouble(elements[3]), Convert.ToDateTime(elements[4]), Convert.ToBoolean(elements[5])));
                }
            }
        }

        public MenuItem GetMenuObject()
        {
            Console.WriteLine("Type in menu item Id");
            MenuItem menuItemObject;
            do
            {
                int inputId;
                int.TryParse(Console.ReadLine(), out inputId);
                if (inputId == 0.0)
                    Console.WriteLine("Type in numbers XX");
                else
                {
                    if (MenuItems.Where(x => x.Id == inputId).Count() == 1)
                    {
                        menuItemObject = MenuItems.Where(x => x.Id == inputId).First();
                        break;
                    }
                    else
                        Console.WriteLine("Menu item not found");
                }
            } while (true);

            return menuItemObject;
        }
    }
}
