using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halado_algoritmusok_feleves_feladatok.SmallestBoundaryPolygonProblem
{
    class Point
    {
        public double x { get; set; }
        public double y{ get; set; }

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class SmallestBoundaryPolygonProblem
    {
        public List<Point> Points{ get; set; }

        public SmallestBoundaryPolygonProblem()
        {
            Points = new List<Point>();
        }

        public void loadTownsFromFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            foreach (var line in lines)
            {
                var split = line.Split("\t");
                Points.Add(new Point(double.Parse(split[0]), double.Parse(split[1])));
            }
        }

        public void savePointsToFile(string filename, List<Point> points)
        {
            using (var writer = new StreamWriter(filename))
            {
                foreach (var point in points)
                {
                    writer.WriteLine("{0}\t{1}", point.x, point.y);
                }
            }
        }

        double distanceFromLine(Point lp1, Point lp2, Point p)
        {
            return ((lp2.y - lp1.y) * p.x - (lp2.x - lp1.x) * p.y + lp2.x * lp1.y - lp2.y * lp1.x) / Math.Sqrt(Math.Pow(lp2.y - lp1.y, 2) + Math.Pow(lp2.x - lp1.x, 2));
        }

        double outerDistanceToBoundary(List<Point> solution)
        {
            double sum_min_distances = 0;

            for (int pi = 0; pi < Points.Count(); pi++)
            {
                double min_dist = Double.PositiveInfinity;
                for (int li = 0; li < solution.Count(); li++)
                {
                    double act_dist = distanceFromLine(solution[li], solution[(li + 1) % solution.Count()], Points[pi]);
                    if (li == 0 || act_dist < min_dist)
                        min_dist = act_dist;
                }
                if (min_dist < 0)
                    sum_min_distances += -min_dist;
            }
            return sum_min_distances;
        }

        double lengthOfBoundary(List<Point> solution)
        {
            double sum_length = 0;

            for (int li = 0; li < solution.Count() - 1; li++)
            {
                Point p1 = solution[li];
                Point p2 = solution[(li + 1) % solution.Count()];
                sum_length += Math.Sqrt(Math.Pow(p1.x - p2.x, 2) + Math.Pow(p1.y - p2.y, 2));
            }
            return sum_length;
        }

        double objective(List<Point> solution)
        {
            return lengthOfBoundary(solution);
        }

        double constraint(List<Point> solution)
        {
            return -outerDistanceToBoundary(solution);
        }

        public List<Point> generateRandomPolygon(int sizeOfPolygon)
        {
            List<Point> polygon = new List<Point>();

            int xMin = (int)Math.Floor(Points.Min(p => p.x));
            int xMax = (int)Math.Ceiling(Points.Max(p => p.x));
            int yMin = (int)Math.Floor(Points.Min(p => p.y));
            int yMax = (int)Math.Ceiling(Points.Max(p => p.y));

            for (int i = 0; i < sizeOfPolygon; i++)
            {
                polygon.Add(new Point(Util.rnd.Next(xMin / 2, xMax * 2), Util.rnd.Next(yMin / 2, yMax * 2)));
            }

            var sorted = sortPointsClockwise(polygon);
            return sorted;
        }

        public List<Point> sortPointsClockwise(List<Point> points)
        {
            double centerX = points.Average(p => p.x);
            double centerY = points.Average(p => p.y);
            Point center = new Point(centerX, centerY);

            List<Point> sortedPoints = points.OrderBy(p => Math.Atan2(p.y - center.y, p.x- center.x)).ToList();

            return sortedPoints;
        }

        public List<Point> getRandomNeighbour(List<Point> polygon, int distance)
        {
            List<Point> neighbours = new List<Point>(polygon);
            
            neighbours.ForEach(n =>
            {
                n.x += Util.rnd.Next(-distance, distance + 1);
                n.y += Util.rnd.Next(-distance, distance + 1);
            });

            return sortPointsClockwise(neighbours);
        }

        public List<Point> hillClimbing(int sizeOfPolygon, int stopCondition)
        {
            var p = generateRandomPolygon(sizeOfPolygon);

            int idx = 0;
            Console.WriteLine("{0}. p: {1}", idx, objective(p));
            while (idx < stopCondition || constraint(p) < 0)
            {
                var q = getRandomNeighbour(p, 5);
                if (objective(q) <= objective(p))
                {
                    Console.WriteLine("{0}. Better solution found: p: {1}, q: {2}", idx, objective(p), objective(q));
                    p = q;
                }
                else
                    Console.WriteLine("{0}. p: {1}", idx, objective(q));
                idx++;
            }
            return p;
        }
    }
}
