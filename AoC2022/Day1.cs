
namespace AoC2022 {
    
    class Day1 {
        List<string> lines = new();

        public Day1(string inputFile="input1.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            lines.Add("");

            Console.WriteLine($"Maximum calories: {part1()}");
            Console.WriteLine($"Sum of top 3: {part2()}");
        }

        public int part1() {
            int max = 0;
            int sum = 0;
            foreach(string line in lines) {
                if(line == "") {
                    if(sum > max) max=sum;
                    sum = 0;
                    continue;
                }

                sum += Int32.Parse(line);
            }
            return max;
        }

        public int part2() {
            int[] max = {0,0,0};
            int sum = 0;
            foreach(string line in lines) {
                if(line == "") {
                    if(sum > max[0]) {
                        max[2] = max[1];
                        max[1] = max[0];
                        max[0] = sum;
                    } else if(sum > max[1]) {
                        max[2] = max[1];
                        max[1] = sum;
                    } else if(sum > max[2]) {
                        max[2] = sum;
                    }
                    //Console.WriteLine(sum);
                    sum = 0;
                    continue;
                }

                sum += Int32.Parse(line);
            }
            return max[0]+max[1]+max[2];
        }
    }
}
