using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Basic_ATM.Models
{
    internal class UserInfo
    {
        public Guid UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public double MoneyLeft { get; set; }
        public List<UserTransactionsInfo> UserTransactionsInfos { get; set; }
        public DateTime OpenTime { get; set; }

        public UserInfo() {}

        public void PrintUserInfo()
        {
            Console.WriteLine($"{FirstName} {LastName} {Email}");
        }

        public void PrintMoneyLeft()
        {
            Console.WriteLine($"Pinigu likutis : {MoneyLeft}");
        }

        public void PrintTransactions()
        {
            UserTransactionsInfos.ForEach( ( t ) => Console.WriteLine( t.ToString() ) );
        }

        public string DataStringToFile()//galima buvo i kiekviena eilute tiesiog butu paprasciau
        {
            return $"{FirstName};{LastName};{Email};{MoneyLeft};{OpenTime.Day}/{OpenTime.Month}/{OpenTime.Year}";
        }





    }
}
