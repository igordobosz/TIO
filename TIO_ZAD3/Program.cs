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
            var problem = DataLoader.LoadProblem("easy_0"); // get shortest tour using greedy algorithm
            Graph graph = new Graph(problem.Cities);
            Parameters parameters = new Parameters()  // Most parameters will be default. We only have to set InitalPheromoneValue (initial pheromone level)
            {
                InitalPheromoneValue = (1.0 / (graph.Dimensions * graph.Dimensions))
            };
            var colony2 = new AntColony(parameters, graph);
            Console.WriteLine(colony2.Solve(problem, AntStrategy.TTP));
            // Console.WriteLine(colony3.Solve(problem));
        }
    }
}
