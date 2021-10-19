using System;
using System.IO;
using System.Diagnostics;
using MenuNS;
using System.Text.RegularExpressions;

namespace Commander
{
    public class Manager
    {
        public Panel Current { get; set; }
        public Panel Second { get; set; }
        public FileModel Copy;
        enum CopyMode
        {
            NotCopy = 0,
            Copy = 1,
            Cut = 2
        }
        private CopyMode Copymode = CopyMode.NotCopy;
        public bool EnterHandler(FileModel file)
        {
            if (!file.IsDirectionary)
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(file.File.FullName)
                {
                    UseShellExecute = true
                };
                p.Start();
                return false;
            }
            else
            {
                Current = new Panel(file.File as DirectoryInfo, Current._offsetX);
                Current.Show(true);
                return true;
            }
        }
        public void ReLoad()
        {
            Current.ReLoad(true);
            Second.ReLoad(false);
        }
        public void Start(string Path)
        {
            DirectoryInfo dir = new DirectoryInfo(Path);
            Current = new Panel(dir);
            Second = new Panel(dir, Current.FullPad + 1);
            Start();
        }
        public void Start()
        {
            bool ReStart;
            while (true)
            {
                ConsoleKeyInfo K;
                Current.Show(true);
                Second.Show(false);
                Console.SetCursorPosition(0, 0);
                ReStart = false;
                while (!ReStart)
                {
                    try
                    {
                        K = Console.ReadKey(true);
                        while (Console.KeyAvailable) { Console.ReadKey(true); }
                        switch (K.Key)
                        {
                            case ConsoleKey.W:
                            case ConsoleKey.UpArrow:
                                Current.Up();
                                break;
                            case ConsoleKey.DownArrow:
                            case ConsoleKey.S:
                                Current.Down();
                                break;
                            case ConsoleKey.F1:
                                InfoPanel("F1 = Открыть информационную панель\n" +
                                    "F2 = Переименовать файл (Очень странно работает. Лучше не использовать)\n" +
                                    "F3 = Обновить панель\n" +
                                    "BACKSPACE = Вернуться в прошлую директроию (В корневой папке показывает список дисков)\n" +
                                    "Tab, A,D,Стрелочки <-,-> = Переключение между панелями\n" +
                                    "DELETE = Удалить файл\n" +
                                    "CTRL+C = Копировать\n" +
                                    "CTRL+X = Вырезать\n" +
                                    "CTRL+V = Вставить\n" +
                                    "ESCAPE = Выход");
                                break;
                            case ConsoleKey.F2:
                                RenamePanel(Current.Get());
                                break;
                            case ConsoleKey.F3:
                                ReLoad();
                                break;
                            case ConsoleKey.Delete:
                                DeleteConfirmPanel(Current.Get());
                                break;
                            case ConsoleKey.Backspace:
                                if (Current.BackDir() != null)
                                {
                                    EnterHandler(Current.BackDir());
                                }
                                else
                                {
                                    DriveSwitcher();
                                }
                                break;
                            case ConsoleKey.C:
                                if (K.Modifiers == ConsoleModifiers.Control)
                                {
                                    Copy = Current.Get();
                                    Copymode = CopyMode.Copy;
                                    InfoPanel("Скопированно");
                                }
                                break;
                            case ConsoleKey.X:
                                if (K.Modifiers == ConsoleModifiers.Control)
                                {
                                    Copy = Current.Get();
                                    Copymode = CopyMode.Cut;
                                    InfoPanel("Вырезано");
                                }
                                break;
                            case ConsoleKey.V:
                                if (K.Modifiers == ConsoleModifiers.Control && Copy != null)
                                {
                                    File.Copy(Copy.Path, Current._current.FullName + @"\" + Copy.Name);
                                    if (Copymode == CopyMode.Cut)
                                    {
                                        FileModel temp = new FileModel(Copy.File);
                                        Copy.File.Delete();
                                        Copy = new FileModel(new FileInfo(Current._current.FullName + @"\" + Copy.Name));
                                        Copymode = CopyMode.Copy;
                                    }
                                    ReLoad();
                                    InfoPanel("Вставлено");
                                }
                                break;
                            case ConsoleKey.Tab:
                            case ConsoleKey.RightArrow:
                            case ConsoleKey.LeftArrow:
                            case ConsoleKey.D:
                            case ConsoleKey.A:
                                PanelSwap();
                                break;
                            case ConsoleKey.Enter:
                                EnterHandler(Current.Get());
                                break;
                            case ConsoleKey.Escape:
                                Environment.Exit(0);
                                break;
                            default:
                                break;
                        }
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    catch (Exception Ex)
                    {
                        InfoPanel("ERROR!\n\n" + Ex.Message);
                    }
                }
            }
        }
        public void DriveSwitcher()
        {
            Current?.Clear();
            var drives = DriveInfo.GetDrives();
            Menu DriveMenu = new Menu(drives);
            var drive = drives[DriveMenu.Start(Current._offsetX, 0)].RootDirectory;
            EnterHandler(new FileModel(drive));
        }
        public void PanelSwap()
        {
            (Current, Second) = (Second, Current);
            Current.Show(true);
            Second.Show(false);
        }
        public void InfoPanel(string message)
        {
            var info = new InfoPanel(message);
            info.Show();
            Console.Clear();
            Current.Show(true);
            Second.Show(false);
        }
        public void DeleteConfirmPanel(FileModel file)
        {
            var Delete = new ConfirmPanel("Вы уверены что хотите удалить");
            if (Delete.Show())
            {
                file.File.Delete();
                ReLoad();
            }
            Console.Clear();
            Current.Show(true);
            Second.Show(true);
        }
        public void RenamePanel(FileModel file)
        {
            var info = new TextPanel("Переимениование " + file.Name + " в:");
            string newName = info.Show();
            Console.BackgroundColor = ConsoleColor.Black;
            InfoPanel(file.Path+"\n" + Regex.Match(file.File.FullName, @"^.+\\").Value +"\n"+newName+"\n"+ Regex.Match(file.File.FullName, @"^.+\\").Value + newName);
            File.Copy(file.Path, Regex.Match(file.File.FullName, @"^.+\\").Value + newName);
            File.Delete(file.Path);
            ReLoad();
        }
    }
}
