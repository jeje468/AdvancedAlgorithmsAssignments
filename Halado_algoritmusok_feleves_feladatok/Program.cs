// See https://aka.ms/new-console-template for more information
using Halado_algoritmusok_feleves_feladatok.SmallestBoundaryPolygonProblem;
using Halado_algoritmusok_feleves_feladatok.TravelingSalesman;

TravelingSalesmanProblem travelingSalesmanProblem = new TravelingSalesmanProblem();
travelingSalesmanProblem.loadTownsFromFile("Towns.txt");
travelingSalesmanProblem.saveTownsToFile("Saved.txt", travelingSalesmanProblem.Towns);
var optimalRoute = travelingSalesmanProblem.GeneticAlgorithm(50000, 10);
foreach (var town in optimalRoute)
{
    Console.Write(town.name + ", ");
}

//SmallestBoundaryPolygonProblem smallestBoundaryPolygonProblem = new SmallestBoundaryPolygonProblem();
//smallestBoundaryPolygonProblem.loadTownsFromFile("Points.txt");
//smallestBoundaryPolygonProblem.hillClimbing(4, 10000);
//var x = 1;
