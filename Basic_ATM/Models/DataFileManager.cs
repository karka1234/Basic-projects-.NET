using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Basic_ATM.Models
{
    internal class DataFileManager : PasswordsHashingWithSalt
    {
        public string AccountFolderPath { get; set; }
        public string IdentificationFolderPath { get; set; }
        public string TranzactionFolderPath { get; set; }
        public Guid IdentificationId { get; set; }

        private string PassHash;
        private byte[] Salt;

        private string AccountFulFilePath;
        private string IdentificationFulFilePath;
        private string IdentificationFulFilePathSalt;
        private string TransactionsnFulFilePath;

        public DataFileManager(string accountFolderPath, string identificationFolderPath, string tranzactionFolderPath, Guid identificationId)
        {
            AccountFolderPath = accountFolderPath;
            IdentificationFolderPath = identificationFolderPath;
            TranzactionFolderPath = tranzactionFolderPath;            
            IdentificationId = identificationId;
            InitFilePAths();
            FilesExists();            
        }

        public DataFileManager()
        {
        }

        private void FilesExists()
        {
            if (!File.Exists(AccountFulFilePath))
            {
                throw new Exception($"{AccountFulFilePath}  file not existing" );
            }
            if (!File.Exists(IdentificationFulFilePath))
            {
                throw new Exception($"{IdentificationFulFilePath} file not existing");
            }
            if (!File.Exists(TransactionsnFulFilePath))
            {
                throw new Exception($"{TransactionsnFulFilePath} file not existing");
            }
            if (!File.Exists(IdentificationFulFilePathSalt))
            {
                throw new Exception($"{IdentificationFulFilePathSalt} file not existing");
            }
        }

        private void InitFilePAths()
        {
            AccountFulFilePath = $"{AccountFolderPath}{IdentificationId}.txt";
            IdentificationFulFilePath = $"{IdentificationFolderPath}{IdentificationId}.txt";
            IdentificationFulFilePathSalt = $"{IdentificationFolderPath}{IdentificationId}_salt.txt";
            TransactionsnFulFilePath = $"{TranzactionFolderPath}{IdentificationId}.txt";            
        }

        public bool CheckPassword(string inputPass)
        {
            string[] hashData = File.ReadAllLines(IdentificationFulFilePath);
            byte[] saltData = File.ReadAllBytes(IdentificationFulFilePathSalt);
            return VerifyPassword(inputPass, hashData[0], saltData);
        }

        public void CreateAndWritePasswordToFile(string newPassword)//salt ir hacha i skirtingus failuis
        {
            PassHash = CreatePasswordForUser(newPassword,out Salt);
            File.WriteAllBytes(IdentificationFulFilePathSalt, Salt);
            File.WriteAllText(IdentificationFulFilePath, PassHash);           
        }

        public void UpdateMoneyLeft(string dataToFile)
        {
            File.WriteAllText(AccountFulFilePath, dataToFile);
        }

        private List<UserTransactionsInfo> GetTransactions()
        {
            List<UserTransactionsInfo> transactionsInfo = new List<UserTransactionsInfo>();
            string[] fileInfo = File.ReadAllLines(TransactionsnFulFilePath);
            foreach (string fileLine in fileInfo)
            {
                if (fileLine != "")
                {
                    string[] transInfoRow = fileLine.Split(';');
                    transactionsInfo.Add(new UserTransactionsInfo() { UserID = IdentificationId, Title = transInfoRow[0], Sum = Convert.ToDouble(transInfoRow[1]), TransactionDate = Convert.ToDateTime(transInfoRow[2]) });
                }
            }
            return transactionsInfo;
        }

        public UserInfo GetUserDataFromFile()
        {           
            string[] fileInfo = File.ReadAllLines(AccountFulFilePath);
            string[] userInfoRow = fileInfo[0].Split(';');
            UserInfo userInfo = new UserInfo();
            userInfo.UserID = IdentificationId;
            userInfo.FirstName = userInfoRow[0];
            userInfo.LastName = userInfoRow[1];
            userInfo.Email = userInfoRow[2];
            userInfo.MoneyLeft = Convert.ToDouble(userInfoRow[3]);
            userInfo.OpenTime = Convert.ToDateTime(userInfoRow[4]);
            userInfo.UserTransactionsInfos = GetTransactions();
            return userInfo;
        }


        public void AddTransactionToFileAndObject(UserInfo userInfo, string title, double amount)
        { 
            UserTransactionsInfo transactionsInfo = new UserTransactionsInfo(title, amount);
            File.AppendAllText(TransactionsnFulFilePath, "\r\n" + transactionsInfo.DataStringToFile());
            userInfo.UserTransactionsInfos.Add(transactionsInfo);
        }


    }
}
