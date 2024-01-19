using System.Diagnostics;

record Range(int Start, int End);

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);
        Stopwatch sw = Stopwatch.StartNew();

        int result = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            Range[] ranges = line.Split(',').Select(x => { var pair = x.Split('-').Select(int.Parse).ToArray(); return new Range(pair[0], pair[1]); }).ToArray();

            if (IsOverLapped(ranges[0], ranges[1]))
            {
                ++result;
            }
        }

        sw.Stop();
        Console.WriteLine($"Result = {result}");
        Console.WriteLine($"Time = {sw.Elapsed.TotalSeconds} seconds");
    }

    private static bool IsOverLapped(Range range1, Range range2)
    {
        if (IsRange1StartInRange2(range1, range2)
            || IsRange1StartInRange2(range2, range1))
        {
            return true;
        }

        return false;
    }

    private static bool IsRange1StartInRange2(Range range1, Range range2)
    {
        return range1.Start >= range2.Start && range1.Start <= range2.End;
    }
}