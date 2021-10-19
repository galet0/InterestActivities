using ConsoleMVCApp.Models;
using ConsoleMVCApp.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMVCApp.Controllers
{
    class TipCalculatorController
    {
        public Display View { get; set; }
        public Tip Model { get; set; }
        public TipCalculatorController()
        {
            this.View = new Display();
            this.Model = new Tip(this.View.Amount, this.View.TipAmount);
            this.View.Tip = this.Model.CalculateTip();
            this.View.Total = this.Model.CalculateTotal();
            this.View.ShowTotalBill();
        }
    }
}
