using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);
        Stopwatch sw = Stopwatch.StartNew();

        string line = lines[0];

        int result = Find_start_of_packet_location(line, 14);

        sw.Stop();
        Console.WriteLine($"Result = {result}");
        Console.WriteLine($"Time = {sw.Elapsed.TotalSeconds} seconds");
    }

    private static int Find_start_of_packet_location(string line, int markerLength)
    {
        HashSet<char> chars = [];
        Dictionary<char, int> dict_char_count = [];
        for (int i = 0; i < line.Length; i++)
        {
            // remove first one if needed
            if (i - markerLength >= 0)
            {
                dict_char_count[line[i - markerLength]]--;
                if (dict_char_count[line[i - markerLength]] == 0)
                {
                    chars.Remove(line[i - markerLength]);
                }
            }

            // add the current one
            if (!chars.Contains(line[i]))
            {
                chars.Add(line[i]);
            }

            if (dict_char_count.TryGetValue(line[i], out int value))
            {
                dict_char_count[line[i]] = ++value;
            }
            else
            {
                dict_char_count[line[i]] = 1;
            }

            if (chars.Count == markerLength)
                return i + 1;
        }

        return -1;
    }
}