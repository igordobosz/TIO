using System;
using System.Collections.Generic;
using System.Text;

namespace TIO_ZAD3
{
    public static class Helpers
    {
        public static double Distance(this City city, City anotherCity)
        {
            return Math.Sqrt(Math.Pow(city.CordX - anotherCity.CordX, 2) + Math.Pow(city.CordY - anotherCity.CordY, 2));
        }

        public static void CalculateCostMatrix(this Problem problem)
        {
            var result = new double[problem.Cities.Count,problem.Cities.Count];
            for (int i = 0; i < problem.Cities.Count; i++)
            {
                for (int j = 0; j < problem.Cities.Count; j++)
                {
                    result[i, j] = problem.Cities[i].Distance(problem.Cities[j]);
                }
            }

            problem.CostMatrix = result;
        }
    }
}
