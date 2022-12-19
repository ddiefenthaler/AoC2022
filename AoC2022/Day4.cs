
namespace AoC2022 {
    
    class Day4 {
        List<string> lines = new();

        public Day4(string inputFile="input4.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            //lines.Add("");

            Console.WriteLine($"Duplicate ranges: {part1()}");
            Console.WriteLine($"Overlapping ranges: {part2()}");
        }

        public int part1() {
            int containing_counter = 0;
            foreach(string line in lines) {
                string[] nrs = line.Split('-',',');
                (int,int) r1 = (Int32.Parse(nrs[0]),Int32.Parse(nrs[1]));
                (int,int) r2 = (Int32.Parse(nrs[2]),Int32.Parse(nrs[3]));

                if(r1.Item1 <= r2.Item1 && r1.Item2 >= r2.Item2) {
                    containing_counter++;
                    continue;
                }
                if(r1.Item1 >= r2.Item1 && r1.Item2 <= r2.Item2) {
                    containing_counter++;
                    continue;
                }
            }
            return containing_counter;
        }

        public int part2() {
            int overlapping_counter = 0;
            foreach(string line in lines) {
                string[] nrs = line.Split('-',',');
                (int,int) r1 = (Int32.Parse(nrs[0]),Int32.Parse(nrs[1]));
                (int,int) r2 = (Int32.Parse(nrs[2]),Int32.Parse(nrs[3]));

                if(r1.Item1 <= r2.Item1 && r1.Item2 >= r2.Item1 ||
                   r1.Item1 <= r2.Item2 && r1.Item2 >= r2.Item2 ||
                   r2.Item1 <= r1.Item1 && r2.Item2 >= r1.Item1 ||
                   r2.Item1 <= r1.Item2 && r2.Item2 >= r1.Item2) {
                    overlapping_counter++;
                }
            }
            return overlapping_counter;
        }
    }
}
