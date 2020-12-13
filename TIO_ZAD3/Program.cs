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
            var problem = DataLoader.LoadProblem("hard_0");
            Graph graph = new Graph(problem.Cities);
            Parameters parameters = new Parameters()  
            {
                InitalPheromoneValue = (1.0 / (graph.Dimensions * graph.Dimensions))
            };
            var colony2 = new AntColony(parameters, graph);
            Console.WriteLine(colony2.Solve(problem, AntStrategy.TTP));
        }
    }
}
