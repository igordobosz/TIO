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
        public int ProgressCount { get; set; }

        public override string ToString()
        {
            return $"Strategy: {Strategy.ToString()} Fitness: {BestAnt.Fitness} Progress: {ProgressCount}";
        }
    }
    public static class Algorithm
    {

        public static Result Solve(this AntColony colony, Problem problem)
        {
            problem.SetupPheromones();
            Random rand = new Random();
            var result = new Result()
            {
                BestAnt = new Ant(),
                ProgressCount = 0,
                Strategy = colony.Strategy
            };
            for(int i = 0; i < colony.Generations; i++)
            {
                var ants = new List<Ant>();
                for (int j = 0; j < colony.AntCount; j++)
                {
                    var ant = new Ant(problem.Cities[rand.Next(problem.Cities.Count)]);
                    ant.Setup(problem);
                    ants.Add(ant);
                }

                foreach (Ant ant in ants)
                {
                    for (int j = 0; j < problem.Cities.Count-1; j++)
                    {
                        ant.SelectNext(colony, problem);
                    }
                    ant.CalculateFitness(problem, false);
                    if (ant.Fitness < result.BestAnt.Fitness)
                    {
                        result.BestAnt = ant;
                        result.ProgressCount++;
                    }
                    ant.UpdatePheromoneDelta(colony, problem);
                }
                problem.UpdatePheromones(ants, colony);
            }
            return result;
        }

        public static void UpdatePheromones(this Problem problem, List<Ant> ants, AntColony colony)
        {
            var matrixSize = problem.CostMatrixEtas.GetLength(0);
            for (int i = 0; i < matrixSize; i++)
            {
                for (int j = 0; j < matrixSize; j++)
                {
                    problem.Pheromones[i, j] *= colony.Rho;
                    foreach (Ant ant in ants)
                    {
                        problem.Pheromones[i, j] += ant.PheromoneDelta[i, j];
                    }
                }
            }
        }

        public static void SelectNext(this Ant ant, AntColony colony, Problem problem)
        {
            var denominator = 0.0;
            // foreach(int city in ant.AllowedCities)
            // {
            //     var cityPheromones = problem.Pheromones[ant.CurrentCity, city];
            //     var eta = problem.CostMatrixEtas[ant.CurrentCity, city];
            //     denominator += Math.Pow(cityPheromones, colony.Alpha) * Math.Pow(eta, colony.Beta);
            // }
            denominator = 1;

            var probabilities = new Dictionary<int, double>();
            foreach(int city in ant.AllowedCities)
            {
                var cityPheromones = problem.Pheromones[ant.CurrentCity, city];
                var eta = problem.CostMatrixEtas[ant.CurrentCity, city];
                var value = Math.Pow(cityPheromones, colony.Alpha) * Math.Pow(eta, colony.Beta) / denominator;
                probabilities.Add(city, value);
            }
            var selectedCity = ant.AllowedCities.FirstOrDefault(e => e == SelectionRoulette(probabilities));
            ant.AllowedCities.Remove(selectedCity);
            ant.VisitedCities.Add(selectedCity);
        }

        private static int SelectionRoulette(Dictionary<int, double> probabilities)
        {
            Random rand = new Random();
            // var sumProp = probabilities.Sum(e => e.Value);
            // var percentages = probabilities.Select(e => new
            //     {
            //         City = e.Key,
            //         Value = e.Value / sumProp
            //     }
            // ).OrderByDescending(e => e.Value);
            // var random = rand.NextDouble();
            // var tmpSum = 0.0;
            // foreach (var percentage in percentages)
            // {
            //     tmpSum += percentage.Value;
            //     if (random < tmpSum)
            //     {
            //         selectedCity = percentage.City;
            //         break;
            //     }
            // }

            var random = rand.NextDouble();
            foreach (var keyValuePair in probabilities)
            {
                random -= keyValuePair.Value;
                if (random <= 0)
                    return keyValuePair.Key;
            }

            return 0;
        }

        public static void UpdatePheromoneDelta(this Ant ant, AntColony colony, Problem problem)
        {
            ant.PheromoneDelta = new double[problem.Cities.Count, problem.Cities.Count];
            for (int i = 0; i<ant.VisitedCities.Count-1; i++)
            {
                var currCity = ant.VisitedCities[i];
                var nextCity = ant.VisitedCities[i + 1];
                switch (colony.Strategy)
                {
                    case AntStrategy.AntQuality:
                        ant.PheromoneDelta[currCity, nextCity] = colony.Q;
                        break;
                    case AntStrategy.AntDensity:
                        ant.PheromoneDelta[currCity, nextCity] = colony.Q / problem.CostMatrix[currCity, nextCity];
                        break;
                    case AntStrategy.AntCycle:
                        ant.PheromoneDelta[currCity, nextCity] = colony.Q / ant.Fitness;
                        break;
                }
            }
        }

        public static void Setup(this Ant ant, Problem problem)
        {
            ant.AllowedCities = problem.Cities.Select(e => e.Id).ToList();
            ant.AllowedCities.Remove(ant.CurrentCity);
        }

        public static void CalculateFitness(this Ant ant, Problem problem, bool useItems = true)
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
                var nextCityId = i + 1 < citiesLen ? i + 1 : 0;
                travel += useItems ? Math.Ceiling(problem.CostMatrix[city, nextCityId] / velocity) : problem.CostMatrix[city, nextCityId]; 
            }

            ant.Fitness = useItems ? profit-travel : travel;
        }
    }

}
