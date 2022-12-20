using System.Text.RegularExpressions;

namespace AoC2022 {

    class Day20 {
        string[] lines;
        List<(int i, long v)> entries = new();

        public Day20(string inputFile="input20.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            parseInput();

            Console.WriteLine($"Grove coordinate sum: {part1()}");
            // Console.WriteLine($"Outer surface area: {part2()}");
        }

        public void parseInput() {
            for(int i=0; i < lines.Length; i++) {
                entries.Add((i,Int32.Parse(lines[i])*811589153L));
            }
        }

        public long part1() {
            for(int j=0; j < 10; j++) {
                for(int i=0; i < lines.Length; i++) {
                    int curIdx = entries.FindIndex(e => e.i == i);
                    long val = entries[curIdx].v;
                    int move = (int)(val % (lines.Length-1));
                    if(move == 0) continue;
                    entries.RemoveAt(curIdx);
                    int target = (curIdx + move + (lines.Length-1)) % (lines.Length-1);
                    entries.Insert(target, (i,val));
                }

                // foreach(var vals in entries) {
                //     Console.WriteLine(vals.v);
                // }
                // Console.WriteLine();
            }
            int zeroIdx = entries.FindIndex(e => e.v == 0);
            Console.WriteLine(entries[(zeroIdx+1000) % lines.Length].v);
            Console.WriteLine(entries[(zeroIdx+2000) % lines.Length].v);
            Console.WriteLine(entries[(zeroIdx+3000) % lines.Length].v);
            return entries[(zeroIdx+1000) % lines.Length].v+entries[(zeroIdx+2000) % lines.Length].v+entries[(zeroIdx+3000) % lines.Length].v;
        }

        public int part2() {
            return 0;
        }
    }
}
