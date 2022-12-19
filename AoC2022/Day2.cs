
namespace AoC2022 {
    
    class Day2 {
        List<string> lines = new();

        public Day2(string inputFile="input2.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            //lines.Add("");

            Console.WriteLine($"Expected score: {part1()}");
            Console.WriteLine($"Actually expected score: {part2()}");
        }

        public static int score_shape(char shape) {
            switch(shape) {
                case 'A':
                case 'X':
                    return 1;
                case 'B':
                case 'Y':
                    return 2;
                case 'C':
                case 'Z':
                    return 3;
            }
            return -99999;
        }

        /**
         * Score of the round, shape2 is my play
         */
        public static int score_round(char shape1, char shape2) {
            if(shape1 < 'X') shape1 += (char)23;
            if(shape2 < 'X') shape2 += (char)23;
            int diff = shape1 - shape2;
            if(diff < 0) diff += 3;
            if(diff == 0) return 3;
            if(diff == 1) return 0;
            if(diff == 2) return 6;
            return -99999;
        }

        /**
         * Score of the round, result is the expected play
         */
        public static int score_round_from_result(char result) {
            switch(result) {
                case 'X':
                    return 0;
                case 'Y':
                    return 3;
                case 'Z':
                    return 6;
            }
            return -99999;
        }

        public static char required_play(char shape1, char result) {
            int diff = result - 'Y';
            diff *= -1;
            if(diff < 0) diff += 3;
            shape1 += (char) 23;
            char shape2 = (char) (shape1 - (char)diff);
            // Console.WriteLine(shape1 + " - " + diff + " = " + shape2);
            if(shape2 < 'X') shape2 += (char)3;
            return shape2;
        }

        public int part1() {
            int sum = 0;
            foreach(string line in lines) {
                sum += score_shape(line[2]);
                sum += score_round(line[0],line[2]);
            }
            return sum;
        }

        public int part2() {
            int sum = 0;
            foreach(string line in lines) {
                sum += score_shape(required_play(line[0],line[2]));
                sum += score_round_from_result(line[2]);
            }
            return sum;
        }
    }
}
