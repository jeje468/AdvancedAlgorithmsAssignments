// See https://aka.ms/new-console-template for more information
using Halado_algoritmusok_feleves_feladatok.TravelingSalesman;

TravelingSalesmanProblem travelingSalesmanProblem = new TravelingSalesmanProblem();
travelingSalesmanProblem.loadTownsFromFile("Towns.txt");
//travelingSalesmanProblem.saveTownsToFile("Saved.txt", travelingSalesmanProblem.Towns);
var optimalRoute = travelingSalesmanProblem.GeneticAlgorithm(10000, 10);
foreach (var town in optimalRoute)
{
    Console.Write(town.name + ", ");
}
var x = 1;
