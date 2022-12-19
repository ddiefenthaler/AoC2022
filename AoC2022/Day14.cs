// using System.Text.Json;

namespace AoC2022 {

    class Day14 {
        string[] lines;

        byte[,] field = new byte[400,200];
        int max_y = 0;

        public Day14(string inputFile="input14.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            parseInput();
            // printField();

            // Console.WriteLine($"Resting sand: {part1()}");
            // Todo reset??
            Console.WriteLine($"Resting sand: {part2()}");
        }

        public static (int, int) parseCoord(string coord) {
            string[] coord_s = coord.Split(',');
            int x = Int32.Parse(coord_s[0]) - 300;
            int y = Int32.Parse(coord_s[1]);
            return (x,y);
        }

        public void parseInput() {
            foreach(string line in lines) {
                string[] coords = line.Split(" -> ");
                int prev_x,prev_y;
                (prev_x, prev_y) = parseCoord(coords[0]);
                if(prev_y > max_y) {
                    max_y = prev_y;
                }
                for(int i=1; i < coords.Length; i++) {
                    int x,y;
                    (x,y) = parseCoord(coords[i]);
                    if(y > max_y) {
                        max_y = y;
                    }
                    int dx, dy;
                    dx = Math.Sign(x - prev_x);
                    dy = Math.Sign(y - prev_y);
                    for(int ix=prev_x,iy=prev_y; ix != x || iy != y; ix+=dx, iy+=dy) {
                        field[ix,iy] = 1;
                    }
                    field[x,y] = 1;
                    prev_x = x;
                    prev_y = y;
                }
            }
        }

        public void printField() {
            for(int j=0; j < max_y+3; j++) {
            // for(int j=0; j < 25; j++) {
                for(int i=35; i < 325; i++) {
                    char c = '.';
                    if(field[i,j] == 1) {
                        c = '#';
                    } else if(field[i,j] == 2) {
                        c = 'o';
                    }
                    Console.Write(c);
                }
                Console.WriteLine();
            }
        }

        public bool dropSand(int x = 500, int y = -1) {
            x -= 300;
            while(true) {
                ++y;
                if(y+1 >= field.GetLength(1)) {
                    return false;
                }
                if(field[x,y+1] == 0) {
                    continue;
                }
                if(field[x-1,y+1] == 0) {
                    --x;
                    continue;
                }
                if(field[x+1,y+1] == 0) {
                    ++x;
                    continue;
                }
                break;
            }
            field[x,y] = 2;
            if(x == 200 && y == 0) {
                return false;
            }
            return true;
        }

        public int part1() {
            int i = 0;
            while(dropSand()) {
                i++;
            }
            printField();
            return i;
        }

        public void addFloor() {
            for(int i=0; i < field.GetLength(0); i++) {
                field[i,max_y+2] = 1;
            }
        }

        public int part2() {
            addFloor();
            int i = 0;
            while(dropSand()) {
                i++;
            }
            i++;
            printField();
            return i;
        }
    }
}
