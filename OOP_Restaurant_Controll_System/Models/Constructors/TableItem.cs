using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OOP_Restaurant_Controll_System.Models.Constructors
{
    public class TableItem
    {
        public string Id { get; set; }
        public bool IsFree { get; set; }
        public int MaxSeat { get; set; }
        public int FreeSeats { get; set; } 
        public OrderItem Order { get; set; }
        

        public TableItem(string id, bool isFree, int maxSeat, int notUsedSeats)
        {
            Id = id;
            IsFree = isFree;
            MaxSeat = maxSeat;
            FreeSeats = notUsedSeats;
        }

        public TableItem(string id, bool isFree, int maxSeat, int notUsedSeats, OrderItem order)
        {
            Id = id;
            IsFree = isFree;
            MaxSeat = maxSeat;
            FreeSeats = notUsedSeats;
            Order = order;
        }

        public TableItem()
        {
        }

        public string TableFileFormat()
        {
            return $"{Id};{IsFree};{MaxSeat};{FreeSeats}";
        }

        public string GetTableStatus()
        {
            string freeOrBusy = IsFree ? "Free" : "Busy";
            return $"| {Id,-3} | {freeOrBusy,-13} | {MaxSeat,-10} | {Order.GetOrderSum(),-6:F2} |";
        }


        public bool CheckIfCanBeBusy(int peopleCount)
        {
            return ((MaxSeat >= peopleCount) && IsFree == true);
        }

        public bool CheckIfCanBeFree()
        {
            return (Order.CheckIfOrderIsEmpty() && IsFree == false);
        }

        public StringBuilder GetReceiptForClient()// perkelt i uimanageri
        {
            StringBuilder receipt = new StringBuilder();
            receipt.AppendLine($"\tReceipt");
            receipt.AppendLine($"Table {Id}");
            receipt.AppendLine($"Employer name {Order.EmployerName}");
            receipt.AppendLine($"---------------------------");
            receipt.AppendLine($"| {"Name",-20} | {"Price",-6:F2} |");
            receipt.AppendLine($"---------------------------");
            Order.OrderItems.ForEach(x => receipt.AppendLine(x.GetFoodItemForReceipt()));
            receipt.AppendLine($"---------------------------");
            receipt.AppendLine($"Total sum with VAT{Order.VatPercent}: {Order.GetOrderSum()} Eur");
            receipt.AppendLine($"Total sum wifouth VAT{Order.VatPercent}: {Order.GetOrderSumMinusVat()} Eur");
            return receipt;
        }

        public void CloseOrder()
        {
            IsFree = true;
            Order = new OrderItem();
            FreeSeats = 0;
        }

        public void CloseOrderDate()
        {
            Order.EndDate = DateTime.Now;
        }

        public void SetTableBusy(string employeName, int peopleCountCameIn)
        {
            IsFree = false;
            FreeSeats = (MaxSeat - peopleCountCameIn);
            Order.EmployerName = employeName;
        }

        public void StartOrder(double vat)
        {
            Order.StartDate = DateTime.Now;
            Order.VatPercent = vat;
        }

        public StatisticItem FillStatisticsItem()
        {
            StatisticItem stats = new StatisticItem(Order.StartDate, Order.EndDate, Id, Order.EmployerName, FreeSeats, (Order.GetOrderSumMinusVat()), Order.GetOrderSum(), Order.TeaMoney, Order.VatPercent);
            return stats;
        }
    }
}
