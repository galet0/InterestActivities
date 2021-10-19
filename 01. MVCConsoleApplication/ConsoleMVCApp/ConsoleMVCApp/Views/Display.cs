using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMVCApp.Views
{
    class Display
    {
        public double Amount { get; set; }
        public double TipAmount { get; set; }
        public double Tip { get; set; }
        public double Total { get; set; }
        public Display()
        {
            this.GetValues();
        }

        public void ShowTotalBill()
        {
            Console.WriteLine($"Your tip is: {this.Tip}");
            Console.WriteLine($"Total bill: {this.Total}");
        }
        private void GetValues()
        {
            Console.WriteLine("Enter the amount of the meal: ");
            this.Amount = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter the percent you want to tip:");
            this.TipAmount = double.Parse(Console.ReadLine());
        }
    }
}
