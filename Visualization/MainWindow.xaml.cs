using Graph;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Visualization
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    enum Action
    {
        Default,
        Add,
        ConnectStart,
        ConnectEnd,
        Delete
    }

    public partial class MainWindow : Window
    {
        Brush vertexBrush = new SolidColorBrush(Color.FromRgb(255, 170, 45));
        Brush strokeBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        Brush textBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));

        Brush dinicLevelBrush = new SolidColorBrush(Color.FromRgb(255, 55, 45));
        Brush dinicFlowBrush = new SolidColorBrush(Color.FromRgb(255, 55, 45));

        private const int strokeThickness = 2;
        private const int vertexDiam = 50;
        private const int lineCount = 6;
        private const int columnCount = 9;

        Graph<int> graph = new Graph<int>();
        Dictionary<int, KeyValuePair<Ellipse, Point>> vertexes = new Dictionary<int, KeyValuePair<Ellipse, Point>>();
        Dictionary<KeyValuePair<int, int>, Line> edges = new Dictionary<KeyValuePair<int, int>, Line>();

        Action action;

        Point tempPos;
        int tempV;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myDialog = new OpenFileDialog();
            myDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            myDialog.CheckFileExists = true;
            myDialog.Multiselect = true;
            if (myDialog.ShowDialog() == true)
            {
                graph = new Graph<int>(myDialog.FileName);
                vertexes.Clear();
                edges.Clear();

                double step = vertexDiam * 3;
                double x = vertexDiam * 0.5;
                double y = vertexDiam * 0.5;

                int curLine = 1;
                int curColumn = 1;

                foreach (var v in graph.GraphCollection)
                {
                    vertexes.Add(v.Key, new KeyValuePair<Ellipse, Point>(new Ellipse
                    {
                        Width = vertexDiam,
                        Height = vertexDiam,
                        Fill = vertexBrush,
                        Stroke = strokeBrush,
                        StrokeThickness = strokeThickness,
                    }, new Point(x, y)));

                    if (curColumn < columnCount / 2)
                    {
                        x += step;
                        y += curColumn % 2 == 1 ? vertexDiam * 0.75 : -(vertexDiam * 0.75);
                        curColumn++;
                    }
                    else
                    {
                        curColumn = 1;
                        curLine++;

                        x = vertexDiam * 0.5;
                        y += step;
                    }
                }

                foreach (var v in graph.GraphCollection)
                {
                    double correction = vertexDiam * 0.5; ;
                    foreach (var adj in v.Value)
                    {
                        if ((!graph.Directed && !edges.ContainsKey(new KeyValuePair<int, int>(adj.Key, v.Key)))
                            || graph.Directed)
                        {
                            edges.Add(new KeyValuePair<int, int>(v.Key, adj.Key), new Line
                            {
                                X1 = vertexes[v.Key].Value.X + correction,
                                Y1 = vertexes[v.Key].Value.Y + correction,
                                X2 = vertexes[adj.Key].Value.X + correction,
                                Y2 = vertexes[adj.Key].Value.Y + correction,
                                Stroke = strokeBrush,
                                StrokeThickness = strokeThickness,
                            });
                        }
                    }
                }
            }


            RenderGraph();
        }

        private void AddVertex(Point pos)
        {
            var tempVertexes = vertexes.OrderBy(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            int v;

            if (tempVertexes.LastOrDefault().Key + 1 != tempVertexes.Count)
            {
                v = tempVertexes.FirstOrDefault().Key;

                foreach (var item in tempVertexes)
                {
                    if (v + 1 <= item.Key)
                    {
                        break;
                    }

                    v++;
                }
            }
            else
            {
                v = tempVertexes.Count;
            }

            graph.AddVertex(v);

            vertexes.Add(v, new KeyValuePair<Ellipse, Point>(new Ellipse
            {
                Width = vertexDiam,
                Height = vertexDiam,
                Fill = vertexBrush,
                Stroke = strokeBrush,
                StrokeThickness = strokeThickness,
            }, pos));

            Canvas.SetLeft(vertexes[v].Key, pos.X);
            Canvas.SetTop(vertexes[v].Key, pos.Y);
            drawing.Children.Add(vertexes[v].Key);

            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 25;
            textBlock.Foreground = textBrush;
            textBlock.Text = v.ToString();
            Canvas.SetLeft(textBlock, pos.X + vertexDiam / 4);
            Canvas.SetTop(textBlock, pos.Y + vertexDiam / 4);
            drawing.Children.Add(textBlock);
        }

        private void ConnectVertex(int endV, Point endPos)
        {
            double weight = 0;
            EdgeWeightWindow edge = new EdgeWeightWindow();
            if (edge.ShowDialog() == true)
            {
                weight = edge.Weight;
            }

            graph.AddEdge(tempV, endV, weight);

            double correction = vertexDiam * 0.5; ;
            Line line = new Line();
            line.X1 = tempPos.X + correction;
            line.Y1 = tempPos.Y + correction;
            line.X2 = endPos.X + correction;
            line.Y2 = endPos.Y + correction;
            line.Stroke = strokeBrush;
            line.StrokeThickness = strokeThickness;

            edges.Add(new KeyValuePair<int, int>(tempV, endV), line);
            drawing.Children.Add(edges[new KeyValuePair<int, int>(tempV, endV)]);

            TextBlock textBlock = new TextBlock();
            textBlock.FontSize = 18;
            textBlock.Foreground = vertexBrush;
            textBlock.Background = textBrush;
            textBlock.Text = weight.ToString();
            Canvas.SetLeft(textBlock, (line.X1 + line.X2) * 0.5);
            Canvas.SetTop(textBlock, (line.Y1 + line.Y2) * 0.5 - 15);
            drawing.Children.Add(textBlock);

            drawing.Children.Remove(vertexes[tempV].Key);
            drawing.Children.Remove(vertexes[endV].Key);
            drawing.Children.Add(vertexes[tempV].Key);
            drawing.Children.Add(vertexes[endV].Key);

            textBlock = new TextBlock();
            textBlock.FontSize = 25;
            textBlock.Foreground = textBrush;
            textBlock.Text = tempV.ToString();
            Canvas.SetLeft(textBlock, tempPos.X + vertexDiam / 4);
            Canvas.SetTop(textBlock, tempPos.Y + vertexDiam / 4);
            drawing.Children.Add(textBlock);

            textBlock = new TextBlock();
            textBlock.FontSize = 25;
            textBlock.Foreground = textBrush;
            textBlock.Text = endV.ToString();
            Canvas.SetLeft(textBlock, endPos.X + vertexDiam / 4);
            Canvas.SetTop(textBlock, endPos.Y + vertexDiam / 4);
            drawing.Children.Add(textBlock);
        }

        private void DeleteVertex(int v, Point vPos)
        {
            vertexes.Remove(v);
            graph.RemoveVertex(v);
            var newEdges = new Dictionary<KeyValuePair<int, int>, Line>();
            foreach (var e in edges)
            {
                if (e.Key.Key != v && e.Key.Value != v)
                {
                    newEdges.Add(e.Key, e.Value);
                }
            }
            edges = newEdges;

            ClearCanvas();
            RenderEdges();
            RenderVertexes();
        }

        private void DrawingAction(object sender, MouseButtonEventArgs e)
        {
            switch (action)
            {
                case Action.Add:
                    AddVertex(Mouse.GetPosition(drawing));
                    break;
                case Action.ConnectStart:
                    var temp = vertexes.Where(v => Math.Abs((v.Value.Value.X - Mouse.GetPosition(drawing).X)) <= vertexDiam
                    && Math.Abs((v.Value.Value.Y - Mouse.GetPosition(drawing).Y)) <= vertexDiam);
                    if (temp.Count() > 0)
                    {
                        action = Action.ConnectEnd;
                        tempV = temp.First().Key;
                        tempPos = temp.First().Value.Value;
                    }
                    break;
                case Action.ConnectEnd:
                    var tempEnd = vertexes.Where(v => Math.Abs((v.Value.Value.X - Mouse.GetPosition(drawing).X)) <= vertexDiam
                    && Math.Abs((v.Value.Value.Y - Mouse.GetPosition(drawing).Y)) <= vertexDiam);
                    if (tempEnd.Count() > 0)
                    {
                        action = Action.Default;
                        var endV = tempEnd.First().Key;
                        var endPos = tempEnd.First().Value.Value;

                        ConnectVertex(endV, endPos);
                    }
                    break;
                case Action.Delete:
                    temp = vertexes.Where(v => Math.Abs((v.Value.Value.X - Mouse.GetPosition(drawing).X)) <= vertexDiam
                    && Math.Abs((v.Value.Value.Y - Mouse.GetPosition(drawing).Y)) <= vertexDiam);
                    if (temp.Count() > 0)
                    {
                        tempV = temp.First().Key;
                        tempPos = temp.First().Value.Value;
                    }

                    DeleteVertex(tempV, tempPos);
                    break;
            }
        }

        private void RenderVertexes()
        {
            foreach (var v in vertexes)
            {
                Canvas.SetLeft(v.Value.Key, v.Value.Value.X);
                Canvas.SetTop(v.Value.Key, v.Value.Value.Y);
                drawing.Children.Add(v.Value.Key);

                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 25;
                textBlock.Foreground = textBrush;
                textBlock.Text = v.Key.ToString();
                Canvas.SetLeft(textBlock, v.Value.Value.X + vertexDiam / 4);
                Canvas.SetTop(textBlock, v.Value.Value.Y + vertexDiam / 4);
                drawing.Children.Add(textBlock);
            }
        }

        private void Arrow(double x1, double y1, double x2, double y2, Brush brush)
        {
            double d = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

            double X = x2 - x1;
            double Y = y2 - y1;

            double X3 = x2 - (X / d) * 55;
            double Y3 = y2 - (Y / d) * 55;

            double Xp = y2 - y1;
            double Yp = x1 - x2;

            double X4 = X3 + (Xp / d) * 5;
            double Y4 = Y3 + (Yp / d) * 5;
            double X5 = X3 - (Xp / d) * 5;
            double Y5 = Y3 - (Yp / d) * 5;

            var p = new Polygon();

            //Line line = new Line
            //{
            //    Stroke = brush,
            //    X1 = x1,
            //    Y1 = y1,
            //    X2 = x2,
            //    Y2 = y2
            //};
            p.Points.Add(new Point(x2, y2));

            //line = new Line
            //{
            //    Stroke = brush,
            //    X1 = x2 - (X / d) * 10,
            //    Y1 = y2 - (Y / d) * 10,
            //    X2 = X4,
            //    Y2 = Y4
            //};
            p.Points.Add(new Point(X4, Y4));


            //line = new Line
            //{
            //    Stroke = brush,
            //    X1 = x2 - (X / d) * 10,
            //    Y1 = y2 - (Y / d) * 10,
            //    X2 = X5,
            //    Y2 = Y5
            //};
            p.Points.Add(new Point(X5, Y5));
            p.Fill = brush;
            drawing.Children.Add(p);

        }

        private void RenderEdges()
        {
            foreach (var e in edges)
            {
                drawing.Children.Add(e.Value);

                TextBlock textBlock = new TextBlock();
                textBlock.FontSize = 18;
                textBlock.Foreground = vertexBrush;
                textBlock.Background = textBrush;
                textBlock.Text = graph.GraphCollection[e.Key.Key][e.Key.Value].ToString();
                Canvas.SetLeft(textBlock, (e.Value.X1 + e.Value.X2) * 0.5);
                Canvas.SetTop(textBlock, (e.Value.Y1 + e.Value.Y2) * 0.5 - 15);
                drawing.Children.Add(textBlock);

                if (graph.Directed)
                {
                    Arrow(e.Value.X1, e.Value.Y1, e.Value.X2, e.Value.Y2, strokeBrush);
                }
            }
        }

        private void RenderGraph()
        {
            ClearCanvas();

            RenderEdges();
            RenderVertexes();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            action = Action.Add;
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e)
        {
            action = Action.ConnectStart;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            action = Action.Delete;
        }

        private void ClearCanvas()
        {
            drawing.Children.Clear();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            graph = new Graph<int>();
            vertexes.Clear();
            edges.Clear();
            drawing.Children.Clear();
        }

        private void DinicBtn_Click(object sender, RoutedEventArgs e)
        {
            InitDinicWindow initDinic = new InitDinicWindow();

            int sourse = 0;
            int sink = 0;
            EdgeWeightWindow edge = new EdgeWeightWindow();
            if (initDinic.ShowDialog() == true)
            {
                sourse = initDinic.Sourse;
                sink = initDinic.Sink;
            }

            Dictionary<int, Dictionary<int, double>> flowGraph;
            var flow = Dinic(sourse, sink, out flowGraph, ref graph);
            var tempVertexes =  new Dictionary<int, KeyValuePair<Ellipse, Point>>(vertexes);
            var tempEdges = new Dictionary<KeyValuePair<int, int>, Line>(edges);

            vertexes.Clear();
            edges.Clear();

            double step = vertexDiam * 2;
            double x = vertexDiam * 0.5;
            double y = vertexDiam * 0.5;

            int curLine = 1;
            int curColumn = 1;
            foreach (var v in flowGraph)
            {
                vertexes.Add(v.Key, new KeyValuePair<Ellipse, Point>(new Ellipse
                {
                    Width = vertexDiam,
                    Height = vertexDiam,
                    Fill = vertexBrush,
                    Stroke = strokeBrush,
                    StrokeThickness = strokeThickness,
                }, new Point(x, y)));

                if (curColumn < columnCount / 2)
                {
                    x += step;
                    y += curColumn % 2 == 1 ? vertexDiam * 0.75 : -(vertexDiam * 0.75);
                    curColumn++;
                }
                else
                {
                    curColumn = 1;
                    curLine++;

                    x = vertexDiam * 0.5;
                    y += step;
                }
            }

            foreach (var v in flowGraph)
            {
                double correction = vertexDiam * 0.5; ;
                foreach (var adj in v.Value)
                {
                    if ((!graph.Directed && !edges.ContainsKey(new KeyValuePair<int, int>(adj.Key, v.Key)))
                        || graph.Directed)
                    {
                        edges.Add(new KeyValuePair<int, int>(v.Key, adj.Key), new Line
                        {
                            X1 = vertexes[v.Key].Value.X + correction,
                            Y1 = vertexes[v.Key].Value.Y + correction,
                            X2 = vertexes[adj.Key].Value.X + correction,
                            Y2 = vertexes[adj.Key].Value.Y + correction,
                            Stroke = strokeBrush,
                            StrokeThickness = strokeThickness,
                        });
                    }
                }
            }

            ClearCanvas();

            foreach (var ed in edges)
            {
                double tempWeight = flowGraph[ed.Key.Key][ed.Key.Value];

                if (!graph.Directed)
                {
                    tempWeight = Math.Max(tempWeight, flowGraph[ed.Key.Value][ed.Key.Key]);
                }

                if (tempWeight != 0)
                {
                    drawing.Children.Add(ed.Value);
                    TextBlock textBlock = new TextBlock();
                    textBlock.FontSize = 18;
                    textBlock.Foreground = vertexBrush;
                    textBlock.Background = textBrush;
                    textBlock.Text = $"{tempWeight} / {graph.GraphCollection[ed.Key.Key][ed.Key.Value]}";
                    Canvas.SetLeft(textBlock, (ed.Value.X1 + ed.Value.X2) * 0.5);
                    Canvas.SetTop(textBlock, (ed.Value.Y1 + ed.Value.Y2) * 0.5 - 15);
                    drawing.Children.Add(textBlock);

                    if (graph.Directed)
                    {
                        Arrow(ed.Value.X1, ed.Value.Y1, ed.Value.X2, ed.Value.Y2, strokeBrush);
                    }
                }
            }

            RenderVertexes();

            TextBlock textFlow = new TextBlock();
            textFlow.FontSize = 18;
            textFlow.Foreground = vertexBrush;
            textFlow.Background = textBrush;
            textFlow.Text = $"Максимальный поток из {sourse} в {sink}: {flow}";
            Canvas.SetLeft(textFlow, 650);
            Canvas.SetTop(textFlow, 720);
            drawing.Children.Add(textFlow);
        }

        private bool DinicBFS(int source, int sink,
            ref Dictionary<int, int> levels, ref Dictionary<int, Dictionary<int, double>> flowGraph, ref Graph<int> _graph)
        {
            ClearCanvas();

            levels.Clear();
            Queue<int> queue = new Queue<int>();

            int temp = source;
            levels.Add(source, 0);
            queue.Enqueue(source);

            while (queue.Count != 0)
            {
                temp = queue.Dequeue();

                foreach (var u in _graph.GraphCollection[temp])
                {
                    if (!levels.ContainsKey(u.Key) && flowGraph[temp][u.Key] < _graph.GraphCollection[temp][u.Key])
                    {
                        queue.Enqueue(u.Key);
                        levels.Add(u.Key, levels[temp] + 1);

                        TextBlock textBlock = new TextBlock();
                        textBlock.FontSize = 18;
                        textBlock.Foreground = dinicLevelBrush;
                        textBlock.Background = textBrush;
                        textBlock.Text = levels[u.Key].ToString();

                        KeyValuePair<int, int> tempE = new KeyValuePair<int, int>(temp, u.Key);
                        if (!edges.ContainsKey(tempE))
                        {
                            tempE = new KeyValuePair<int, int>(u.Key, temp);
                        }

                        Canvas.SetLeft(textBlock, (edges[tempE].X1 + edges[tempE].X2) * 0.5);
                        Canvas.SetTop(textBlock, (edges[tempE].Y1 + edges[tempE].Y2) * 0.5 - 15);
                        drawing.Children.Add(textBlock);

                        double correction = vertexDiam * 0.5; ;
                        Line line = new Line();

                        line.X1 = vertexes[temp].Value.X + correction;
                        line.Y1 = vertexes[temp].Value.Y + correction;
                        line.X2 = vertexes[u.Key].Value.X + correction;
                        line.Y2 = vertexes[u.Key].Value.Y + correction;
                        line.Stroke = dinicLevelBrush;
                        line.StrokeThickness = strokeThickness;

                        drawing.Children.Add(line);

                        if (graph.Directed)
                        {
                            Arrow(line.X1, line.Y1, line.X2, line.Y2, dinicLevelBrush);
                        }
                    }
                }
            }

            RenderVertexes();

            return levels.ContainsKey(sink);
        }

        private double DinicDFS(int sourse, int sink, double flow, ref Dictionary<int, int> ptr,
            ref Dictionary<int, int> levels, ref Dictionary<int, Dictionary<int, double>> flowGraph, ref Graph<int> _graph)
        {
            ClearCanvas();

            if (sourse.Equals(sink))
            {
                return flow;
            }

            int temp = sourse;
            foreach (var adj in _graph.GraphCollection[temp])
            {
                if (!ptr.ContainsKey(temp))
                {
                    ptr.Add(temp, 0);
                }
                ptr[temp]++;

                if (levels[adj.Key] == levels[temp] + 1
                    && flowGraph[temp][adj.Key] < _graph.GraphCollection[temp][adj.Key])
                {
                    double currFlow = Math.Min(flow, _graph.GraphCollection[temp][adj.Key] - flowGraph[temp][adj.Key]);

                    double tempFlow = DinicDFS(adj.Key, sink, currFlow, ref ptr, ref levels, ref flowGraph, ref _graph);

                    if (tempFlow > 0)
                    {
                        flowGraph[temp][adj.Key] += tempFlow;

                        TextBlock textBlock = new TextBlock();
                        textBlock.FontSize = 18;
                        textBlock.Foreground = dinicFlowBrush;
                        textBlock.Background = textBrush;
                        textBlock.Text = $"{flowGraph[temp][adj.Key]} / {_graph.GraphCollection[temp][adj.Key]}";

                        KeyValuePair<int, int> tempE = new KeyValuePair<int, int>(temp, adj.Key);
                        if (!edges.ContainsKey(tempE))
                        {
                            tempE = new KeyValuePair<int, int>(adj.Key, temp);
                        }

                        Canvas.SetLeft(textBlock, (edges[tempE].X1 + edges[tempE].X2) * 0.5);
                        Canvas.SetTop(textBlock, (edges[tempE].Y1 + edges[tempE].Y2) * 0.5 - 15);
                        drawing.Children.Add(textBlock);

                        double correction = vertexDiam * 0.5; ;
                        Line line = new Line();

                        line.X1 = vertexes[temp].Value.X + correction;
                        line.Y1 = vertexes[temp].Value.Y + correction;
                        line.X2 = vertexes[adj.Key].Value.X + correction;
                        line.Y2 = vertexes[adj.Key].Value.Y + correction;
                        line.Stroke = vertexBrush;
                        line.StrokeThickness = strokeThickness;

                        drawing.Children.Add(line);

                        if (graph.Directed)
                        {
                            Arrow(line.X1, line.Y1, line.X2, line.Y2, vertexBrush);
                        }

                        return tempFlow;
                    }
                }
            }
            return 0;
        }

        private double Dinic(int source, int sink, out Dictionary<int, Dictionary<int, double>> flowGraph,
            ref Graph<int> _graph)
        {
            string caption = "Пауза";
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxImage icon = MessageBoxImage.Question;
            
            flowGraph = new Dictionary<int, Dictionary<int, double>>();
            foreach (var v in _graph.GraphCollection)
            {
                var tempAdj = new Dictionary<int, double>();
                foreach (var adj in v.Value)
                {
                    tempAdj.Add(adj.Key, 0);
                }

                flowGraph.Add(v.Key, tempAdj);
            }

            double maxFlow = 0;
            var levels = new Dictionary<int, int>();
            var ptr = new Dictionary<int, int>();
            foreach (var v in _graph.GraphCollection)
            {
                ptr.Add(v.Key, 0);
            }

            while (DinicBFS(source, sink, ref levels, ref flowGraph, ref _graph))
            {
                MessageBox.Show("Продолжить?", caption, button, icon);
                double pushed = DinicDFS(source, sink, double.MaxValue, ref ptr, ref levels, ref flowGraph, ref _graph);
                RenderVertexes();

                MessageBox.Show("Продолжить?", caption, button, icon);

                while (pushed > 0)
                {
                    maxFlow += pushed;
                    pushed = DinicDFS(source, sink, double.MaxValue, ref ptr, ref levels, ref flowGraph, ref _graph);
                    RenderVertexes();
                }
            }

            return maxFlow;
        }
    }
}