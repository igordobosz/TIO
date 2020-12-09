using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TIO_ZAD3
{
    public static class DataLoader
    {
        public static Problem LoadProblem(string fileName)
        {
            List<string> lines = System.IO.File.ReadLines(@$".\Data\{fileName}.ttp").ToList();
            var problem = new Problem();
            int index = 1;
            problem.Name = fileName;
            problem.Dimensions = int.Parse(lines[++index].SplitByT()[1]);
            problem.NumberOfItems = int.Parse(lines[++index].SplitByT()[1]);
            problem.CapacityOfKnapsack = int.Parse(lines[++index].SplitByT()[1]);
            problem.MinSpeed = double.Parse(lines[++index].SplitByT()[1]);
            problem.MaxSpeed = double.Parse(lines[++index].SplitByT()[1]);

            var cities = new List<City>();
            index+=3;
            for (int i = 0; i < problem.Dimensions; i++)
            {
                var parameters = lines[++index].SplitByT();
                var city = new City()
                {
                    Id = int.Parse(parameters[0]),
                    CordX = double.Parse(parameters[1]),
                    CordY = double.Parse(parameters[2])
                };
                cities.Add(city);
            }
            index++;
            for (int i = 0; i < problem.NumberOfItems; i++)
            {
                var parameters = lines[++index].SplitByT();
                var item = new Item()
                {
                    Id = int.Parse(parameters[0]),
                    Profit = int.Parse(parameters[1]),
                    Weight = int.Parse(parameters[2]),
                    CityId = int.Parse(parameters[3])
                };
                var assignedCity = cities.FirstOrDefault(c => c.Id == item.Id);
                item.City = assignedCity;
                assignedCity.Items.Add(item);
            }

            problem.Cities = cities;

            return problem;
        }

        private static string[] SplitByT(this string val)
        {
            return val.Split("\t");
        }
    }
}
