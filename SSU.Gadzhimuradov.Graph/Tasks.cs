using Graph;
using Visualization;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace SSU.Gadzhimuradov
{
    static class Tasks
    {
        static Graph<int> g = new Graph<int>("../../input4.txt");

        public static void Serialize()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nПример сериализации:");
            Console.ForegroundColor = ConsoleColor.Gray;

            string json = JsonConvert.SerializeObject(g);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nJSON: ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(json);
            using (StreamWriter writer = new StreamWriter("graph.json"))
            {
                writer.WriteLine(json);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nГраф сериализован в файл: graph.json");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            BinaryFormatter binary = new BinaryFormatter();

            using (FileStream file = new FileStream("graph.bin", FileMode.OpenOrCreate))
            {
                binary.Serialize(file, g);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nГраф сериализован в файл: graph.bin");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nДесериализация:");
            Console.ForegroundColor = ConsoleColor.Gray;
            using (StreamReader reader = new StreamReader("graph.json"))
            {
                Graph<int> g1 = JsonConvert.DeserializeObject<Graph<int>>(reader.ReadToEnd());
                g1.AddVertex(70);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nВходной граф:");
                Console.ForegroundColor = ConsoleColor.Gray;
                g.Show();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nДесиарилизованный граф с добавленной вершиной:");
                Console.ForegroundColor = ConsoleColor.Gray;
                g1.Show();
            }

            using (FileStream file = new FileStream("graph.bin", FileMode.OpenOrCreate))
            {
                Graph<int> g1 = (Graph<int>)binary.Deserialize(file);
                g1.AddVertex(15);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nВходной граф:");
                Console.ForegroundColor = ConsoleColor.Gray;
                g.Show();

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nДесиарилизованный граф с добавленной вершиной:");
                Console.ForegroundColor = ConsoleColor.Gray;
                g1.Show();
            }
        }

        public static void ShowGraph(string title)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Gray;
            g.Show();
        }

        public static void ShowGraph<T> (string title, Graph<T> graph) where T : IEquatable<T>, IComparable<T>
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.Gray;
            graph.Show();
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

        public static void B4Ex14()
        {
            Graph<string> g1 = new Graph<string>("../../input3.txt");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№14.");
            ShowGraph<string>("Входной граф", g1);

            Dictionary<string, string> parents;
            Stack<string> cycle;
            var bf = g1.BellmanFord("a", out parents, out cycle);

            if (bf.Count > 0)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("\nКратчайшие пути из a:");
                Console.ForegroundColor = ConsoleColor.Gray;

                foreach (var way in Graph<string>.GetWaysToAll(parents))
                {
                    Console.Write("Путь до ");
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(way.Key);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(": ");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    foreach (var w in way.Value)
                    {
                        Console.Write($"{w} ");
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;

                    Console.Write("Вес пути: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"{bf[way.Key]}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\nГраф содержит отрицательный цикл: ");
                Console.ForegroundColor = ConsoleColor.Gray;

                foreach (var c in cycle)
                {
                    Console.Write($"{c} ");
                }
            }
        }

        public static void C4Ex17()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n№17.");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Kратчайшие пути для всех пар вершин:");
            Console.ForegroundColor = ConsoleColor.Gray;

            var parents = new Dictionary<int, Dictionary<int, int>>();
            var fw = g.FloydWarshall(out parents);

            if (fw.Count > 0)
            {
                foreach (var p in parents)
                {
                    Console.Write("\nПуть до всех вершин от вершины ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($"{p.Key}");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($": ");

                    foreach (var way in Graph<int>.GetWaysToAll(p.Value))
                    {
                        Console.Write("\tПуть до ");
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.Write(way.Key);
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(": ");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        foreach (var w in way.Value)
                        {
                            Console.Write($"{w} ");
                        }
                        Console.ForegroundColor = ConsoleColor.Gray;

                        Console.Write("Вес пути: ");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine($"{fw[p.Key][way.Key]}");
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nГраф содержит отрицательный цикл.");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static void MaximumFlow()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nПоиск максимального потока.");
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.Write("Введите исток и сток: ");
            string[] temp = Console.ReadLine().Split(' ');
            int source = int.Parse(temp[0]);
            int sink = int.Parse(temp[1]);

            Dictionary<int, Dictionary<int, double>> flowGraph;
            double f = g.Dinic(source, sink, out flowGraph);

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\nГраф с потоками:");
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (var v in flowGraph)
            {
                Console.Write($"{v.Key} | ");
                foreach (var u in v.Value)
                {
                    Console.Write($"{{{u.Key}, ");
                    Console.ForegroundColor = ConsoleColor.Blue;

                    if (u.Value == g.GraphCollection[v.Key][u.Key])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write(u.Value);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" / ");
                    Console.ForegroundColor = ConsoleColor.Green;

                    if (u.Value == g.GraphCollection[v.Key][u.Key])
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write(g.GraphCollection[v.Key][u.Key]);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("} ");
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nМаксимальный поток графа = ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(f);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
