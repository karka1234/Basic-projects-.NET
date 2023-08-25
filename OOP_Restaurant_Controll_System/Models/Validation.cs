namespace OOP_Restaurant_Controll_System.Models
{
    internal static class Validation
    {

        public static void FoodFilesValidation(string foodFilePath, string[] elements)
        {
            if (elements.Length != 6)
                throw new Exception($"{foodFilePath} incorrect format. MenuItems");
        }

        public static void StatisticFileValidate(string statFilePath, string[] lineContent)
        {
            if (lineContent.Count() != 9)
                throw new Exception("Incorrect statistic file format : " + statFilePath);
        }

        public static void TableAndORdersFileValidate(string tableInfoFilePath, string[] tableInfoLine, string[] orderInfoLine)
        {
            if (tableInfoLine.Length != 4 && orderInfoLine.Length != 5)
                throw new Exception($"{tableInfoFilePath} incorrect format. Table/Order");
        }
    }
}
