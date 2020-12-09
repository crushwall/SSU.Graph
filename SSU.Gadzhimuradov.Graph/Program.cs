using System;

namespace SSU.Gadzhimuradov.Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            //Tasks.ShowGraph("Входной граф");

            //Tasks.A1Ex16();
            //Tasks.A1Ex17();
            //Tasks.B1Ex10();

            //Tasks.A2Ex22();
            //Tasks.A2Ex28();

            //Tasks.A3Boruvka();

            //Tasks.A4Ex11();
            //Tasks.B4Ex14();
            //Tasks.C4Ex17();

            //Tasks.MaximumFlow();

            Tasks.Bridges();

            Console.ReadKey(true);
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
