using System;

namespace SSU.Gadzhimuradov.Graph
{
    class Program
    {
        static void Main(string[] args)
        {
            Tasks.Bridges();

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\nДля закрытия нажмите ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Q");
            Console.ForegroundColor = ConsoleColor.Gray;
            while (Console.ReadKey().Key != ConsoleKey.Q) { }
        }
    }
}
