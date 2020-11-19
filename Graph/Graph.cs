﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Graph
{
    [Serializable]
    public class Graph<T> : ISerializable where T : IEquatable<T>, IComparable<T>
    {
        #region FIELDS

        private bool _directed = false;
        private bool _weighted = false;

        private Dictionary<T, Dictionary<T, double>> _graph = new Dictionary<T, Dictionary<T, double>>();

        #endregion

        #region PROPERTIES

        public bool Directed
        {
            get { return _directed; }

            set { _directed = value; }
        }

        public bool Weighted
        {
            get { return _weighted; }

            set { _weighted = value; }
        }

        public Dictionary<T, Dictionary<T, double>> GraphCollection
        {
            get { return _graph; }
        }

        #endregion

        #region PRIVATE_METHODS

        private static T ConvertFromString(string s)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFrom(s);
        }

        private void Check()
        {
            foreach (var v in _graph)
            {
                foreach (var adj in v.Value)
                {
                    _graph[adj.Key][v.Key] = adj.Value;
                }
            }
        }

        private void ChechExistence()
        {
            Dictionary<T, Dictionary<T, double>> temp = new Dictionary<T, Dictionary<T, double>>();

            foreach (var v in _graph)
            {
                Dictionary<T, double> adjTemp = new Dictionary<T, double>();

                foreach (var adj in v.Value)
                {
                    if (_graph.ContainsKey(adj.Key))
                    {
                        adjTemp.Add(adj.Key, adj.Value);
                    }
                }

                temp.Add(v.Key, adjTemp);
            }

            _graph = temp;
        }

        #endregion  

        #region CONSTRUCTORS

        public Graph() { }

        public Graph(bool directed, bool weighted)
        {
            _directed = directed;
            _weighted = weighted;
        }

        public Graph(string path) : this(path, separator: ' ', directed: false, weighted: false) { }

        public Graph(string path, bool directed) : this(path, separator: ' ', directed, weighted: false) { }

        public Graph(string path, char separator, bool weighted) : this(path, separator, directed: false, weighted) { }

        public Graph(string path, char separator, bool directed, bool weighted)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string[] temp = new string[1];

                temp[0] = reader.ReadLine();
                _directed = (temp[0].Contains(">")) ? true : _directed;
                _weighted = (temp[0].Contains("~")) ? true : _weighted;

                int optional = _weighted ? 2 : 1;

                char[] separators = new char[optional];
                separators[0] = separator;
                if (_weighted) { separators[1] = '~'; }

                while (!reader.EndOfStream)
                {
                    temp = reader.ReadLine().Split(separators);

                    T v = ConvertFromString(temp[0]);

                    Dictionary<T, double> adjacency = new Dictionary<T, double>();

                    for (int i = 1; i < temp.Length; i += optional)
                    {
                        adjacency.Add(ConvertFromString(temp[i]), (_weighted ? double.Parse(temp[i + 1]) : 1));
                    }

                    _graph.Add(v, adjacency);
                }
            }

            if (!_directed) { Check(); }
        }

        public Graph(Graph<T> g)
        {
            _weighted = g._weighted;
            _directed = g._directed;

            _graph = new Dictionary<T, Dictionary<T, double>>();

            foreach (var v in g._graph)
            {
                _graph.Add(v.Key, new Dictionary<T, double>(v.Value));
            }
        }

        public Graph(SerializationInfo info, StreamingContext context)
        {
            _directed = (bool)info.GetValue("Directed", typeof(bool));
            _weighted = (bool)info.GetValue("Weighted", typeof(bool));
            _graph = (Dictionary<T, Dictionary<T, double>>)info.GetValue("GraphCollection", typeof(Dictionary<T, Dictionary<T, double>>));
        }

        #endregion

        #region METHODS

        public void AddVertex()
        {
            Console.Write("Название или номер вершины: ");
            T v = ConvertFromString(Console.ReadLine());

            Dictionary<T, double> adjacency = new Dictionary<T, double>();
            Console.WriteLine("Список смежных вершин через пробел\nЕсли граф взвешенный, то с весом ребра до кажой вершины\nФормат:\n\tназвание~вес:");

            char[] separators = { ' ', '~' };
            string[] temp = Console.ReadLine().Split(separators);

            int optional = _weighted ? 2 : 1;

            for (int i = 0; i < temp.Length / optional; i += optional)
            {
                adjacency.Add(ConvertFromString(temp[i]), (_weighted ? double.Parse(temp[i + 1]) : 1));
            }

            _graph.Add(v, adjacency);
        }

        public void AddVertex(T data)
        {
            Dictionary<T, double> adjacency = new Dictionary<T, double>();

            _graph.Add(data, adjacency);
        }

        public void AddVertex(T data, Dictionary<T, double> adjacency)
        {
            Dictionary<T, double> adj = new Dictionary<T, double>();

            foreach (var a in adjacency)
            {
                adj.Add(a.Key, a.Value);
            }

            _graph.Add(data, adj);
        }

        public void RemoveVertex(T data)
        {
            if (_graph.ContainsKey(data))
            {
                foreach (var v in _graph)
                {
                    if (v.Value.ContainsKey(data))
                    {
                        v.Value.Remove(data);
                    }
                }

                _graph.Remove(data);
            }
            else
            {
                throw new Exception($"Vertex with data {data} do not exists.");
            }
        }

        public void AddEdge()
        {
            Console.WriteLine("Введите номер вершин и вес ребра, при необходимости\nФормат\n\tвершина вершина вес");

            string[] temp = Console.ReadLine().Split(' ');

            double weight = (temp.Length == 3) ? double.Parse(temp[2]) : 1;

            _graph[ConvertFromString(temp[0])].Add(ConvertFromString(temp[1]), weight);
            _graph[ConvertFromString(temp[1])].Add(ConvertFromString(temp[0]), weight);
        }

        public void AddEdge(T startVertex, T endVertex)
        {
            _graph[startVertex].Add(endVertex, 1);
            _graph[endVertex].Add(startVertex, 1);
        }

        public void AddEdge(T startVertex, T endVertex, double weight)
        {
            _graph[startVertex].Add(endVertex, weight);
            _graph[endVertex].Add(startVertex, weight);
        }

        public void RemoveEdge(T startVertex, T endVertex)
        {
            _graph[startVertex].Remove(endVertex);
            _graph[endVertex].Remove(startVertex);
        }

        public IEnumerable<T> GetAllNeighbors(T v, T u)
        {
            var adjV = _graph[v].Keys;
            var adjU = _graph[u].Keys;

            return adjV.Intersect(adjU);
        }

        public Dictionary<T, int> GetAllWaysFrom(T v)
        {
            Dictionary<T, int> allWaysFromV = new Dictionary<T, int>();

            allWaysFromV.Add(v, 0);
            GetAllWaysFrom(v, ref allWaysFromV, 1);

            return allWaysFromV;
        }

        private void GetAllWaysFrom(T v, ref Dictionary<T, int> ways, int currentLenght)
        {
            List<T> added = new List<T>();

            foreach (var adj in _graph[v])
            {
                if (!ways.ContainsKey(adj.Key))
                {
                    ways.Add(adj.Key, currentLenght);
                    added.Add(adj.Key);
                }
            }

            foreach (var adj in added)
            {
                GetAllWaysFrom(adj, ref ways, currentLenght + 1);
            }
        }

        #region OPERATIONS

        public Graph<T> Union(Graph<T> otherGraph)
        {
            Graph<T> union = new Graph<T>();

            union._directed = _directed;
            union._weighted = _weighted;

            union._graph = _graph.Union(otherGraph._graph).GroupBy(pair => pair.Key).
                ToDictionary(gr => gr.Key, gr => gr.SelectMany(pair => pair.Value).Distinct().ToDictionary(pair => pair.Key, pair => pair.Value));

            return union;
        }

        public Graph<T> Intersect(Graph<T> otherGraph)
        {
            Graph<T> intersect = new Graph<T>();

            intersect._directed = _directed;
            intersect._weighted = _weighted;

            intersect._graph = _graph.Where(pair => otherGraph._graph.ContainsKey(pair.Key))
                .ToDictionary(gr => gr.Key, gr => gr.Value);

            return intersect;
        }

        public Graph<T> Difference(Graph<T> otherGraph)
        {
            Graph<T> difference = new Graph<T>();

            difference._directed = _directed;
            difference._weighted = _weighted;

            difference._graph = _graph.Where(pair => !otherGraph._graph.ContainsKey(pair.Key))
                .ToDictionary(gr => gr.Key, gr => gr.Value);

            difference.ChechExistence();

            return difference;
        }

        public Graph<T> SymmetricDifference(Graph<T> otherGraph)
        {
            Graph<T> difference = new Graph<T>();

            difference._directed = _directed;
            difference._weighted = _weighted;

            Graph<T> temp = new Graph<T>();
            temp = Intersect(otherGraph);

            difference = Union(otherGraph);

            difference = difference.Difference(temp);

            return difference;
        }

        #endregion

        #region ALGORITHMS

        public Dictionary<T, int> BFS(T v)
        {
            Dictionary<T, T> parents;

            return BFS(v, out parents);
        }

        public Dictionary<T, int> BFS(T v, out Dictionary<T, T> parents)
        {
            Dictionary<T, int> d = new Dictionary<T, int>(_graph.Count);
            parents = new Dictionary<T, T>();

            Queue<T> queue = new Queue<T>();

            T tempV = v;
            d.Add(v, 0);
            queue.Enqueue(v);
            parents[v] = v;

            while (queue.Count != 0)
            {
                tempV = queue.Dequeue();

                foreach (var u in _graph[tempV])
                {
                    if (!d.ContainsKey(u.Key))
                    {
                        d.Add(u.Key, d[tempV] + 1);
                        parents.Add(u.Key, tempV);
                        queue.Enqueue(u.Key);
                    }
                }
            }

            return d;
        }

        public bool IsCyclic()
        {
            T cycle;
            return IsCyclic(out cycle);
        }

        public bool IsCyclic(out T cycle)
        {
            cycle = default(T);

            Stack<T> stack = new Stack<T>();
            List<T> used = new List<T>();

            T tempV = _graph.First().Key;
            stack.Push(tempV);  

            while (stack.Count != 0)
            {
                foreach (var u in _graph[tempV])
                {
                    if (!used.Contains(u.Key) && !stack.Contains(u.Key))
                    {
                        stack.Push(u.Key);
                    }
                    else if (!used.Contains(u.Key) && stack.Contains(u.Key))
                    {
                        cycle = _graph[tempV].Count >= _graph[u.Key].Count ? u.Key : tempV;
                        return true;
                    }
                }

                used.Add(tempV);
                tempV = stack.Pop();
            }
            
            return false;
        }

        private bool ContainsNegativeWeights()
        {
            foreach (var v in _graph)
            {
                var temp = v.Value.Where(pair => pair.Value < 0).ToList();
                if (temp.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<T, double> Dijkstra(T v)
        {
            Dictionary<T, T> parents;

            return Dijkstra(v, out parents);
        }

        public Dictionary<T, double> Dijkstra(T v, out Dictionary<T, T> parents)
        {
            if (ContainsNegativeWeights())
            {
                return BellmanFord(v, out parents);
            }

            Dictionary<T, double> d = new Dictionary<T, double>(_graph.Count);
            parents = new Dictionary<T, T>();

            var comparer = Comparer<KeyValuePair<double, T>>.Create((x, y) => x.Key > y.Key ? 1 : x.Key < y.Key ? -1
            : x.Value.CompareTo(y.Value));
            SortedSet<KeyValuePair<double, T>> set = new SortedSet<KeyValuePair<double, T>>(comparer);

            T tempV = v;
            d.Add(v, 0);
            set.Add(new KeyValuePair<double, T>(0, v));
            parents.Add(v, v);

            while (set.Count != 0)
            {
                tempV = set.Min.Value;
                set.Remove(set.Min);

                foreach (var u in _graph[tempV])
                {
                    if (d.ContainsKey(u.Key) && (d[u.Key] > d[tempV] + u.Value))
                    {
                        KeyValuePair<double, T> temp;
                        set.TryGetValue(new KeyValuePair<double, T>(d[u.Key], u.Key), out temp);
                        set.Remove(temp);

                        d[u.Key] = d[tempV] + u.Value;
                        parents[u.Key] = tempV;
                        set.Add(new KeyValuePair<double, T>(d[u.Key], u.Key));
                    }
                    else if (!d.ContainsKey(u.Key))
                    {
                        d.Add(u.Key, d[tempV] + u.Value);
                        parents.Add(u.Key, tempV);
                        set.Add(new KeyValuePair<double, T>(d[u.Key], u.Key));
                    }
                }
            }

            return d;
        }

        public Dictionary<T, double> BellmanFord(T v)
        {
            Dictionary<T, T> parents;
            Stack<T> cycle;

            return BellmanFord(v, out parents, out cycle);
        }

        public Dictionary<T, double> BellmanFord(T v, out Dictionary<T, T> parents)
        {
            Stack<T> cycle;

            return BellmanFord(v, out parents, out cycle);
        }

        public Dictionary<T, double> BellmanFord(T v, out Stack<T> cycle)
        {
            Dictionary<T, T> parents;

            return BellmanFord(v, out parents, out cycle);
        }

        public Dictionary<T, double> BellmanFord(T v, out Dictionary<T, T> parents, out Stack<T> cycle)
        {
            Dictionary<T, double> d = new Dictionary<T, double>(_graph.Count);
            parents = new Dictionary<T, T>();
            cycle = new Stack<T>();

            d.Add(v, 0);
            parents.Add(v, v);

            bool cycleFlag = false;
            T c = default(T);
            for (int i = 0; i < _graph.Count; i++)
            {
                cycleFlag = false;
                foreach (var u in _graph)
                {
                    if (!d.ContainsKey(u.Key))
                    {
                        d.Add(u.Key, double.MaxValue);
                    }

                    foreach (var adj in u.Value)
                    {
                        if (d.ContainsKey(adj.Key) && (d[adj.Key] > d[u.Key] + adj.Value))
                        {
                            d[adj.Key] = d[u.Key] + adj.Value;
                            parents[adj.Key] = u.Key;
                            cycleFlag = true;
                            c = adj.Key;
                        }
                        else if (!d.ContainsKey(adj.Key))
                        {
                            d.Add(adj.Key, d[u.Key] + adj.Value);
                            parents.Add(adj.Key, u.Key);
                            c = adj.Key;
                        }
                    }
                }
            }

            if (cycleFlag)
            {
                for (int i = 0; i < _graph.Count; i++)
                {
                    c = parents[c];
                }

                cycle.Push(c);
                T y = parents[c];
                while (!y.Equals(c))
                {
                    cycle.Push(y);
                    y = parents[y];
                }

                d.Clear();
                parents.Clear();
            }
            
            return d;
        }

        public Dictionary<T, Dictionary<T, double>> FloydWarshall()
        {
            Dictionary<T, Dictionary<T, T>> parents;

            return FloydWarshall(out parents);
        }

        public Dictionary<T, Dictionary<T, double>> FloydWarshall(out Dictionary<T, Dictionary<T,T>> parents)
        {
            Dictionary<T, Dictionary<T, double>> d = new Dictionary<T, Dictionary<T, double>>();
            parents = new Dictionary<T, Dictionary<T, T>>();

            if (ContainsNegativeWeights())
            {
                return d;
            }

            Dictionary<T, uint> vertexEnum = new Dictionary<T, uint>(_graph.Count);
            double[,] matrix = new double[_graph.Count, _graph.Count];
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    matrix[i, j] = double.MaxValue;
                }
            }

            foreach (var v in _graph)
            {
                if (!vertexEnum.ContainsKey(v.Key))
                {
                    vertexEnum.Add(v.Key, (uint)vertexEnum.Count);
                }

                matrix[vertexEnum[v.Key], vertexEnum[v.Key]] = 0;
                foreach (var adj in v.Value)
                {
                    if (!vertexEnum.ContainsKey(adj.Key))
                    {
                        vertexEnum.Add(adj.Key, (uint)vertexEnum.Count);
                    }

                    matrix[vertexEnum[v.Key], vertexEnum[adj.Key]] = adj.Value;

                    if (!parents.ContainsKey(v.Key))
                    {
                        Dictionary<T, T> temp = new Dictionary<T, T>();
                        temp.Add(v.Key, v.Key);
                        temp.Add(adj.Key, v.Key);
                        parents.Add(v.Key, temp);

                        Dictionary<T, double> tempD = new Dictionary<T, double>();
                        tempD.Add(v.Key, 0);
                        tempD.Add(adj.Key, adj.Value);
                        d.Add(v.Key, tempD);
                    }
                    else
                    {
                        parents[v.Key].Add(adj.Key, v.Key);

                        d[v.Key].Add(adj.Key, adj.Value);
                    }
                }
            }

            for (int k = 0; k < matrix.GetLength(0); k++)
            {
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    for (int j = 0; j < matrix.GetLength(0); j++)
                    {
                        if (matrix[i, k] < double.MaxValue && matrix[k, j] < double.MaxValue
                            && matrix[i, j] > matrix[i, k] + matrix[k, j])
                        {
                            matrix[i, j] = matrix[i, k] + matrix[k, j];

                            if (!parents.ContainsKey(vertexEnum.ElementAt(i).Key))
                            {
                                parents.Add(vertexEnum.ElementAt(i).Key, new Dictionary<T, T>());

                                d.Add(vertexEnum.ElementAt(i).Key, new Dictionary<T, double>());
                                d[vertexEnum.ElementAt(i).Key].Add(vertexEnum.ElementAt(j).Key, matrix[i, j]);
                            }
                            else
                            {
                                if (!parents[vertexEnum.ElementAt(i).Key].ContainsKey(vertexEnum.ElementAt(j).Key))
                                {
                                    parents[vertexEnum.ElementAt(i).Key].Add(vertexEnum.ElementAt(j).Key, vertexEnum.ElementAt(k).Key);

                                    d[vertexEnum.ElementAt(i).Key].Add(vertexEnum.ElementAt(j).Key, matrix[i, j]);

                                }
                                else
                                {
                                    parents[vertexEnum.ElementAt(i).Key][vertexEnum.ElementAt(j).Key] = vertexEnum.ElementAt(k).Key;

                                    d[vertexEnum.ElementAt(i).Key][vertexEnum.ElementAt(j).Key] = matrix[i, j];

                                }
                            }
                        }
                    }
                }
            }

            return d;
        }

        public static Stack<T> GetWayTo(Dictionary<T, T> parents, T to)
        {
            Stack<T> way = new Stack<T>();

            T temp = to;
            if (parents.ContainsKey(temp))
            {
                while (!parents[temp].Equals(temp))
                {
                    way.Push(temp);
                    temp = parents[temp];
                }
                way.Push(temp);
            }

            return way;
        }

        public static Dictionary<T, Stack<T>> GetWaysToAll(Dictionary<T, T> parents)
        {
            Dictionary<T, Stack<T>> allWays = new Dictionary<T, Stack<T>>();

            foreach (var p in parents)
            {
                Stack<T> way = new Stack<T>();

                T temp = p.Key;
                if (parents.ContainsKey(temp))
                {
                    while (!parents[temp].Equals(temp))
                    {
                        way.Push(temp);
                        temp = parents[temp];
                    }
                    way.Push(temp);
                }

                allWays.Add(p.Key, way);
            }

            return allWays;
        }

        public Graph<T> Boruvka()
        {
            Graph<T> tree = new Graph<T>(false, _weighted);
            Dictionary<List<T>, KeyValuePair<T, T>> components = new Dictionary<List<T>, KeyValuePair<T, T>>();
            Dictionary<List<T>, KeyValuePair<T, T>> componentsTemp = new Dictionary<List<T>, KeyValuePair<T, T>>();

            foreach (var v in _graph)
            {
                tree._graph.Add(v.Key, new Dictionary<T, double>());

                List<T> temp = new List<T>();
                temp.Add(v.Key);
                T to = v.Value.OrderBy(pair => pair.Value).FirstOrDefault().Key;
                components.Add(temp, new KeyValuePair<T, T>(v.Key, to));
                componentsTemp.Add(temp, new KeyValuePair<T, T>(v.Key, to));
            }

            int k = 0;
            while (k < _graph.Count - 1)
            {
                foreach (var comp in components)
                {
                    if (!tree._graph[comp.Value.Key].ContainsKey(comp.Value.Value))
                    {
                        tree._graph[comp.Value.Key].Add(comp.Value.Value, _graph[comp.Value.Key][comp.Value.Value]);
                        tree._graph[comp.Value.Value].Add(comp.Value.Key, _graph[comp.Value.Key][comp.Value.Value]);
                        k++;
                    }

                    var fromComp = componentsTemp.Keys.Where(l => l.Contains(comp.Value.Key)).FirstOrDefault();
                    //componentsTemp.Remove(fromComp);
                    var toComp = componentsTemp.Keys.Where(l => l.Contains(comp.Value.Value)).FirstOrDefault();
                    fromComp = fromComp.Union(toComp).ToList();
                    componentsTemp.Remove(toComp);

                    T from = default(T);
                    T to = default(T);
                    double minWeight = double.MaxValue;
                    foreach (var v in fromComp)
                    {
                        foreach (var adj in _graph[v])
                        {
                            if (adj.Value < minWeight && !fromComp.Contains(adj.Key))
                            {
                                minWeight = adj.Value;

                                from = v;
                                to = adj.Key;
                            }
                        }
                    }


                    componentsTemp[fromComp] = new KeyValuePair<T, T>(from, to);
                    //componentsTemp.Add(fromComp, new KeyValuePair<T, T>(from, to));
                }

                components = new Dictionary<List<T>, KeyValuePair<T, T>>(componentsTemp);
            }

            return tree;
        }

        public Dictionary<T, double> Eccentricity()
        {
            Dictionary<T, double> eccentricity = new Dictionary<T, double>();

            Dictionary<T, T> parents = new Dictionary<T, T>();
            foreach (var v in _graph)
            {
                var max = Dijkstra(v.Key, out parents).OrderByDescending(pair => pair.Value).FirstOrDefault();
                eccentricity.Add(v.Key, max.Value);
            }

            return eccentricity;
        }

        public Dictionary<T, double> Eccentricity(out Dictionary<T, Stack<T>> ways)
        {
            Dictionary<T, double> eccentricity = new Dictionary<T, double>();
            ways = new Dictionary<T, Stack<T>>();

            Dictionary<T, T> parents = new Dictionary<T, T>();
            foreach (var v in _graph)
            {
                var max = Dijkstra(v.Key, out parents).OrderByDescending(pair => pair.Value).FirstOrDefault();
                eccentricity.Add(v.Key, max.Value);

                ways.Add(v.Key, GetWayTo(parents, max.Key));
            }

            return eccentricity;
        }

        public double Radius()
        {
            Dictionary<T, Stack<T>> ways;
            return  Eccentricity().OrderBy(pair => pair.Value).FirstOrDefault().Value;
        }

        public double Diam()
        {
            return Eccentricity().OrderByDescending(pair => pair.Value).FirstOrDefault().Value;
        }

        public IEnumerable<T> Centr()
        {
            List<T> centr = new List<T>();

            Dictionary<T, double> eccentricity = new Dictionary<T, double>();

            foreach (var v in _graph)
            {
                var min = Dijkstra(v.Key).OrderByDescending(pair => pair.Value).FirstOrDefault();
                eccentricity.Add(v.Key, min.Value);
            }

            var radius = Eccentricity().OrderBy(pair => pair.Value).FirstOrDefault().Value;

            foreach (var v in eccentricity)
            {
                if (v.Value == radius)
                {
                    centr.Add(v.Key);
                }
            }

            return centr;
        }

        public int MaximumFlow(T source, T sink)
        {
           return  Dinic(source, sink);
        }

        public int Dinic(T source, T sink)
        {
            int maxFlow = 0;

            Dictionary<T, int> flow = new Dictionary<T, int>();



            return maxFlow;

        }

        #endregion

        public void Show()
        {
            Console.WriteLine((_directed ? "" : "не ") + "ориентированный | " + (_weighted ? "" : "не ") + "взвешенный\n");

            if (_graph.Count > 0)
            {
                foreach (var vs in _graph)
                {
                    Console.Write($"{vs.Key} | ");
                    foreach (var v in vs.Value)
                    {
                        if (_weighted)
                        {
                            Console.Write($"{{{v.Key}, {v.Value}}} ");
                        }
                        else
                        {
                            Console.Write($"{{{v.Key}}} ");
                        }
                    }

                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Граф не содержит элементов.");
            }
        }

        public void ToFile(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.Write(this);
            }
        }

        public override string ToString()
        {
            string strGraph = "" + (_directed ? ">" : "") + (_weighted ? "~\n" : "\n");

            int i = 0;

            foreach (var v in _graph)
            {
                strGraph += $"{v}";

                if (v.Value.Count > 0)
                {
                    strGraph += " ";

                    foreach (var adj in v.Value)
                    {
                        i++;

                        if (i < v.Value.Count)
                        {
                            if (_weighted)
                            {
                                strGraph += $"{adj.Key}~{adj.Value} ";
                            }
                            else
                            {
                                strGraph += $"{adj.Key} ";
                            }
                        }
                        else
                        {
                            if (_weighted)
                            {
                                strGraph += $"{adj.Key}~{adj.Value}\n";
                            }
                            else
                            {
                                strGraph += $"{adj.Key}\n";
                            }
                        }
                    }
                }
                else
                {
                    strGraph += "\n";
                }

                i = 0;
            }

            return strGraph;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Directed", _directed, typeof(bool));
            info.AddValue("Weighted", _weighted, typeof(bool));
            info.AddValue("GraphCollection", _graph, typeof(Dictionary<T, Dictionary<T, double>>));
        }

        #endregion
    }
}