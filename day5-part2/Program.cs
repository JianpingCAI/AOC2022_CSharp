using System.Diagnostics;
using System.Text;

record Range(int Start, int End);

internal class Program
{
    record Move(int Count, int From, int To);

    private static void Main(string[] args)
    {
        string filePath = "input2.txt";
        string[] lines = File.ReadAllLines(filePath);
        Stopwatch sw = Stopwatch.StartNew();

        ParseInput(lines, out List<Stack<char>> stacks, out List<Move> moves);

        ExecuteMoves_Version2(stacks, moves);

        string tops = GetStackTops(stacks);

        sw.Stop();
        Console.WriteLine($"Result = {tops}");
        Console.WriteLine($"Time = {sw.Elapsed.TotalSeconds} seconds");
    }

    private static string GetStackTops(List<Stack<char>> stacks)
    {
        StringBuilder sb = new();
        for (int i = 1; i < stacks.Count; i++)
        {
            sb.Append(stacks[i].Pop());
        }
        return sb.ToString();
    }

    private static void ExecuteMoves_Version2(List<Stack<char>> stacks, List<Move> moves)
    {
        foreach (var move in moves)
        {
            var fromStack = stacks[move.From];
            var toStack = stacks[move.To];
            var moveCount = move.Count;

            Stack<char> tmpStack = new();
            while (moveCount > 0)
            {
                tmpStack.Push(fromStack.Pop());
                moveCount--;
            }

            while (tmpStack.Count > 0)
            {
                toStack.Push(tmpStack.Pop());
            }
        }
    }

    private static void ParseInput(string[] lines, out List<Stack<char>> out_stacks, out List<Move> out_moves)
    {
        //find stack count
        int stackCount = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i + 1]))
            {
                stackCount = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).Count();
                break;
            }
        }

        Stack<char>[] tempStacks = new Stack<char>[stackCount];
        for (int i = 0; i < stackCount; i++)
        {
            tempStacks[i] = new Stack<char>();
        }

        out_moves = [];

        bool isStackData = true;
        for (int i = 0; i < lines.Length; i++)
        {
            if (isStackData)
            {
                if (string.IsNullOrEmpty(lines[i + 1]))
                {
                    isStackData = false;
                    ++i;
                    continue;
                }

                char[] rowCrates = ParseCratesRow(lines[i], stackCount);

                for (int k = 0; k < rowCrates.Length; k++)
                {
                    char c = rowCrates[k];
                    if (c != default)
                    {
                        tempStacks[k].Push(c);
                    }
                }
            }
            else
            {
                string[] tempArray = [.. lines[i].Split(' ')];
                Move move = new(int.Parse(tempArray[1]),
                    int.Parse(tempArray[3]),
                    int.Parse(tempArray[5]));
                out_moves.Add(move);
            }
        }

        out_stacks = new List<Stack<char>>(stackCount + 1);
        for (int i = 0; i < stackCount + 1; i++)
        {
            out_stacks.Add(new Stack<char>());
        }

        for (int i = 0; i < stackCount; i++)
        {
            while (tempStacks[i].Count > 0)
            {
                out_stacks[i + 1].Push(tempStacks[i].Pop());
            }
        }
    }

    private static char[] ParseCratesRow(string line, int stackCount)
    {
        char[] row = new char[stackCount];
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] >= 'A' && line[i] <= 'Z')
            {
                row[i / 4] = line[i];
            }
        }
        return row;
    }
}