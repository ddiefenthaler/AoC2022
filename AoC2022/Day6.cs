
namespace AoC2022 {
    
    class Day6 {
        List<string> lines = new();

        public Day6(string inputFile="input6.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            //lines.Add("");

            Console.WriteLine($"End of packet-marker: {part1()}");
            Console.WriteLine($"End of message-marker: {part2()}");
        }

        public static int minimalShift(string candidate) {
            for(int i=candidate.Length-1; i > 0; i--) {
                for(int j=i-1; j >= 0; j--) {
                    if(candidate[i] == candidate[j]) {
                        return j+1;
                    }
                }
            }
            return 0;
        }

        public static int searchMarker(string line, int markerLength) {
            int shiftBy = markerLength - 1;
            int i=shiftBy;
            while(shiftBy > 0 && i < line.Length) {
                shiftBy = minimalShift(line[(i-markerLength+1)..(i+1)]);
                i += shiftBy;
            }
            if(i >= line.Length) {
                return -1;
            }
            return i+1;
        }

        public int part1() {
            string line = lines[0];
            return searchMarker(line, 4);
        }

        public int part2() {
            string line = lines[0];
            return searchMarker(line, 14);
        }
    }
}
