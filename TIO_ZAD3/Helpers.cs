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
            var resultEtas = new double[problem.Cities.Count, problem.Cities.Count];
            var pheromones = new double[problem.Cities.Count, problem.Cities.Count];
            for (int i = 0; i < problem.Cities.Count; i++)
            {
                for (int j = 0; j < problem.Cities.Count; j++)
                {
                    var distance = problem.Cities[i].Distance(problem.Cities[j]);
                    var eta = distance == 0 ? 0 : 1 / distance;
                    result[i, j] = distance;
                    resultEtas[i, j] = eta;
                }
            }
            problem.CostMatrixEtas = resultEtas;
            problem.CostMatrix = result;
        }

        public static void SetupPheromones(this Problem problem)
        {
            var pheromones = new double[problem.Cities.Count, problem.Cities.Count];
            for (int i = 0; i < problem.Cities.Count; i++)
            {
                for (int j = 0; j < problem.Cities.Count; j++)
                {
                    pheromones[i, j] = 1/Math.Pow(problem.Cities.Count, 2);
                }
            }
            problem.Pheromones = pheromones;
        }
    }
}
