using System.Text.RegularExpressions;

namespace AoC2022 {

    class Day22 {
        string[] lines;
        byte[,] field = new byte[150,200];
        int[] movements;
        char[] directions;
        int x,y = 0;
        int start_x;
        int facing = 0;

        public Day22(string inputFile="input22.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            parseInput();

            Console.WriteLine($"Grove password: {part1()}");
            Console.WriteLine($"Grove password with cube: {part2()}");
        }

        void printField(int tx = -1, int ty = -1) {
            for(int j=0; j < field.GetLength(1); j++) {
                for(int i=0; i < field.GetLength(0); i++) {
                    if(i == tx && j == ty) {
                        Console.Write("T");
                        continue;
                    }
                    if(i == x && j == y) {
                        switch(facing) {
                            case 0:
                                Console.Write(">");
                                break;
                            case 1:
                                Console.Write("v");
                                break;
                            case 2:
                                Console.Write("<");
                                break;
                            case 3:
                                Console.Write("^");
                                break;
                            default:
                                break;
                        }
                        continue;
                    }
                    switch(field[i,j]) {
                        case 0:
                            Console.Write(" ");
                            break;
                        case 1:
                            Console.Write(".");
                            break;
                        case 2:
                            Console.Write("#");
                            break;
                        case 3:
                            Console.Write(">");
                            break;
                        case 4:
                            Console.Write("v");
                            break;
                        case 5:
                            Console.Write("<");
                            break;
                        case 6:
                            Console.Write("^");
                            break;
                        default:
                            break;
                    }
                }
                Console.WriteLine();
            }
        }

        public void parseInput() {
            int i;
            // int maxLength=0;
            bool foundStart = false;
            for(i=0; i < lines.Length; i++) {
                for(int j=0; j < lines[i].Length; j++) {
                    switch(lines[i][j]) {
                        case '.':
                            field[j,i] = 1;
                            if(!foundStart) {
                                start_x = x = j;
                                foundStart = true;
                            }
                            break;
                        case '#':
                            field[j,i] = 2;
                            break;
                        default:
                            break;
                    }
                }
                // entries.Add((i,Int32.Parse(lines[i])*811589153L));
                // if(lines[i].Length > maxLength) {
                //     maxLength = lines[i].Length;
                // }
                if(lines[i] == "") {
                    break;
                }
            }
            // Console.WriteLine($"{i},{maxLength}");

            movements = lines[i+1].Split(new char [] {'L','R'}).Select(s => Int32.Parse(s)).ToArray();
            directions = lines[i+1].Split(new char[] {'0','1','2','3','4','5','6','7','8','9'},StringSplitOptions.RemoveEmptyEntries).Select(s => s[0]).ToArray();
        }

        void move(int distance) {
            if(distance == 0) {
                return;
            }

            int old_x = x;
            int old_y = y;

            switch(facing) {
                case 0:
                    moveTo(x+1,y,1,0);
                    break;
                case 1:
                    moveTo(x,y+1,0,1);
                    break;
                case 2:
                    moveTo(x-1,y,-1,0);
                    break;
                case 3:
                    moveTo(x,y-1,0,-1);
                    break;
                default:
                    break;
            }

            if(old_x == x && old_y == y)
                return;

            move(distance-1);
            return;
        }

        void moveTo(int x, int y, int dx, int dy) {
            x = (x+field.GetLength(0)) % field.GetLength(0);
            y = (y+field.GetLength(1)) % field.GetLength(1);
            // x >= 0 && x < field.GetLength(0) && y >= 0 && y < field.GetLength(1) <-- should be always true
            // find next field in facing direction
            while(field[x,y] == 0) {
                x+=dx;
                y+=dy;
                x = (x+field.GetLength(0)) % field.GetLength(0);
                y = (y+field.GetLength(1)) % field.GetLength(1);
            }

            // act according to wall or free space
            if(field[x,y] == 2) {
                return;
            } else {//if(field[x,y] == 1) {
                this.x = x;
                this.y = y;
                return;
            }
            throw(new IndexOutOfRangeException());
        }

        void rotate(char dir) {
            switch(dir) {
                case 'L':
                    // facing--;
                    facing += 3;
                    break;
                case 'R':
                    facing++;
                    break;
                default:
                    throw(new ArgumentOutOfRangeException());
            }
            facing %= 4;
        }

        public int part1() {
            for(int i=0; i < movements.Length || i < directions.Length; i++) {
                move(movements[i]);
                if(i < directions.Length) {
                    rotate(directions[i]);
                }
            }
            return (y+1)*1000 + (x+1)*4 + facing;
        }

        void moveCube(int distance) {
            if(distance == 0) {
                return;
            }

            int old_x = x;
            int old_y = y;

            switch(facing) {
                case 0:
                    moveToCube(x+1,y);
                    break;
                case 1:
                    moveToCube(x,y+1);
                    break;
                case 2:
                    moveToCube(x-1,y);
                    break;
                case 3:
                    moveToCube(x,y-1);
                    break;
                default:
                    break;
            }

            if(old_x == x && old_y == y)
                return;

            moveCube(distance-1);
            return;
        }

        void moveToCube(int x, int y) {
            int new_facing = facing;
            bool print = false;
            if(y == -1) {
                if(x < 100) {   // 50-99
                    y = 100 + x;
                    x = 0;
                    new_facing = 0;
                    print = true;
                } else {        // 100-149
                    y = 199;
                    x = 149 - x;
                    print = true;
                }
            } else if(x == 150) {
                // y: 0-49
                y = 149 - y;
                x = 99;
                new_facing = 2;
                print = true;
            } else if(y == 50 && x >= 100) {
                y = x - 50;
                x = 99;
                new_facing = 2;
                print = true;
            } else if(x == 100 && y >= 50) {
                if(y < 100) {   // 50-99
                    x = y + 50;
                    y = 49;
                    new_facing = 3;
                    print = true;
                } else {        // 100-149
                    y = 149 - y;
                    x = 149;
                    new_facing = 2;
                    print = true;
                }
            } else if(y == 150 && x >= 50) {
                y = 100 + x;
                x = 49;
                new_facing = 2;
                print = true;
            } else if(x == 50 && y >= 150) {
                x = y - 100;
                y = 149;
                new_facing = 3;
                print = true;
            } else if(y == 200) {
                y = 0;
                x = 149 - x;
                print = true;
            } else if(x == -1) {
                if(y >= 150) {  // 150-199
                    x = y - 100;
                    y = 0;
                    new_facing = 1;
                    print = true;
                } else {        // 100-149
                    x = 50;
                    y = 149 - y;
                    new_facing = 0;
                    print = true;
                }
            } else if(y == 99 && x <= 49) {
                y = x + 50;
                x = 50;
                new_facing = 0;
                print = true;
            } else if(x == 49 && y <= 99) {
                if(y >= 50) {   // 50-99
                    x = y - 50;
                    y = 100;
                    new_facing = 1;
                    print = true;
                } else {        // 0-49
                    x = 0;
                    y = 149 - y;
                    new_facing = 0;
                    print = true;
                }
            }

            // act according to wall or free space
            if(field[x,y] == 2) {
                return;
            } else {// if(field[x,y] == 1) {
                if(print) {
                    printField(x,y);
                    Console.ReadKey(true);
                }
                this.x = x;
                this.y = y;
                facing = new_facing;
                field[x,y] = (byte)(new_facing+3);
                return;
            }
            throw(new IndexOutOfRangeException());
        }

        public int part2() {
            y = 0;
            x = start_x;
            facing = 0;
            for(int i=0; i < movements.Length || i < directions.Length; i++) {
                moveCube(movements[i]);
                if(i < directions.Length) {
                    rotate(directions[i]);
                }
            }
            return (y+1)*1000 + (x+1)*4 + facing;
        }
    }
}
