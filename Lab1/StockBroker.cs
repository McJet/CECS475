﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace Stock
{
    public class StockBroker
    {
        public string BrokerName { get; set; }

        public List<Stock> stocks = new List<Stock>();

        public static ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();
        readonly string docPath = @"C:\Users\Documents\CECS 475\Lab1_output.txt";

        public string titles = "Broker".PadRight(10) + "Stock".PadRight(15) +
       "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time";

        /// <summary>
        /// The stockbroker object
        /// </summary>
        /// <param name="brokerName">The stockbroker's name</param>
        public StockBroker(string brokerName)
        {
            BrokerName = brokerName;
        }
        /// <summary>
        /// Adds stock objects to the stock list
        /// </summary>
        /// <param name="stock">Stock object</param>
        public void AddStock(Stock stock)
        {
            stocks.Add(stock); // adds stock to the stock list
            stock.OnStockEvent += EventHandler; // subscribes the broker the the StockEvent
        }
        /// <summary>
        /// The eventhandler that raises the event of a change
        /// </summary>
        /// <param name="sender">The sender that indicated a change</param>
        /// <param name="e">Event arguments</param>
        private void EventHandler(Object sender, StockNotification sn)
        {
            try
            {
                Stock newStock = (Stock)sender;
                string statement;
                statement = BrokerName + " " + sn.StockName + " " + sn.CurrentValue + " " + sn.NumChanges;
                Console.WriteLine(statement);

            }
            catch
            {

            }
        }
    }
}