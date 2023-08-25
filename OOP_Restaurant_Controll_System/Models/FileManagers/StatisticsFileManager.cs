using OOP_Restaurant_Controll_System.Models.Constructors;
using System.Text;

namespace OOP_Restaurant_Controll_System.Models.FileManagers
{
    internal class StatisticsFileManager : FileManager
    {
        public void UpdateStatisticsFileFromGivenObject(StatisticItem item)
        {
            string statInfoFilePath = filePathStatistics + item.OrderEndtDate.ToString("dd MM yyyy") + ".csv";
            using (StreamWriter writer = new StreamWriter(statInfoFilePath, true))
            {
                writer.WriteLine(item.GetFileFormat());
            }
            item = new StatisticItem();
        }

        public StringBuilder GetStatisticsFromFiles()//nenorejau kur objektu nu nes tipo nereikia nes maziau ramu naudoja programos veikimo metu o ir norejau pabandyt taip
        {
            string[] statisticFiles = Directory.GetFiles(filePathStatistics).Select(x => x.Split('\\').Last()).ToArray();

            StringBuilder statictisData = new StringBuilder();
            Dictionary<string, int> employerOrders = new Dictionary<string, int>();

            foreach (var statFile in statisticFiles)
            {
                List<StatisticItem> statisticsDayItems = new List<StatisticItem>();
                string statFilePath = filePathStatistics + statFile;
                using (StreamReader reader = new StreamReader(statFilePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            FillObjectsFromFile(employerOrders, statisticsDayItems, statFilePath, line);
                        }
                    }
                }
                AppendOrderFilesData(statictisData, statFile, statisticsDayItems);
            }
            AppendEmployerResultData(statictisData, employerOrders);
            return statictisData;
        }

        private static void AppendEmployerResultData(StringBuilder statictisData, Dictionary<string, int> employerOrders)
        {
            statictisData.AppendLine("\r\nEmployers stats");
            statictisData.AppendLine("------------------------------------");
            employerOrders.Select(i => $"{i.Key}: {i.Value}").ToList().ForEach(x => statictisData.AppendLine(x));
        }

        private static void AppendOrderFilesData(StringBuilder statictisData, string statFile, List<StatisticItem> statisticsDayItems)
        {
            statictisData.AppendLine($"\r\nStaistics date : {statFile}");
            statictisData.AppendLine("------------------------------------");
            statisticsDayItems.ForEach(x => statictisData.AppendLine(x.GetStatisticData()));
            statictisData.AppendLine("------------------------------------");
            statictisData.AppendLine($"Total day amount with VAT{statisticsDayItems.First().VatPercent} {statisticsDayItems.Select(x => x.OrderSumWithVat).Sum(),6:F2}");
            statictisData.AppendLine($"Total day amount wifouth VAT{statisticsDayItems.First().VatPercent} {statisticsDayItems.Select(x => x.OrderSumWifouthVat).Sum(),6:F2}");
            statictisData.AppendLine("------------------------------------");
        }


        private static void FillObjectsFromFile(Dictionary<string, int> employerOrders, List<StatisticItem> statisticsDayItems, string statFilePath, string line)
        {
            string[] lineContent = line.Split(';');
            Validation.StatisticFileValidate(statFilePath, lineContent);
            statisticsDayItems.Add(new StatisticItem(Convert.ToDateTime(lineContent[0]), Convert.ToDateTime(lineContent[1]), lineContent[2], lineContent[3], Convert.ToInt16(lineContent[4]), Convert.ToDouble(lineContent[5]), Convert.ToDouble(lineContent[6]), Convert.ToDouble(lineContent[7]), Convert.ToDouble(lineContent[8])));
            CollectEmployeOrders(employerOrders, lineContent);
        }

        private static void CollectEmployeOrders(Dictionary<string, int> employerOrders, string[] lineContent)
        {
            if (employerOrders.ContainsKey(lineContent[3]))
                employerOrders[lineContent[3]] += 1;
            else
                employerOrders.Add(lineContent[3], 1);
        }
    }
}
