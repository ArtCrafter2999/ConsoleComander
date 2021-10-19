using System;
using System.Collections.Generic;
using System.Text;

namespace MenuNS
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
    public class Menu
    {
        private int _sel = 0;
        public List<string> Arr { get; private set; } = new List<string>();
        private Coord _offset = new Coord { X = 0, Y = 0 };
        public Menu()
        {

        }
        public Menu(params object[] puncts)
        {
            ToList(puncts);
        }
        public Menu(int X, int Y)
        {
            SetOffset(X, Y);
        }
        public Menu(int X, int Y, params object[] puncts)
        {
            ToList(puncts);
            SetOffset(X, Y);
        }
        private void DrawOption(int Index, bool Active)
        {
            Console.SetCursorPosition(_offset.X, Index + _offset.Y);
            Console.BackgroundColor = Active ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = Active ? ConsoleColor.Black : ConsoleColor.White;
            Console.WriteLine(Arr[Index]);
        }
        private void Up()
        {
            DrawOption(_sel, false);
            if (_sel > 0)
            {
                _sel--;
            }
            else
            {
                _sel = Arr.Count - 1;
            }
            DrawOption(_sel, true);
        }
        private void Down()
        {
            DrawOption(_sel, false);
            if (_sel < Arr.Count - 1)
            {
                _sel++;
            }
            else
            {
                _sel = 0;
            }
            DrawOption(_sel, true);
        }
        public void ToList(List<object> list)
        {
            List<string> temp = new List<string>();
            foreach (var item in list)
            {
                temp.Add(item.ToString());
            }
            if (temp != Arr)
            {
                if (Arr.Count != 0) Arr.Clear();
                Arr = temp;
            }
        }
        public void AddToList(params object[] puncts)
        {
            foreach (var item in puncts)
            {
                Arr.Add(item.ToString());
            }
        }
        public void ToList(params object[] puncts)
        {
            List<string> temp = new List<string>();
            foreach (var item in puncts)
            {
                temp.Add(item.ToString());
            }
            if (temp != Arr)
            {
                if (Arr.Count != 0) Arr.Clear();
                Arr = temp;
            }
        }
        public void Show()
        {
            for (int i = 0; i < Arr.Count; i++)
            {
                DrawOption(i, i == _sel);
            }
        }
        public void SetOffset(int x, int y)
        {
            _offset.X = x;
            _offset.Y = y;
        }
        public int Start()
        {
            Console.CursorVisible = false;
            ConsoleKeyInfo K;
            Show();
            do
            {
                K = Console.ReadKey(true);
                while (Console.KeyAvailable) { Console.ReadKey(true); }
                if (K.Key == ConsoleKey.UpArrow || K.Key == ConsoleKey.W)
                {
                    Up();
                }
                else if (K.Key == ConsoleKey.DownArrow || K.Key == ConsoleKey.S)
                {
                    Down();
                }
            } while (K.Key != ConsoleKey.Enter);
            Console.CursorVisible = true;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            return _sel;
        }
        public int Start(params object[] puncts)
        {
            ToList(puncts);
            return Start();
        }
        public int Start(int X, int Y)
        {
            SetOffset(X, Y);
            return Start();
        }
        public int Start(int X, int Y, params object[] puncts)
        {
            ToList(puncts);
            SetOffset(X, Y);
            return Start();
        }
    }
}

