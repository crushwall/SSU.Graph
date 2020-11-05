using Graph;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SSU.Gadzhimuradov
{
    static class Tasks
    {
        static Graph<int> g = new Graph<int>("../../input.txt");

        public static void Test()
        {
            var g1 = new Graph<int>(g);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nИзначальный граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nГраф копия:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.Show();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nДобавляем вершину 8.");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.AddVertex(8);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nИзначальный граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nГраф копия:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.Show();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nУдаляем вершину 1.");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.RemoveVertex(1);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nИзначальный граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nГраф копия:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.Show();

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nДобавлем ребро 2 -> 8.");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.AddEdge(2, 8);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nИзначальный граф:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nГраф копия:");
            Console.ForegroundColor = ConsoleColor.Gray;
            g1.Show();

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
            g.Show();

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
            g.Show();

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
            Console.Write($" вершин.\n{(r.Count <= 1 ? "Можно" : "Нельзя")} удалить одну вершину, чтобы получилось дерево.");
        }

        public static void A2Ex28()
        {
            g.Show();

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
            g.Show();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nАлгоритм Борувки.");
            Console.ForegroundColor = ConsoleColor.Gray;

            Graph<int> tree = g.Boruvka();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nМинимальное остовное дерево:");
            Console.ForegroundColor = ConsoleColor.Gray;

            tree.Show();
        }
    }
}
