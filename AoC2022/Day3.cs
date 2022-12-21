
namespace AoC2022 {
    
    class Day3 {
        List<string> lines = new();

        public Day3(string inputFile="input3.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            //lines.Add("");

            Console.WriteLine($"Sum of items: {part1()}");
            Console.WriteLine($"Sum of badges: {part2()}");
        }

        public static int get_prio(char item) {
            int prio = item - 'a' + 1;
            if(prio < 0) {
                prio += 'a' - 'A' + 26;
            }
            return prio;
        }

        public int part1() {
            int sum = 0;
            foreach(string line in lines) {
                System.UInt64 bitfield = 0;
                for(int i=0; i < line.Length; i++) {
                    if(i < line.Length/2) {
                        bitfield |= (1ul << get_prio(line[i]));
                    } else {
                        if((bitfield & (1ul << get_prio(line[i]))) != 0) {
                            sum += get_prio(line[i]);
                            // Console.WriteLine(get_prio(line[i]) + ", "+line[i]);
                            bitfield = 0;
                            continue;
                        }
                    }
                }
            }
            return sum;
        }

        public static int find_bit(ulong candidates) {
            int i=1;
            for(System.UInt64 b=2ul; b != (1ul << 53); b <<= 1) {
                if((b & candidates) != 0) {
                    break;
                }
                i++;
            }
            return i;
        }

        public int part2() {
            int sum = 0;
            int group_counter = 0;
            System.UInt64 candidates = 0xFFFFFFFFFFFFFFFF;
            foreach(string line in lines) {
                System.UInt64 bitfield = 0;
                for(int i=0; i < line.Length; i++) {
                    bitfield |= (1ul << get_prio(line[i]));
                }
                candidates &= bitfield;
                // Console.WriteLine(String.Format("{0,53}",Convert.ToString((long)bitfield,2)));
                // Console.WriteLine(String.Format("{0,53}",Convert.ToString((long)candidates,2)));

                group_counter++;
                if(group_counter == 3) {
                    // Console.WriteLine(find_bit(candidates));
                    sum += find_bit(candidates);
                    candidates = 0xFFFFFFFFFFFFFFFF;
                    group_counter = 0;
                }
            }
            return sum;
        }
    }
}
