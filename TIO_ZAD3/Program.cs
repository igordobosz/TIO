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
            // var colony = new AntColony(10, 1000, 1.0, 2, 0.2, 1, AntStrategy.AntDensity);
            var colony2 = new AntColony(10, 1000, 1.0, 2, 0.2, 1, AntStrategy.AntCycle);
            // var colony3 = new AntColony(10, 1000, 1.0, 2, 0.2, 1, AntStrategy.AntQuality);
            // Console.WriteLine(colony.Solve(problem));
            Console.WriteLine(colony2.Solve(problem));
            // Console.WriteLine(colony3.Solve(problem));
        }
    }
}
