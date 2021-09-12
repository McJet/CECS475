using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Stock
{
    public class Stock
    {
        public event EventHandler<StockNotification> OnStockEvent;

        private readonly Thread _thread;

        public string StockName { get; set; }
        public int InitialValue { get; set; }
        public int CurrentValue { get; set; }
        public int MaxChange { get; set; }
        public int Threshold { get; set; }
        public int NumChanges { get; set; }

        /// <summary>
        /// Stock class that contains all the information and changes of the stock
        /// </summary>
        /// <param name="name">Stock name</param>
        /// <param name="startingValue">Starting stock value</param>
        /// <param name="maxChange">The max value change of the stock</param>
        /// <param name="threshold">The range for the stock</param>
        public Stock(string name, int startingValue, int maxChange, int threshold)
        {
            this.StockName = name;
            this.InitialValue = startingValue;
            this.CurrentValue = startingValue;
            this.MaxChange = maxChange;
            this.Threshold = threshold;
            this.NumChanges = 0;
            // When a stock object is created, a thread is started.
            ThreadStart stockThread = new ThreadStart(Activate);
            _thread = new Thread(stockThread);
            _thread.Start();
        }
        /// <summary>
        /// Activates the threads synchronizations
        /// </summary>
        public void Activate()
        {
            for (int i = 0; i < 25; i++)
            {
                // This thread causes the stock's value to be modified every 500 milliseconds.
                Thread.Sleep(500); // 1/2 second
                // Call the function ChangeStockValue
                ChangeStockValue();
            }
        }
        /// <summary>
        /// Changes the stock value and also raising the event of stock value changes
        /// </summary>
        public void ChangeStockValue()
        {
            var rand = new Random();
            CurrentValue += rand.Next(-1 * MaxChange, MaxChange);
            this.NumChanges++;
            // If its value changes from its initial value by
            // more than the specified notification threshold, an event method is invoked. 
            if (Math.Abs((CurrentValue - InitialValue)) > Threshold)
            {
                // This invokes the stockEvent (of event-type StockNotification) and multicasts a
                // notification to all listeners who have registered with stockEvent.
                OnStockEvent?.Invoke(this, new StockNotification(this.StockName, this.CurrentValue, this.NumChanges)); 
            }
        }
    }
}