using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Commander
{
    public struct Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return String.Format("[{0}] - [{1}]", X, Y);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 19;
            Console.BufferHeight = 19;
            Console.WindowWidth = 111;
            Console.BufferWidth = 111;
            Console.TreatControlCAsInput = true;
            Console.Clear();
            Console.CursorVisible = false;
            Manager manager = new Manager();
            manager.Start(@"C:\");

        }
    }
}
