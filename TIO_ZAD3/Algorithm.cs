using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TIO_ZAD3
{
    public class Result
    {
        public Ant BestAnt { get; set; }
        public AntStrategy Strategy { get; set; }
        public List<Ant> Results { get; set; }

        public override string ToString()
        {
            return $"Strategy: {Strategy.ToString()} Fitness: {BestAnt.Fitness}";
        }
    }
    public static class Algorithm
    {
        public static Result Solve(this AntColony colony, Problem problem, AntStrategy strategy)
        {
            colony.Graph.SetupPheromones(colony.Parameters.InitalPheromoneValue);
            var result = new Result()
            {
                Strategy = strategy,
                Results = new List<Ant>()
            };
            for (int i = 0; i < colony.Parameters.Generations; i++)
            {
                var ants = new List<Ant>();

                for (int j = 0; j < colony.Parameters.AntCount; j++)
                {
                    var ant = new Ant(colony.Graph, colony.Parameters.Beta, colony.Parameters.Q);
                    ant.Init(colony.Graph.Cities[Helpers.Rand.Next(0, colony.Graph.Dimensions)]);
                    ants.Add(ant);
                }

                result.BestAnt ??= ants[0];
                for (int j = 0; j < colony.Graph.Dimensions; j++)
                {
                    foreach (var ant in ants)
                    {
                        var edge = ant.MoveNext();
                        double evaporate = (1 - colony.Parameters.LocalEvaporationRate);
                        colony.Graph.DeletePheromone(edge, evaporate);

                        double deposit = colony.Parameters.LocalEvaporationRate * colony.Parameters.InitalPheromoneValue;
                        colony.Graph.AddPheromone(edge, deposit);
                    }
                }

                ants.ForEach(a => a.CalculateFitness(problem, colony.Graph, strategy == AntStrategy.TTP));

                double deltaR = 1 / result.BestAnt.Fitness;
                foreach (Edge edge in result.BestAnt.Path)
                {
                    double evaporate = (1 - colony.Parameters.GlobalEvaporationRate);
                    colony.Graph.DeletePheromone(edge, evaporate);

                    double deposit = colony.Parameters.GlobalEvaporationRate * deltaR;
                    colony.Graph.AddPheromone(edge, deposit);
                }
                var bestIterationAnt = ants.OrderByDescending(x => x.Fitness).Last();
                if (bestIterationAnt.Compare(result.BestAnt, strategy))
                {
                    result.BestAnt = bestIterationAnt;
                    result.Results.Add(bestIterationAnt);
                    Console.WriteLine("Current Global Best: " + result.BestAnt.Fitness + " found in " + i + " iteration");
                }
            }

            return result;
        }

        public static void CalculateFitness(this Ant ant, Problem problem, Graph graph, bool useItems = false)
        {
            int profit = 0;
            int weight = 0;
            double travel = 0;
            var citiesLen = ant.VisitedCities.Count;
            for (int i = 0; i < citiesLen; i++)
            {
                var city = ant.VisitedCities[i];
                var cityItems = problem.Cities.FirstOrDefault(e => e.Id == city)?.Items;
                foreach (Item item in cityItems)
                {
                    if (weight + item.Weight <= problem.CapacityOfKnapsack)
                    {
                        profit += item.Profit;
                        weight += item.Weight;
                        ant.Items.Add(item);
                        break;
                    }
                }
                var velocity = problem.MaxSpeed - weight * (problem.MaxSpeed - problem.MinSpeed) / problem.CapacityOfKnapsack;
                var nextCityId = i + 1 < citiesLen ? ant.VisitedCities[i+1] : ant.VisitedCities[0];
                travel += useItems ? Math.Ceiling(graph.GetEdge(city, nextCityId).Length / velocity) : graph.GetEdge(city, nextCityId).Length;
            }

            ant.Fitness = useItems ? travel-profit : travel;
        }
    }

}
