using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIO_ZAD3
{
    public static class Helpers
    {
        public static Random Rand = new Random();

        public static double Distance(this City city, City anotherCity)
        {
            return Math.Sqrt(Math.Pow(city.CordX - anotherCity.CordX, 2) + Math.Pow(city.CordY - anotherCity.CordY, 2));
        }

        public static IEnumerable<Edge> EdgeCumulativeSum(IEnumerable<Edge> sequence)
        {
            double sum = 0;
            foreach (var item in sequence)
            {
                sum += item.Weight;
                item.Weight = sum;
            }

            return sequence;
        }

        public static City GetRandomEdge(IEnumerable<Edge> cumSum)
        {
            var random = Rand.NextDouble();
            return cumSum.First(j => j.Weight >= random).End;
        }
    }
}
