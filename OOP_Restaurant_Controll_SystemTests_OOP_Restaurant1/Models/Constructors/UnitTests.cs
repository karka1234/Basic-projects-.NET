using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OOP_Restaurant_Controll_System.Models.Constructors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP_Restaurant_Controll_System.Models.Constructors.Tests
{
    [TestClass()]
    public class UnitTests
    {
        [TestMethod()]
        public void GetOrderSumWithVatTest_TestIfVatCalculatedCorrectly()
        {
            //Arrange           
            List<MenuItem> OrderItems = new List<MenuItem>();
            OrderItems.Add(new MenuItem(1, "Test1", "Dtest1", 5.00, DateTime.Now, true));
            OrderItems.Add(new MenuItem(2, "Test2", "Dtest2", 10.50, DateTime.Now, false));
            OrderItem item = new OrderItem(OrderItems, "Etest1", DateTime.Now, 20);
            double expected = 12.4;
            //Act
            double result = item.GetOrderSumMinusVat();
            //Asset
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void GetOrderSumTest_TestIfSumCalculatedCorectly()
        {
            //Arrange           
            List<MenuItem> OrderItems = new List<MenuItem>();
            OrderItems.Add(new MenuItem(1, "Test1", "Dtest1", 5.00, DateTime.Now, true));
            OrderItems.Add(new MenuItem(2, "Test2", "Dtest2", 10.50, DateTime.Now, false));
            OrderItem item = new OrderItem(OrderItems, "Etest1", DateTime.Now, 20);
            double expected = 15.5;
            //Act
            double result = item.GetOrderSum();
            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod()]
        public void OrderFileFormatTest_TestOrderFileFormat()
        {
            //Arrange
            List<MenuItem> OrderItems = new List<MenuItem>();
            OrderItems.Add(new MenuItem(1, "Test1", "Dtest1", 5.00, DateTime.MinValue, true));
            OrderItems.Add(new MenuItem(2, "Test2", "Dtest2", 10.50, DateTime.MinValue, false));
            OrderItem item = new OrderItem(OrderItems, "Etest1", DateTime.MinValue, 20);
            string expected = "Etest1;1/1/0001 0:00:00;1/1/0001 0:00:00;20";
            //Act
            string actual = item.OrderFileFormat();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TableFileFormatTest_TestTableFileFormat()
        {
            //Arrange
            TableItem tableItem = new TableItem("test", false, 10, 1);
            string expected = "test;False;10;1";
            //Act
            string actual = tableItem.TableFileFormat();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void FoodFileFormatTest_TestFoodFilesFormat()
        {
            //Arrange
            MenuItem item = new MenuItem(1, "Test1", "Dtest1", 5.00, DateTime.MinValue, true);
            string expected = "1;Test1;Dtest1;5;1/1/0001 0:00:00;True";
            //Act
            string actual = item.FoodFileFormat();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void GetFileFormatTest_TestStatisticFileFormat()
        {
            //Arrange
            StatisticItem item = new StatisticItem(DateTime.MinValue, DateTime.MinValue, "test", "etest", 5, 5, 5, 5, 20);
            string expected = "1/1/0001 0:00:00;1/1/0001 0:00:00;test;etest;5;5;5;5;20";
            //Act
            string actual = item.GetFileFormat();
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void CloseOrderTest_TEstIfValuesChanged()
        {
            //Arrange
            List<MenuItem> OrderItems = new List<MenuItem>();
            OrderItems.Add(new MenuItem(1, "Test1", "Dtest1", 5.00, DateTime.MinValue, true));
            OrderItems.Add(new MenuItem(2, "Test2", "Dtest2", 10.50, DateTime.MinValue, false));
            OrderItem item = new OrderItem(OrderItems, "Etest1", DateTime.MinValue, 20);
            TableItem actual = new TableItem("test", false, 10, 1, item);

            TableItem expected = new TableItem("test", true, 10, 0, new OrderItem());
            //Act
            actual.CloseOrder();
            //Assert
            Assert.AreEqual(expected.TableFileFormat(), actual.TableFileFormat());//iskart i stringa
        }

        [TestMethod()]
        public void SetTableBusyTest_TEstIfValuesChanged()
        {
            //Arrange
            List<MenuItem> OrderItems = new List<MenuItem>();
            OrderItems.Add(new MenuItem(1, "Test1", "Dtest1", 5.00, DateTime.MinValue, true));
            OrderItems.Add(new MenuItem(2, "Test2", "Dtest2", 10.50, DateTime.MinValue, false));
            OrderItem item = new OrderItem(OrderItems, "newPErsonTest", DateTime.MinValue, 20);
            TableItem actual = new TableItem("test", true, 10, 5, item);

            TableItem expected = new TableItem("test", false, 10, 5, new OrderItem());
            //Act
            actual.SetTableBusy(expected.Order.EmployerName, 5);
            //Assert
            Assert.AreEqual(expected.TableFileFormat(), actual.TableFileFormat());
        }

    }
}