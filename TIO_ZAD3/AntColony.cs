using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace TIO_ZAD3
{
    public class Parameters
    {
        public int AntCount { get; set; }
        public int Generations { get; set; }
        public double LocalEvaporationRate { get; set; }
        public double Beta { get; set; }
        public double GlobalEvaporationRate { get; set; }
        public double Q { get; set; }
        public double InitalPheromoneValue { get; set; }
        public AntStrategy Strategy { get; set; }

        public Parameters()
        {
            Beta = 2;
            GlobalEvaporationRate = 0.1;
            LocalEvaporationRate = 0.01;
            Q = 0.9;
            AntCount = 20;
            Generations = 100000;
            InitalPheromoneValue = 0.01;
        }
    }
    public enum AntStrategy
    {
        TSP, TTP
    }

    public class AntColony
    {
        public Parameters Parameters { get; set; }
        public Graph Graph { get; set; }

        public AntColony(Parameters parameters, Graph graph)
        {
            Parameters = parameters;
            graph.MinimumPheromone = parameters.InitalPheromoneValue;
            Graph = graph;
        }
    }

    public class Ant
    {
        public Graph Graph { get; set; }
        public double Beta { get; set; }
        public double Q { get; set; }
        public double Fitness { get; set; }
        public List<int> VisitedCities { get; set; }
        public List<int> AllowedCities { get; set; }
        public List<Item> Items { get; set; }
        public List<Edge> Path { get; set; }

        public Ant(double fitness)
        {
            Fitness = fitness;
        }

        public Ant(Graph graph, double beta, double q)
        {
            Graph = graph;
            Beta = beta;
            Q = q;
            VisitedCities = new List<int>();
            AllowedCities = new List<int>();
            Items = new List<Item>();
            Path = new List<Edge>();
        }

        public void Init(City startCity)
        {
            Fitness = 0;
            VisitedCities.Add(startCity.Id);
            AllowedCities = Graph.Cities.Select(e => e.Id).ToList();
            AllowedCities.Remove(startCity.Id);
            Path.Clear();
        }

        public int CurrentCity() => VisitedCities.Last();

        public Edge MoveNext()
        {
            int endPoint;
            var startPoint = CurrentCity();

            if (AllowedCities.Count == 0)
            {
                endPoint = VisitedCities[0];
            }
            else
            {
                endPoint = ChooseNextCity();
                VisitedCities.Add(endPoint);
                AllowedCities.Remove(endPoint);
            }

            var edge = Graph.GetEdge(startPoint, endPoint);
            Path.Add(edge);
            return edge;
        }

        private int ChooseNextCity()
        {
            List<Edge> edgesWithWeight = new List<Edge>();
            Edge bestEdge = new Edge();

            foreach (var node in AllowedCities)
            {
                var edge = Graph.GetEdge(CurrentCity(), node);
                edge.Weight = edge.Pheromone * Math.Pow(1 / edge.Length, Beta);

                if (edge.Weight > bestEdge.Weight)
                {
                    bestEdge = edge;
                }

                edgesWithWeight.Add(edge);
            }

            var random = Helpers.Rand.NextDouble();
            if (random < Q)
            {
                return bestEdge.End.Id;
            }
            else
            {
                double totalSum = edgesWithWeight.Sum(x => x.Weight);
                var edgeProbabilities = edgesWithWeight.Select(w => { w.Weight = (w.Weight / totalSum); return w; }).ToList();
                var cumSum = Helpers.EdgeCumulativeSum(edgeProbabilities);
                City choosenPoint = Helpers.GetRandomEdge(cumSum);

                return choosenPoint.Id;
            }
        }

        public bool Compare(Ant ant, AntStrategy strategy)
        {
            if (strategy == AntStrategy.TSP)
            {
                return Math.Round(Fitness, 3) < Math.Round(ant.Fitness, 3);
            }
            else
            {
                return Math.Round(Fitness, 3) < Math.Round(ant.Fitness, 3);
            }
        }
    }
}
