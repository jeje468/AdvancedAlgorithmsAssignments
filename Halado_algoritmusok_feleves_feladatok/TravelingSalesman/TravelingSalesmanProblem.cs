using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halado_algoritmusok_feleves_feladatok.TravelingSalesman
{
    static class Util
    {
        public static Random rnd = new Random();
    }

    class Town
    {
        public string name { get; set; }
        public double x{ get; set; }
        public double y { get; set; }

        public Town(string name, double x, double y)
        {
            this.name = name;
            this.x = x;
            this.y = y;
        }

        public Town(Town town)
        {
            this.name=town.name;
            this.x = town.x;
            this.y = town.y;
        }
    }

    class TravelingSalesmanProblem
    {
        public List<Town> Towns { get; set; }

        public TravelingSalesmanProblem()
        {
            Towns = new List<Town>();
        }
        public double objective(List<Town> solution)
        {
            double sumLength = 0;
            for (int i = 0; i < solution.Count() - 1; i++)
            {
                Town t1 = solution[i];
                Town t2 = solution[i + 1];
                sumLength += Math.Sqrt(Math.Pow(t1.x - t2.x, 2) + Math.Pow(t1.y - t2.y, 2));
            }

            return sumLength;
        }

        public void loadTownsFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            foreach (var line in lines)
            {
                var split = line.Split("\t");
                Towns.Add(new Town(split[0], double.Parse(split[1]), double.Parse(split[2])));
            }
        }

        public void saveTownsToFile(string filename, List<Town> towns)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var town in towns)
                {
                    writer.WriteLine("{0}\t{1}\t{2}", town.name, town.x, town.y);
                }
            }
        }

        public List<List<Town>> initializePopulation(int populationSize)
        {
            var population = new List<List<Town>>();

            for (int i = 0; i < populationSize; i++)
            {
                var shuffledTowns = Towns.OrderBy(t => Guid.NewGuid()).ToList();
                population.Add(shuffledTowns);
            }

            return population;
        }

        public List<double> evaluatePopulation(List<List<Town>> population)
        {
            List<double> result = new List<double>();
            population.ForEach(population => result.Add(objective(population)));

            return result;
        }

        public List<List<Town>> selection(List<List<Town>> population, List<double> evaluation, int matingPoolSize)
        {
            var sortedIndexes = evaluation.Select((x, i) => new KeyValuePair<double, int>(x, i))
                                       .OrderBy(x => x.Key)
                                       .Take(matingPoolSize)
                                       .Select(x => x.Value);

            var bestIndividuals = sortedIndexes.Select(i => population[i]).ToList();

            return bestIndividuals;
        }

        public static List<Town> Crossover(List<Town> parent1, List<Town> parent2)
        {
            int n = parent1.Count;
            int start = Util.rnd.Next(n);
            int end = Util.rnd.Next(start + 1, n);

            List<Town> child = Enumerable.Repeat<Town>(null, parent1.Count()).ToList();

            for (int i = start; i < end; i++)
            {
                child[i] = parent1[i];
            }

            int idx = 0;
            for (int i = 0; i < parent2.Count(); i++)
            {
                if (start <= idx && idx < end)
                    idx += end - start;

                if (idx > child.Count() - 1)
                    break;

                if (!child.Contains(parent2[i]))
                    child[idx++] = parent2[i];
            }
            return child;
        }

        public static void Mutate(List<Town> child, int times)
        {
            for (int i = 0; i < times; i++)
            {
                int idx1 = Util.rnd.Next(0, child.Count());
                int idx2 = Util.rnd.Next(0, child.Count());
                while (idx1 == idx2)
                {
                    idx2 = Util.rnd.Next(0, child.Count());
                }

                Swap(child, idx1, idx2);
            }
        }

        public static void Swap<Town>(List<Town> list, int index1, int index2)
        {
            Town temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
        }


        public List<Town> GeneticAlgorithm(int stopCondition, int matingPoolSize)
        {
            var population = initializePopulation(100);
            var evaluation = evaluatePopulation(population);
            var pBest = population[evaluation.IndexOf(evaluation.Min())];

            int idx = 0;
            Console.WriteLine("{0}. pBest: {1}", idx, objective(pBest));
            while (idx < stopCondition)
            {
                var parents = selection(population, evaluation, matingPoolSize);
                var parentsEval = evaluatePopulation(parents);
                List<List<Town>> children = new List<List<Town>>();
                while (children.Count() < population.Count() - matingPoolSize)
                {
                    var parent1 = population[Util.rnd.Next(0, parents.Count())];
                    var parent2 = population[Util.rnd.Next(0, parents.Count())];
                    while (parent1 == parent2)
                    {
                        parent2 = population[Util.rnd.Next(0, parents.Count())];
                    }
                    var child = Crossover(parent1, parent2);
                    Mutate(child, 1);
                    children.Add(child);
                }
                children.AddRange(parents);
                population = children;
                evaluation = evaluatePopulation(population);
                pBest = population[evaluation.IndexOf(evaluation.Min())];
                Console.WriteLine("{0}. pBest: {1}", idx, objective(pBest));
                idx++;
            }

            return pBest;
        }
    }
}
