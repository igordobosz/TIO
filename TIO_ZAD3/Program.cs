using System;
using System.Globalization;
using System.IO;
using System.Text;
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
                InitalPheromoneValue = (1.0 / (graph.Dimensions * graph.Dimensions)),
                Generations = 500
            };
            var colony2 = new AntColony(parameters, graph);
            var result = colony2.Solve(problem, AntStrategy.TTP);
            var resultPath = @$".\Data\result.csv";
            var csv = new StringBuilder();
            csv.AppendLine("index, best_thief, avg, worst_thief");
            int index = 0;
            foreach (var resultLine in result.Results)
            {
                csv.AppendLine($"{index++}, {resultLine.Best}, {resultLine.Average}, {resultLine.Worst}");
            }
            File.WriteAllText(resultPath, csv.ToString());
        }
    }
}
