using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using RichardSzalay.MockHttp;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class SQLiteTest
    {
        [TestMethod]
        public void TestAddAndGetData()
        {
            // Initialize database
            SQLiteSample.InitializeDatabase();

            // Add data
            string inputText = "This is a cat fact";
            SQLiteSample.AddData(inputText);

            // Retrieve data
            List<string> result = SQLiteSample.GetData();

            // Check if data was retrieved correctly
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(inputText, result[0]);
        }

        [TestMethod]
        public void TestDeleteData()
        {
            // Initialize database
            SQLiteSample.InitializeDatabase();

            // Add data
            string inputText = "This is a cat fact";
            SQLiteSample.AddData(inputText);

            // Delete data
            SQLiteSample.DeleteData();

            // Retrieve data
            List<string> result = SQLiteSample.GetData();

            // Check if data was deleted correctly
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }
    }
}
