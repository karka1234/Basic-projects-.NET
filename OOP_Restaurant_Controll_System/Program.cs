using OOP_Restaurant_Controll_System.Models;
using OOP_Restaurant_Controll_System.Models.FileManagers;

namespace OOP_Restaurant_Controll_System
{
    internal class Program
    {
        static void Main(string[] args) //add table padaryt
        {
            MenuFileManager menu = new MenuFileManager();
            TableOrderFileManager tables = new TableOrderFileManager();
            StatisticsFileManager restaurantStats = new StatisticsFileManager();
            menu.UpdateMenuObjectFromFile();
            tables.UpdateTableObjectFromFiles(menu);
            double vat = 21;

            List<string> employers = EmployeRegister();
            string employeName = UIManager.GetEmployerName(employers);
            UIManager.MenuManager(menu, tables, restaurantStats, vat, employeName);

            Console.WriteLine("\tBYE");
        }

        private static List<string> EmployeRegister()
        {
            List<string> employers = new List<string>();
            employers.Add("Karolis");
            employers.Add("Edvinas");
            employers.Add("Marius");
            return employers;
        }





    }
}