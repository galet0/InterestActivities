using ConsoleMVCApp.Controllers;
using System;

namespace ConsoleMVCApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    TipCalculatorController controller =
                    new TipCalculatorController();
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }
    }
}
