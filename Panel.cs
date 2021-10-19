using System;
using System.Collections.Generic;
using System.IO;

namespace Commander
{
    
    public class Panel
    {
        private List<FileModel> _files = new List<FileModel>();
        private int visibleFirst;
        private int visibleLast;
        private int PanelHeight;
        public DirectoryInfo _current;
        private int _selected = 0;
        public int _offsetX { get; set; }
        public int PadRight { get; set; } = 20;
        public int PadMiddle { get; set; } = 15;
        public int PadLeft { get; set; } = 20;
        public int FullPad { get => PadRight + PadMiddle + PadLeft; }
        public Panel(DirectoryInfo dir, int offset = 0, int Height = 18)
        {
            PanelHeight = Height-2;
            _offsetX = offset;
            visibleFirst = 0;
            visibleLast = PanelHeight;
            _current = dir;
            if (_current.Parent != null)
            {
                FileModel back = new FileModel(_current.Parent);
                back.IsParentButton = true;
                _files.Add(back);
            }
            foreach (FileSystemInfo item in _current.GetDirectories())
            {
                _files.Add(new FileModel(item));
            }
            foreach (FileSystemInfo item in _current.GetFiles())
            {
                _files.Add(new FileModel(item));
            }
        }
        public FileModel BackDir()
        {
            if (_files[0].IsParentButton)
            {
                _selected = 0;
                return Get();
            }
            return null;
        }
        private void ShowOne(int Index)
        {
            Console.SetCursorPosition(_offsetX, Index - visibleFirst);
            if (_files[Index].Name.Length > PadRight - 1)
            {
                Console.Write(_files[Index].Name.Substring(0, PadRight - 4) + "...");
            }
            else
            {
                Console.Write(_files[Index].Name);
            }
            Console.SetCursorPosition(_offsetX + PadRight, Index - visibleFirst);
            if (_files[Index].IsDirectionary)
            {
                Console.Write("Dir");
            }
            else
            {
                Console.Write(_files[Index].Size + "B");
            }
            Console.SetCursorPosition(_offsetX + PadRight + PadMiddle, Index - visibleFirst);
            Console.Write(_files[Index].Date);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        private void DrawOption(int Index, bool Active)
        {
            Console.BackgroundColor = Active ? ConsoleColor.White : ConsoleColor.Black;
            Console.ForegroundColor = Active ? ConsoleColor.Black : ConsoleColor.White;
            ShowOne(Index);
        }
        public void Show(bool Active)
        {
            Clear();
            if (visibleLast+2 > _files.Count)
            {
                visibleLast = _files.Count-2;
            }
            for (int i = visibleFirst; i < visibleLast + 2; i++)
            {
                DrawOption(i, Active? i == _selected:false);
            }
        }
        public void Up()
        {
            DrawOption(_selected, false);
            if (_selected > 0)
            {
                if (_selected - 1 < visibleFirst)
                {
                    visibleFirst = _selected - 1;
                    visibleLast = _selected - 1 + (Console.WindowHeight - 3);
                    Show(true);
                    DrawOption(visibleFirst + 1, false);
                }
                _selected--;
            }
            DrawOption(_selected, true);
        }
        public void Down()
        {
            DrawOption(_selected, false);
            if (_selected < _files.Count - 1)
            {
                if (_selected > visibleLast)
                {
                    visibleLast = _selected;
                    visibleFirst = _selected - (Console.WindowHeight - 3);
                    Show(true);
                    DrawOption(visibleLast, false);
                }
                _selected++;
            }
            DrawOption(_selected, true);
        }
        public void Clear()
        {
            for (int i = 0; i < PanelHeight+2; i++)
            {
                Console.SetCursorPosition(_offsetX, i);
                for (int j = 0; j < FullPad; j++)
                {
                    
                    Console.Write(" ");
                }
            }
        }
        public FileModel Get()
        {
            return _files[_selected];
        }
        public void ReLoad(bool Active)
        {
            _files.Clear();
            if (_current.Parent != null)
            {
                FileModel back = new FileModel(_current.Parent);
                back.IsParentButton = true;
                _files.Add(back);
            }
            foreach (FileSystemInfo item in _current.GetDirectories())
            {
                _files.Add(new FileModel(item));
            }
            foreach (FileSystemInfo item in _current.GetFiles())
            {
                _files.Add(new FileModel(item));
            }
            Show(Active);
        }
    }
}
