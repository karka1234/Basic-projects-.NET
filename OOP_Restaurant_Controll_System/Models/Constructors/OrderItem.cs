using System.Text;

namespace OOP_Restaurant_Controll_System.Models.Constructors
{
    public class OrderItem
    {
        public List<MenuItem> OrderItems { get; set; } = new List<MenuItem>();
        public string EmployerName { get; set; }
        public double TeaMoney { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double VatPercent { get; set; }


        public OrderItem(List<MenuItem> orderItems, string employerName, DateTime startDate, DateTime endDate, double vatPercent)
        {
            OrderItems = orderItems;
            EmployerName = employerName;
            StartDate = startDate;
            EndDate = endDate;
            VatPercent = vatPercent;
        }

        public OrderItem(List<MenuItem> orderItems, string employerName, DateTime startDate, double vatPercent)
        {
            OrderItems = orderItems;
            EmployerName = employerName;
            StartDate = startDate;
            VatPercent = vatPercent;
        }

        public string OrderFileFormat()
        {
            return $"{EmployerName};{StartDate};{EndDate};{VatPercent}";
        }

        public StringBuilder OrderMenuItemsId()
        {
            StringBuilder orderItems = new StringBuilder();
            foreach (MenuItem item in OrderItems)
            {
                orderItems.AppendLine(item.Id.ToString());
            }
            return orderItems;
        }

        public bool CheckIfOrderIsEmpty()//cia jei prideciau kokiu nemokamu dalyku del to pagal kieki o ne suma
        {
            return OrderItems.Count == 0;
        }

        public double GetOrderSum()
        {
            return Math.Round(OrderItems.Select(x => x.Price).Sum(), 2);
        }
        public double GetOrderSumMinusVat()
        {
            return Math.Round(OrderItems.Select(x => x.Price).Sum() * (1 - (VatPercent / 100)), 2);
        }

        public OrderItem()
        {
        }
    }
}
