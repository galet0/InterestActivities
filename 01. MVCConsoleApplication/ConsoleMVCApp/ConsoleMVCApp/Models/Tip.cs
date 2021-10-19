using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMVCApp.Models
{
    class Tip
    {
        private double percent;
        private double amount;

        public Tip(double amount, double percent)
        {
            this.Amount = amount;
            this.Percent = percent;
        }

        public double Amount 
        {
            get => this.amount;
            private set
            {
                if(value <= 0)
                {
                    throw new InvalidOperationException("Bill amount must be positive number!");
                }

                this.amount = value;
            }
        }
        public double Percent 
        {
            get => this.percent;
            private set 
            {
                if(value < 0)
                {
                    throw new InvalidOperationException("Percent must be positive number");
                }
                if (value > 1)
                {
                    value /= 100;
                }

                this.percent = value;
            }
        }
        public double Total { get; set; }

        public double CalculateTip()
        {
            return this.Amount * this.Percent;
        }
        public double CalculateTotal()
        {
            return this.Amount + this.CalculateTip();
        }

    }
}
