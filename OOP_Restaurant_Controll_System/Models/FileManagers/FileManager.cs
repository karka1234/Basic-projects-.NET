namespace OOP_Restaurant_Controll_System.Models.FileManagers
{
    internal class FileManager
    {
        private static string filePathMenu = @"C:\";
        protected static string filePathStatistics = @"C:\";
        protected static string filePathTables = @"C:\";
        //protected kad pasiektu paveldintys
        protected static string filePathMenuFood = filePathMenu + "food.csv";
        protected static string filePathMenuDrinks = filePathMenu + "drinks.csv";
        public FileManager()
        {
            CheckIfDirectoriesExists();
            CheckIfMenuItemsExists();
        }
        private void CheckIfDirectoriesExists()
        {
            if (!(Directory.Exists(filePathMenu) && Directory.Exists(filePathTables) && Directory.Exists(filePathStatistics)))
                throw new Exception("Some of Directories not existing");

        }
        private void CheckIfMenuItemsExists()
        {
            if (!File.Exists(filePathMenuDrinks))
                throw new Exception("Drinks folder not exists :" + filePathMenuDrinks);
            if (!File.Exists(filePathMenuFood))
                throw new Exception("Food folder not exists :" + filePathMenuFood);
        }

    }
}
