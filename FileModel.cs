using System.IO;

namespace Commander
{
    public class FileModel
    {
        public FileSystemInfo File { get; set; }
        public string Name { get => IsParentButton ? ".." : File.Name; }
        public bool IsDirectionary { get => File is DirectoryInfo; }
        public long Size
        {
            get
            {
                if (File is FileInfo)
                {
                    return (File as FileInfo).Length;
                }
                else
                {
                    return -1;
                }
            }
        }
        public string Date { get => File.LastWriteTime.ToString(); }
        public FileModel(FileSystemInfo file)
        {
            File = file;
        }
        public bool IsParentButton { get; set; } = false;
        public string Path { get => File.FullName; }
    }
}
