// using System.Text.Json;

namespace AoC2022 {

    class Day17 {
        string[] lines;

        static (int,int)[] rock1 = {(0,0),(1,0),(2,0),(3,0)};
        static (int,int)[] rock2 = {(1,0),(0,1),(1,1),(2,1),(1,2)};
        static (int,int)[] rock3 = {(0,0),(1,0),(2,0),(2,1),(2,2)};
        static (int,int)[] rock4 = {(0,0),(0,1),(0,2),(0,3)};
        static (int,int)[] rock5 = {(0,0),(1,0),(0,1),(1,1)};
        static (int x,int y)[][] rocks = {rock1,rock2,rock3,rock4,rock5};
        List<byte> field = new ();
        byte dropping = 0;
        int left = 2;
        int bottom = 3; // field.Count + 3
        long saved = 0;

        public Day17(string inputFile="input17.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            Console.WriteLine($"Tower height: {part1()}");
            Console.WriteLine($"Tower height at infinity: {part2()}");
        }

        public void printField() {
            foreach(var row in field.AsEnumerable().Reverse()) {
                for(int p=1 << 6; p > 0; p >>=1) {
                    if((row & p) != 0) {
                        Console.Write('#');
                    } else {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        public int rockWidth() {
            switch(dropping) {
                case 0:
                    return 4;
                case 1:
                case 2:
                    return 3;
                case 3:
                    return 1;
                case 4:
                    return 2;
                default:
                    throw new InvalidDataException();
            }
        }

        public bool checkCollision(int rowIdx, int x) {
            if(rowIdx >= field.Count) {
                return false; // no collision
            }
            byte row = field[rowIdx];
            if((row & (64 >> x)) != 0) {
                return true;
            }
            return false; // no collision
        }

        public void writeRow(int y, byte new_row) {
            if(y == field.Count) {
                field.Add(new_row);
            } else {
                field[y] |= new_row;
            }
            // if(new_row == 127) { never happens // row full, nothing can go past
            if(y > 3000000) { // 3*10^6
                saved += 2000000;
                bottom -= 2000000;
                field.RemoveRange(0,2000000);
                // Console.WriteLine("Test");
            }
        }

        public void addRock() {
            int y = bottom;
            byte new_row = 0;
            foreach(var part in rocks[dropping]) {
                if(y != bottom + part.y) {
                    writeRow(y, new_row);
                    y = bottom + part.y;
                    new_row = 0;
                }
                new_row |= (byte) (64 >> (left + part.x));
            }
            writeRow(y, new_row);
        }

        public bool moveDown() {
            // Check collision with floor
            if(bottom == 0) {
                addRock();
                return false;
            }
            // To high, no collision possible
            if(bottom - 1 >= field.Count) {
                bottom--;
                return true;
            }
            // Check collision with field
            foreach(var part in rocks[dropping]) {
                if(checkCollision(bottom + part.y - 1,left + part.x)) {
                    addRock();
                    return false;
                }
            }
            bottom--;
            return true;
        }

        public bool moveLeft() {
            // Check collision with wall
            if(left == 0) {
                return false;
            }
            // To high, no collision possible
            if(bottom >= field.Count) {
                left--;
                return true;
            }
            // Check collision with field
            foreach(var part in rocks[dropping]) {
                if(checkCollision(bottom + part.y,left + part.x - 1)) {
                    return false;
                }
            }
            left--;
            return true;
        }

        public bool moveRight() {
            // Check collision with wall
            if(left + rockWidth() == 7) {
                return false;
            }
            // To high, no collision possible
            if(bottom  >= field.Count) {
                left++;
                return true;
            }
            // Check collision with field
            foreach(var part in rocks[dropping]) {
                if(checkCollision(bottom + part.y,left + part.x + 1)) {
                    return false;
                }
            }
            left++;
            return true;
        }

        public long part1(long targetDrops = 2022) {
            int dropped=0;
            int curWind=0;
            int prevDropped = 0;
            long prevHeight = 0;
            while(true) {
                if(lines[0][curWind] == '>') {
                    moveRight();
                } else if(lines[0][curWind] == '<') {
                    moveLeft();
                }
                curWind++;
                if(curWind == lines[0].Length) {
                    // wind resets, loop visible?
                    // Console.WriteLine();
                    
                    Console.WriteLine($"r:{dropping},b:{bottom - field.Count},dd:{dropped - prevDropped},dh:{field.Count + saved - prevHeight}");
                    prevDropped = dropped;
                    prevHeight = field.Count + saved;
                    if(dropped > 1700*4) break;
                }
                curWind %= lines[0].Length;
                if(!moveDown()) {
                    dropped++;
                    dropping++;
                    dropping %= 5;
                    left = 2;
                    bottom = field.Count + 3;
                    if(dropped % 1000000 == 0) {
                        // Console.Write($"\r{dropped/1000000,7}");
                        // Console.Write($"\r{field.Count}");
                    }
                    if(dropped == targetDrops) {
                        // Console.WriteLine();
                        // printField();
                        break;
                    }
                }
            }
            return field.Count + saved;
        }

        public long part2() {
            field.Clear();
            dropping = 0;
            left = 2;
            bottom = 3;
            saved = 0;
            // Console.WriteLine((1000000000000-1713)%1700+1713);
            // Todo get these values from active runs
            return part1((1000000000000-1713)%1700+1713) + (1000000000000-1713)/1700 * 2660L;
        }
    }
}
