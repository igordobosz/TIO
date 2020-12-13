using System;
using System.Collections.Generic;
using System.Text;

namespace TIO_ZAD3
{
    public class Problem
    {
        public string Name { get; set; }
        public int Dimensions { get; set; }
        public int NumberOfItems { get; set; }
        public int CapacityOfKnapsack { get; set; }
        public double MinSpeed { get; set; }
        public double MaxSpeed { get; set; }
        public List<City> Cities { get; set; } = new List<City>();
        public double[,] CostMatrix { get; set; }
        public double [,] CostMatrixEtas { get; set; }
        public double[,] Pheromones { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public double CordX { get; set; }
        public double CordY { get; set; }
        public List<Item> Items {get; set; } = new List<Item>();
    }

    public class Item
    {
        public int Id { get; set; }
        public int Profit { get; set; }
        public int Weight { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
