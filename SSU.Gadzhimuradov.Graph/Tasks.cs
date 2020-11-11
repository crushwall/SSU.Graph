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
            Dictionary<int, Stack<int>> w;
            g.Eccentricity(out w);

            foreach (var item in w)
            {
                Console.Write($"{item.Key} ");

                foreach (var u in item.Value)
                {
                    Console.Write($"{u} ");
                }
                Console.WriteLine();
            }

            Console.WriteLine();

        }
        public static void ShowGraph()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Входной граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();
        }

        public static void A1Ex16()
        {
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

        public static void A2Ex22()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№22.");
            Console.ForegroundColor = ConsoleColor.Gray;

            int removable;
            List<int> r = new List<int>();

            while (g.IsCyclic(out removable))
            {
                r.Add(removable);
                g.RemoveVertex(removable);
            }

            g.Show();

            Console.Write($"\nБыло удалено ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(r.Count);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($" вершин.\n{(r.Count <= 1 ? "Можно" : "Нельзя")} удалить одну вершину, чтобы получилось дерево.\n");
        }

        public static void A2Ex28()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№28. Введитe веришину:");
            Console.ForegroundColor = ConsoleColor.Gray;

            int v = int.Parse(Console.ReadLine());

            Dictionary<int, int> parents;
            Dictionary<int, int> d = g.BFS(v, out parents);

            var ways = Graph<int>.GetWaysToAll(parents);

            foreach (var u in ways)
            {
                Console.Write($"Путь до {u.Key} [{d[u.Key]}]: ");
                Console.ForegroundColor = ConsoleColor.Cyan;
                foreach (var p in u.Value)
                {
                    Console.Write($"{p} ");
                }
                Console.ForegroundColor = ConsoleColor.Gray;

                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void A3Boruvka()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nАлгоритм Борувки.");
            Console.ForegroundColor = ConsoleColor.Gray;

            Graph<int> tree = g.Boruvka();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nМинимальное остовное дерево:");
            Console.ForegroundColor = ConsoleColor.Gray;

            tree.Show();
        }

        public static void A4Ex11()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№11.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Радиус графа: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(g.Radius());
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nДиаметр графа: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(g.Diam());
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("Центральные вершины графа: ");
            Console.ForegroundColor = ConsoleColor.Cyan;

            foreach (var c in g.Centr())
            {
                Console.Write($"{c} ");
            }
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine();
        }
    }
}
