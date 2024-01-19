using System.Diagnostics;

public interface ISystemItem
{
    public DirItem Parent { get; set; }

    public int Size { get; set; }

    public string Name { get; set; }
}

public class SystemItemBase
{
    public DirItem Parent { get; set; }

    public string Name { get; set; } = string.Empty;
    public int Size { get; set; } = 0;
}

public class FileItem : SystemItemBase, ISystemItem
{
}

public class DirItem : SystemItemBase, ISystemItem
{
    public List<ISystemItem> SubItems { get; set; } = [];
}

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);
        Stopwatch sw = Stopwatch.StartNew();

        DirItem root = BuildFileSystem(lines);

        List<DirItem> dirItems = GetDirItemsWithMaxSize_BFS(root, 100000);

        long result = dirItems.Select(x => x.Size).Sum();

        sw.Stop();
        Console.WriteLine($"Result = {result}");
        Console.WriteLine($"Time = {sw.Elapsed.TotalSeconds} seconds");
    }

    private static List<DirItem> GetDirItemsWithMaxSize_BFS(DirItem root, int maxSize)
    {
        List<DirItem> dirItems = [];
        Queue<DirItem> queDirs = new();
        queDirs.Enqueue(root);

        while (queDirs.Count > 0)
        {
            var dirItem = queDirs.Dequeue();
            if (dirItem.Size <= maxSize)
            {
                dirItems.Add(dirItem);
            }
            foreach (ISystemItem subDirItem in dirItem.SubItems)
            {
                if (subDirItem is DirItem)
                {
                    queDirs.Enqueue(subDirItem as DirItem);
                }
            }
        }

        return dirItems;
    }

    private static DirItem BuildFileSystem(string[] lines)
    {
        DirItem root = new() { Name = "/" };

        DirItem curDir = root;
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            // switch dir
            if (line.StartsWith("$ cd"))
            {
                string dirName = new(line.Skip(5).ToArray());
                if (dirName == "..")
                {
                    curDir = curDir.Parent;
                }
                else
                {
                    curDir = (DirItem)curDir.SubItems.First(x => x.Name == dirName);
                }

                continue;
            }
            else if (line.StartsWith("$ ls"))
            {
                continue;
            }
            else if (line.StartsWith("dir"))
            {
                DirItem dirItem = new() { Name = new string(line.Skip(4).ToArray()), Parent = curDir };

                curDir.SubItems.Add(dirItem);
            }
            else
            {
                var tempArray = line.Split(' ').ToArray();
                int size = int.Parse(tempArray[0]);
                string name = tempArray[1];

                FileItem fileItem = new() { Name = name, Size = size, Parent = curDir };

                curDir.SubItems.Add(fileItem);
                //curDir.Size += size;
            }
        }

        // calculate size
        root.Size = GetSize(root);

        return root;
    }

    private static int GetSize(ISystemItem systemItem)
    {
        if (systemItem is FileItem)
        {
            return (systemItem as FileItem).Size;
        }

        foreach (ISystemItem item in (systemItem as DirItem).SubItems)
        {
            systemItem.Size += GetSize(item);
        }
        return systemItem.Size;
    }
}