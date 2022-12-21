using System.Text.RegularExpressions;

namespace AoC2022 {
    
    class Day5 {
        List<string> lines = new();

        Stack<char>[] stacks;

        public Day5(string inputFile="input5.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            //lines.Add("");

            parseHeader();

            // Console.WriteLine($"Top of stacks: {part1()}");
            Console.WriteLine($"Actual Top of stacks: {part2()}");
        }

        public void parseHeader() {
            int i=0;
            while(lines[i] != "") {
                i++;
            }

            int headerLength = i+1;
            i-=2;
            int stackCount = (lines[i].Length+1)/4;
            stacks = new Stack<char>[stackCount];
            for(int s=0; s<stacks.Length; s++) {
                stacks[s] = new();
            }
            for(; i>=0; i--) {
                for(int j=1,s=0; j < lines[i].Length; j+=4,s++) {
                    if(lines[i][j] != ' ') {
                        stacks[s].Push(lines[i][j]);
                    }
                }
            }

            lines.RemoveRange(0,headerLength);
        }

        public string part1() {
            Regex r = new Regex(@"move (\d+) from (\d+) to (\d+)");
            foreach(string line in lines) {
                var match = r.Match(line);
                int count = Int32.Parse(match.Groups[1].Value);
                int src = Int32.Parse(match.Groups[2].Value) - 1;
                int tgt = Int32.Parse(match.Groups[3].Value) - 1;

                while(count > 0) {
                    stacks[tgt].Push(stacks[src].Pop());
                    count--;
                }
            }

            string result = "";
            for(int i=0; i < stacks.Length; i++) {
                result += stacks[i].Peek();
            }
            return result;
        }

        public string part2() {
            Regex r = new Regex(@"move (\d+) from (\d+) to (\d+)");
            foreach(string line in lines) {
                var match = r.Match(line);
                int count = Int32.Parse(match.Groups[1].Value);
                int src = Int32.Parse(match.Groups[2].Value) - 1;
                int tgt = Int32.Parse(match.Groups[3].Value) - 1;

                Stack<char> tmp = new();
                while(count > 0) {
                    tmp.Push(stacks[src].Pop());
                    count--;
                }
                while(tmp.Count > 0) {
                    stacks[tgt].Push(tmp.Pop());
                }
            }

            string result = "";
            for(int i=0; i < stacks.Length; i++) {
                result += stacks[i].Peek();
            }
            return result;
        }
    }
}
