using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;

namespace Commander
{
    class InfoPanel
    {
        public static Coord offset = new Coord() { X = 30, Y = 3 };
        public int width = 40;
        protected string[] _message;
        public InfoPanel(string message)
        {
            List<string> temp = new List<string>();
            var match = Regex.Matches(message, @".{40}|.+\n|.+$");
            foreach (Match item in match)
            {
                temp.Add(item.ToString());
            }
            _message = temp.ToArray();
        }
        public void Show()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            for (int i = 0; i < _message.Length+2; i++)
            {
                for (int j = 0; j < width + 4; j++)
                {
                    Console.SetCursorPosition(offset.X+j, offset.Y + i);
                    Console.Write(" ");
                }
            }
            for (int i = 0; i < _message.Length; i++)
            {
                Console.SetCursorPosition(offset.X + 2, offset.Y + i + 1);
                Console.Write(_message[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ReadKey();
        }
    }
    class ConfirmPanel
    {
        public static Coord offset = new Coord() { X = 30, Y = 3 };
        public int width = 40;
        private bool selec = false;
        protected string[] _message;
        public ConfirmPanel(string message)
        {
            List<string> temp = new List<string>();
            var match = Regex.Matches(message, @".{40}|.+\n|.+$");
            foreach (Match item in match)
            {
                temp.Add(item.ToString());
            }
            _message = temp.ToArray();
        }
        private void DrawOption()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(offset.X + 5, offset.Y + _message.Length + 2);
            Console.BackgroundColor = selec ? ConsoleColor.Cyan : ConsoleColor.Blue;
            Console.Write("Да");
            Console.SetCursorPosition(offset.X + width - 5, offset.Y + _message.Length + 2);
            Console.BackgroundColor = !selec ? ConsoleColor.Cyan : ConsoleColor.Blue;
            Console.Write("Нет");
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public bool Show()
        {
            Console.BackgroundColor = ConsoleColor.Blue;
            for (int i = 0; i < _message.Length + 4; i++)
            {
                for (int j = 0; j < width + 4; j++)
                {
                    Console.SetCursorPosition(offset.X + j, offset.Y + i);
                    Console.Write(" ");
                }
            }
            for (int i = 0; i < _message.Length; i++)
            {
                Console.SetCursorPosition(offset.X + 2, offset.Y + i + 1);
                Console.Write(_message[i]);
            }
            Console.BackgroundColor = ConsoleColor.Black;
            ConsoleKeyInfo K;
            while (true)
            {
                DrawOption();
                K = Console.ReadKey();
                switch (K.Key)
                {
                    case ConsoleKey.LeftArrow:
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.A:
                    case ConsoleKey.D:
                        if (selec)
                        {
                            selec = false;
                        }
                        else
                        {
                            selec = true;
                        }
                        DrawOption();
                        break;
                    case ConsoleKey.Enter:
                        return selec;
                    default:
                        break;
                }
            }
        }
    }
    class TextPanel
    {
        public static Coord offset = new Coord() { X = 30, Y = 3 };
        public int width = 40;
        protected string[] _message;
        public TextPanel(string message)
        {
            List<string> temp = new List<string>();
            var match = Regex.Matches(message, @".{40}|.+\n|.+$");
            foreach (Match item in match)
            {
                temp.Add(item.ToString());
            }
            _message = temp.ToArray();
        }
        public string Show()
        {
            for (int i = 0; i < _message.Length + 4; i++)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                for (int j = 0; j < width + 4; j++)
                {
                    if (i == _message.Length + 2 && j > 1 && j < width+2)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    Console.SetCursorPosition(offset.X + j, offset.Y + i);
                    Console.Write(" ");
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                }
            }
            for (int i = 0; i < _message.Length; i++)
            {
                Console.SetCursorPosition(offset.X + 2, offset.Y + i + 1);
                Console.Write(_message[i]);
            }
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(offset.X + 2, offset.Y + _message.Length + 2);
            return Console.ReadLine();
        }
    }
}
