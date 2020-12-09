using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;

namespace TIO_ZAD3
{
    public enum AntStrategy
    {
        AntCycle, AntQuality, AntDensity
    }

    public class AntColony
    {
        public int AntCount { get; set; }
        public int Generations { get; set; }
        public double Alpha { get; set; }
        public double Beta { get; set; }
        public double Rho { get; set; }
        public int Q { get; set; }
        public AntStrategy Strategy { get; set; }

        public AntColony(int antCount, int generations, double alpha, double beta, double rho, int q, AntStrategy strategy)
        {
            AntCount = antCount;
            Generations = generations;
            Alpha = alpha;
            Beta = beta;
            Rho = rho;
            Q = q;
            Strategy = strategy;
        }
    }

    public class Ant
    {
        public double Fitness { get; set; }
        public double[,] PheromoneDelta { get; set; }
        public List<City> VisitedCities { get; set; }
        public List<Item> Items { get; set; }

        public Ant()
        {
            VisitedCities = new List<City>();
            Items = new List<Item>();
            Fitness = 0;
        }

        public void CalculateFitness(Problem problem)
        {
            int profit = 0;
            int weight = 0;
            double travel = 0;
            var citiesLen = VisitedCities.Count;
            for (int i = 0; i < citiesLen; i++)
            {
                profit += Items[i].Profit;
                weight += Items[i].Weight;
                var velocity = problem.MaxSpeed - weight * (problem.MaxSpeed - problem.MinSpeed) / problem.CapacityOfKnapsack;
                var nextCityId = i + 1 < citiesLen ? i + i : 0;
                travel += Math.Ceiling(problem.CostMatrix[i,nextCityId]/velocity);
            }

            Fitness = profit - travel;
        }
    }
}
