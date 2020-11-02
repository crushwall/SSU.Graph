using Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSU.Gadzhimuradov
{
    static class Tasks
    {
        static Graph<int> g = new Graph<int>("../../input2.txt");

        public static void Test()
        {
            g.Show();
            Console.WriteLine();

            Dictionary<int, int> parents = new Dictionary<int, int>();

            Dictionary<int, double> d = g.Dijkstra(1, out parents);

            foreach (var v in d)
            {
                Console.WriteLine($"\t{{{v.Key}, {v.Value}}}");
            }

            Console.WriteLine("Parents:");
            foreach (var v in parents)
            {
                Console.WriteLine($"\t[{v.Key}, {v.Value}]");
            }

            var way = Graph<int>.GetWayTo(parents, 5);

            foreach (var w in way)
            {
                Console.Write($"{w} ");
            }
        }

        public static void A1Ex16()
        {
            g.Show();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№16. Введите веришины:");
            Console.ForegroundColor = ConsoleColor.Gray;
            string[] temp = Console.ReadLine().Split(' ');
            int v = int.Parse(temp[0]);
            int u = int.Parse(temp[1]);

            Console.Write($"Все соседи для вершин {v} и {u}: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var n in g.GetAllNeighbors(v, u))
            {
                Console.Write($"{n} ");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void A1Ex17()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№17. Введите веришины:");
            Console.ForegroundColor = ConsoleColor.Gray;

            string[] temp = Console.ReadLine().Split(' ');
            int v = int.Parse(temp[0]);
            int u = int.Parse(temp[1]);

            Dictionary<int, int> parents = new Dictionary<int, int>();
            Dictionary<int, double> d = g.Dijkstra(v, out parents);

            var way = Graph<int>.GetWayTo(parents, u);

            Console.Write($"Из {v} {(way.Count == 3 ? "можно" : "нельзя")} попасть в {u} через одну вершину.\nПуть {v} -> {u}: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            foreach (var w in way)
            {
                Console.Write($"{w} ");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void B1Ex10()
        {
            var g2 = new Graph<int>("../../input2.txt");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№10.\nПервый граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g2.Show();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nВторой граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();

            var gd = g2.SymmetricDifference(g);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nСимметрическая разность:");
            Console.ForegroundColor = ConsoleColor.Gray;
            gd.Show();
        }

        public static void A2Ex28()
        {

        }
    }
}
