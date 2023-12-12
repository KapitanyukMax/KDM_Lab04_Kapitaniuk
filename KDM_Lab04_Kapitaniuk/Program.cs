using Graphs;
using Functions;
using BoolFunctions;
using BinarySequences;

namespace KDM_Lab04_Kapitaniuk
{
    public class Program
    {
        public static void Task1(Function function)
        {
            Console.WriteLine("Task 1");
            Console.Write("Function pair list: ");
            foreach (var pair in function.Graph.Edges)
                Console.Write($"({pair.From},{pair.To}) ");
            Console.WriteLine();

            if (function.IsInjective())
                Console.WriteLine("Function is injective");
            else
            {
                Console.WriteLine("Function is not injective");
                Console.Write("Injective fixed function pair list: ");
                var injectiveFunction = function.MakeInjective();
                foreach (var pair in injectiveFunction.Graph.Edges)
                    Console.Write($"({pair.From},{pair.To}) ");
                Console.WriteLine();
            }

            if (function.IsSurjective())
                Console.WriteLine("Function is surjective");
            else
            {
                Console.WriteLine("Function is not surjective");
                Console.Write("Set, where function is surjective: { ");
                Console.Write(string.Join(", ", function.GetValuesSet()));
                Console.WriteLine(" }");
            }

            Console.WriteLine($"Function is {(function.IsBijective() ? "" : "not ")}bijective");

            Console.WriteLine();
        }

        public static void Task3(BoolFunction boolFunction)
        {
            Console.WriteLine("Task 3");

            Console.WriteLine("Bool function true values list: { "
                + string.Join(',', boolFunction.TrueValues) + " }");

            Console.WriteLine("Function PDNF: "
                + boolFunction.GetPerfectDisjunctiveNormalForm());

            List<bool> parameters = new();
            Console.WriteLine($"Enter {boolFunction.Arguments.Count} bool arguments:");
            for (int i = 0; i < boolFunction.Arguments.Count; i++)
                parameters.Add(Console.ReadLine() != "0");

            Console.WriteLine("Function result: "
                + (boolFunction.GetValue(parameters) ? "1" : "0"));

            Console.WriteLine();
        }

        public static void Task5(BoolFunction boolFunction)
        {
            Console.WriteLine("Task 5");

            Console.WriteLine("Bool function true values list: { "
                + string.Join(',', boolFunction.TrueValues) + " }");
            Console.WriteLine("Function reduced DNF: "
                + boolFunction.McCluskeyAlgorithm());

            Console.WriteLine();
        }

        public static void Task6(string sequence)
        {
            Console.WriteLine("Task 6");

            Console.Write("Hamming code sequence: ");
            Console.WriteLine(BinaryOperations.GetHammingCode(sequence));

            Console.WriteLine(BinaryOperations.GetInitialSequence(sequence));

            Console.WriteLine();
        }

        public static void Main()
        {
            List<Edge> pairList = new()
            {
                new("1", "2"),
                new("2", "3"),
                new("3", "1"),
                new("4", "5"),
                new("5", "5")
            };
            List<string> functionSet = new() { "1", "2", "3", "4", "5" };
            Function function = new(pairList, functionSet);

            List<string> arguments = new() { "x", "y", "z", "w" };
            List<int> trueValues = new() { 0, 1, 4, 5, 8, 9, 11, 12, 15 };
            //List<int> trueValues = new() { 2, 4, 5, 7, 8, 9, 10, 11, 12, 13, 15 };
            BoolFunction boolFunction = new(arguments, trueValues);

            string sequence = "101010101010";

            Task1(function);
            Task3(boolFunction);
            Task5(boolFunction);
            Task6(sequence);
        }
    }
}
