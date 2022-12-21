using System.Text.RegularExpressions;

namespace AoC2022 {
    
    class Day10 {
        string[] lines;

        public Day10(string inputFile="input10.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            Console.WriteLine($"Sum of interesting signals: {part1()}");
            // Console.WriteLine($"Longer rope visited: {part2()}");
            part2();
        }

        public int part1() {
            int sum=0;
            int cycle=0;
            int cycle_interesting = 20;
            int X=1;
            var instr_noop = new Regex(@"^noop$");
            var instr_addx = new Regex(@"^addx (-?\d+)$");
            foreach(string line in lines) {
                int inc=0;
                if(instr_noop.IsMatch(line)) {
                    cycle++;
                } else {
                    var match = instr_addx.Match(line);
                    if(match.Success) {
                        cycle += 2;
                        inc = Int32.Parse(match.Groups[1].Value);
                    }
                }

                if(cycle >= cycle_interesting) {
                    sum += cycle_interesting*X;
                    // Console.WriteLine($"{line}: {cycle_interesting*X}, X:{X}, X':{X+inc}");
                    cycle_interesting += 40;
                }

                X += inc;
            }
            return sum;
        }

        public void part2() {
            int cycle=0;
            int drawn = 0;
            // int cycle_interesting = 20;
            int X=1;
            var instr_noop = new Regex(@"^noop$");
            var instr_addx = new Regex(@"^addx (-?\d+)$");
            foreach(string line in lines) {
                int inc=0;
                if(instr_noop.IsMatch(line)) {
                    cycle++;
                } else {
                    var match = instr_addx.Match(line);
                    if(match.Success) {
                        cycle += 2;
                        inc = Int32.Parse(match.Groups[1].Value);
                    }
                }

                while(drawn < cycle) {
                    int line_pos = drawn % 40;
                    if(line_pos >= X-1 && line_pos <= X+1) {
                        Console.Write("@");
                    } else {
                        Console.Write(" ");
                    }
                    // Console.WriteLine($"{line_pos} {X}");
                    drawn++;
                    if(drawn % 40 == 0) {
                        Console.WriteLine();
                    }
                }
                        // Console.WriteLine();

                X += inc;
            }
        }
    }
}
