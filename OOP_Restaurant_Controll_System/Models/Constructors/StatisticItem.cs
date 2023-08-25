namespace OOP_Restaurant_Controll_System.Models.Constructors
{
    public class StatisticItem
    {
        public DateTime OrderStartDate { get; set; }
        public DateTime OrderEndtDate { get; set; }
        public TimeSpan OrderTime { get; set; }
        public string TableId { get; set; }
        public string EmployeName { get; set; }
        public int FreeSeats { get; set; }
        public double OrderSumWithVat { get; set; }
        public double OrderSumWifouthVat { get; set; }
        public double TeaMoney { get; set; }
        public double VatPercent { get; set; }


        public StatisticItem(DateTime orderStartDate, DateTime orderEndtDate, string tableId, string employeName, int freeSeats, double orderSumWithVat, double orderSumWifouthVat, double teaMoney, double vatPercent)
        {
            OrderStartDate = orderStartDate;
            OrderEndtDate = orderEndtDate;
            OrderTime = orderEndtDate - orderStartDate;
            TableId = tableId;
            EmployeName = employeName;
            FreeSeats = freeSeats;
            OrderSumWithVat = orderSumWithVat;
            OrderSumWifouthVat = orderSumWifouthVat;
            TeaMoney = teaMoney;
            VatPercent = vatPercent;
        }

        public StatisticItem() { }

        public string GetFileFormat()
        {
            return $"{OrderStartDate};{OrderEndtDate};{TableId};{EmployeName};{FreeSeats};{OrderSumWithVat};{OrderSumWifouthVat};{TeaMoney};{VatPercent}";
        }


        public string GetStatisticRestaurantReceipt()
        {
            return $"Table id {TableId}. \r\nEmploye {EmployeName}. \r\nOrder time {OrderTime.Minutes} minutes. " +
                $"Empty seats {FreeSeats}. \r\nTotal order amount with VAT{VatPercent} {OrderSumWifouthVat} Eur,\r\n \t wifouth {OrderSumWithVat} Eur.\r\n Tea money {TeaMoney} Eur\r\n";
        }

        public string GetStatisticData()
        {
            return $"Table id {TableId}. Employe {EmployeName}. Order time {OrderTime.Minutes} minutes. " +
                $"Empty seats {FreeSeats}. Total order amount with VAT{VatPercent} {OrderSumWifouthVat} Eur, wifouth {OrderSumWithVat} Eur. Tea money {TeaMoney} Eur";
        }

    }
}
