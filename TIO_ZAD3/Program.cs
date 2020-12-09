using System;
using System.Globalization;
using System.Threading;

namespace TIO_ZAD3
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            var problem = DataLoader.LoadProblem("easy_0");
            problem.CalculateCostMatrix();

            Console.ReadLine();
        }
    }
}
