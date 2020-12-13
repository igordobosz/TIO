using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TIO_ZAD3
{
    public class Graph
    {
        public List<City> Cities { get; set; }
        public Dictionary<int,Edge> Edges { get; set; }
        public int Dimensions { get; set; }
        public double MinimumPheromone { get; set; }

        public Graph(List<City> cities)
        {
            Edges = new Dictionary<int, Edge>();
            this.Cities = cities;
            Dimensions = cities.Count;
            CreateEdges();
        }

        private void CreateEdges()
        {
            for (int i = 0; i < Cities.Count; i++)
            {
                var city1 = Cities[i];
                for (int j = 0; j < Cities.Count; j++)
                {
                    if (i != j)
                    {
                        var city2 = Cities[j];
                        Edges.Add(HashEdge(city1.Id, city2.Id), new Edge(city1, city2, city2.Distance(city1)));
                    }
                }
            }
        }

        public Edge GetEdge(int city1, int city2)
        {
            return Edges[HashEdge(city1, city2)];
        }

        private int HashEdge(int city1, int city2)
        {
            return city1 * 1000000 + city2;
        }

        public void SetupPheromones(double pheromoneValue)
        {
            foreach (var edge in Edges)
            {
                edge.Value.Pheromone = pheromoneValue;
            }
        }

        public void DeletePheromone(Edge edge, double value)
        {
            var secondEdge = GetEdge(edge.End.Id, edge.Start.Id);
            edge.Pheromone = Math.Max(MinimumPheromone, edge.Pheromone * value);
            secondEdge.Pheromone = Math.Max(MinimumPheromone, secondEdge.Pheromone * value);
        }

        public void AddPheromone(Edge edge, double value)
        {
            var secondEdge = GetEdge(edge.End.Id, edge.Start.Id);
            edge.Pheromone += value;
            secondEdge.Pheromone += value;
        }
    }

    public class Edge
    {
        public City Start { get; set; }
        public City End { get; set; }
        public double Length { get; set; }
        public double Pheromone { get; set; }
        public double Weight { get; set; }

        public Edge() { }

        public Edge(City start, City end, double distance)
        {
            Start = start;
            End = end;
            Length = distance;
        }
    }
}
