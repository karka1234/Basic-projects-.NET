using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basic_ATM.Models
{
    internal class UserTransactionsInfo
    {
        public Guid UserID { get; set; }
        public string Title{ get; set; }
        public double Sum { get;set; }
        public DateTime TransactionDate { get; set; }
        public UserTransactionsInfo() { }

        public UserTransactionsInfo(string title, double sum)//kai kuriam nauja nustatom dabartine data
        {
            Title = title;
            Sum = sum;
            TransactionDate = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Title,-30} {TransactionDate.Year}:{TransactionDate.Month}:{TransactionDate.Day} {Sum} ";
        }

        public string DataStringToFile()//galima buvo i kiekviena eilute tiesiog butu paprasciau
        {
            return $"{Title};{Sum};{TransactionDate.Day}/{TransactionDate.Month}/{TransactionDate.Year};";
        }


    }
}
