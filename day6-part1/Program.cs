using System.Diagnostics;

internal class Program
{
    private static void Main(string[] args)
    {
        string filePath = "input.txt";
        string[] lines = File.ReadAllLines(filePath);
        Stopwatch sw = Stopwatch.StartNew();

        string line = lines[0];

        int result = Find_start_of_packet_location(line);

        sw.Stop();
        Console.WriteLine($"Result = {result}");
        Console.WriteLine($"Time = {sw.Elapsed.TotalSeconds} seconds");
    }

    private static int Find_start_of_packet_location(string line)
    {
        HashSet<char> chars = [];
        Dictionary<char, int> dict_char_count = [];
        for (int i = 0; i < line.Length; i++)
        {
            // remove first one if needed
            if (i - 4 >= 0)
            {
                dict_char_count[line[i - 4]]--;
                if (dict_char_count[line[i - 4]] == 0)
                {
                    chars.Remove(line[i - 4]);
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

            if (chars.Count == 4)
                return i + 1;
        }

        return -1;
    }
}