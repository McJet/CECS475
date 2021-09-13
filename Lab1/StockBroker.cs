using System;
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
        readonly string docPath = @"C:\Users\Jether\Documents\CECS 475\Lab1_output.txt";
        public string titles = "Broker".PadRight(10) + "Stock".PadRight(15) + "Value".PadRight(10) + "Changes".PadRight(10) + "Date and Time\n";
        private bool newFileBool = true;

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
        private async void EventHandler(Object sender, StockNotification sn)
        {
            try
            {
                Stock newStock = (Stock)sender;
                string statement;

                //=== write to console the stock notification info ===
                statement = BrokerName + " " + sn.StockName + " " + sn.CurrentValue + " " + sn.NumChanges;
                Console.WriteLine(statement);

                //=== write to file the stock notification info ===
                myLock.EnterWriteLock();
                WriteStockToFile(sn);
                myLock.ExitWriteLock();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error caught: {0}", e.Message);
            }
        }

        private void WriteStockToFile(StockNotification sn)
        {
            //if the file exists, overwrites the file and adds the titles to the top of the file
            if (newFileBool)
            {
                using (StreamWriter writer = new StreamWriter(docPath))
                {
                    writer.WriteLine(titles);
                }
                newFileBool = false;
            }

            using (StreamWriter writer = new StreamWriter(docPath, true))
            {
                DateTime now = DateTime.Now;
                string time = now.ToString("F"); // "F" specifier creates returns a full date and time string
                //save the following information to a file when the stock's threshold is reached: date and time, stock name, inital value and current value
                string fileData = BrokerName.PadRight(10) + sn.StockName.PadRight(15) + sn.CurrentValue.ToString().PadRight(10) + sn.NumChanges.ToString().PadRight(10) + time;

                writer.WriteLine(fileData);
            }
        }
    }
}