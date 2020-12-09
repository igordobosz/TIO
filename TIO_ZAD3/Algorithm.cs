using System;
using System.Collections.Generic;
using System.Text;

namespace TIO_ZAD3
{
    public static class Algorithm
    {

        public static Ant Solve(this AntColony colony, Problem problem)
        {
            Ant bestAnt = new Ant();
            double[,] pheromones = new double[problem.Cities.Count,problem.Cities.Count];
            for(int i = 0; i < colony.Generations; i++)
            {
                var ants = new List<Ant>();
                for (int j = 0; j < colony.AntCount; j++)
                {
                    ants.Add(new Ant());
                }

                foreach (Ant ant in ants)
                {
                    for (int j = 0; j < problem.Cities.Count; j++)
                    {
                        ant.SelectNext();
                    }
                    ant.CalculateFitness(problem);
                    if (ant.Fitness > bestAnt.Fitness)
                        bestAnt = ant;
                    ant.UpdatePheromoneDelta();
                }
                UpdatePheromones(out pheromones, ants);
            }
            return bestAnt;
        }

        public static void UpdatePheromones(out double[,] pheromones, List<Ant> ants)
        {

        }

        public static void SelectNext(this Ant ant, )
        {
            var denominator = 0;
            
        }

        public static void UpdatePheromoneDelta(this Ant ant)
        {

        }
    }

}
