using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<int> VisitedCities { get; set; }
        public List<Item> Items { get; set; }
        public int CurrentCity => VisitedCities.Last();

        public List<int> AllowedCities { get; set; }

        public Ant(City startCity = null)
        {
            VisitedCities = new List<int>();
            Items = new List<Item>();
            Fitness = double.MaxValue;
            if (startCity != null)
            {
                VisitedCities.Add(startCity.Id);
            }
        }
    }
}
