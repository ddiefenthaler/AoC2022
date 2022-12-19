using System.Drawing;

namespace AoC2022 {
    
    class Day9 {
        string[] lines;
        Point head_pos = new(0,0);
        // Point tail_pos = new(0,0);
        Point[] rope_pos;
        HashSet<Point> tail_visited = new(new[] {new Point(0,0)});
        // HashSet<Point> tail9_visited = new(new[] {new Point(0,0)});

        public Day9(string inputFile="input9.txt")
        {
            uint rope_length=10;
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            rope_pos = new Point[rope_length];
            for(int i=0; i < rope_pos.Length; i++) {
                rope_pos[i] = new (0,0);
            }

            Console.WriteLine($"Visited positions: {part1()}");

            head_pos = new(0,0);
            rope_pos = new Point[rope_length];
            for(int i=0; i < rope_pos.Length; i++) {
                rope_pos[i] = new (0,0);
            }
            tail_visited = new(new[] {new Point(0,0)});
            Console.WriteLine($"Longer rope visited: {part2()}");
        }

        public void drawRope(int tail_offset) {
            for(int j=-15; j <= 5; j++) {
                for(int i=-11; i <= 14; i++) {
                    if(head_pos.X == i && head_pos.Y == j) {
                        Console.Write("H");
                        continue;
                    }
                    for(int r=0;r<tail_offset; r++) {
                        if(rope_pos[r].X == i && rope_pos[r].Y == j) {
                            Console.Write(r+1);
                            goto CONT_OUTER;
                        }
                    }
                    if(tail_visited.Contains(new Point(i,j))) {
                        Console.Write("#");
                    } else {
                        Console.Write(".");
                    }
CONT_OUTER:;
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");
        }

        public void moveHead(int dx, int dy, int tail_offset=1) {
            int dir;
            dir = Math.Sign(dx);
            dx *= dir;
            for(;dx > 0; dx--) {
                head_pos.Offset(dir,0);
                moveRope(tail_offset);
                // drawRope(tail_offset);
                // moveTail(tail_offset);
            }
            dir = Math.Sign(dy);
            dy *= dir;
            for(;dy > 0; dy--) {
                head_pos.Offset(0,dir);
                moveRope(tail_offset);
                // moveTail(tail_offset);
            }
            // drawRope(tail_offset);
        }

        // public void moveTail(int t=0) {
        //     Point diff = Point.Subtract(head_pos, new Size(tail_pos));
        //     if(Math.Abs(diff.X) > 1 || Math.Abs(diff.Y) > 1) {
        //         int dx = Math.Sign(diff.X);
        //         int dy = Math.Sign(diff.Y);
        //         tail_pos.Offset(dx, dy);
        //         Console.Write($"\r{tail_pos.X,5}, {tail_pos.Y,5}");
        //         tail_visited.Add(tail_pos);
        //     }
        //     // Thread.Sleep(100);
        // }

        public void moveRope(int tail_offset) {
            tail_offset--;
            Point pull = head_pos;
            // Point pulled = rope_pos[0];
            for(int i=0; i < rope_pos.Length; pull = rope_pos[i++]) {
                // pulled = rope_pos[i];
                Point diff = Point.Subtract(pull, new Size(rope_pos[i]));
                if(Math.Abs(diff.X) > 1 || Math.Abs(diff.Y) > 1) {
                    int dx = Math.Sign(diff.X);
                    int dy = Math.Sign(diff.Y);
                    rope_pos[i].Offset(dx, dy);
                    if(i == tail_offset) {
                        // Console.Write($"\r{rope_pos[i].X,5}, {rope_pos[i].Y,5}");
                        tail_visited.Add(rope_pos[i]);
                    }
                } else {
                    break;
                }
            }
            // Thread.Sleep(100);
        }

        public int part1() {
            foreach(string line in lines) {
                int dx = 0,dy = 0;
                switch(line[0]) {
                    case 'R':
                        dx = 1*Int32.Parse(line.Substring(2));
                        break;
                    case 'L':
                        dx = -1*Int32.Parse(line.Substring(2));
                        break;
                    case 'D':
                        dy = 1*Int32.Parse(line.Substring(2));
                        break;
                    case 'U':
                        dy = -1*Int32.Parse(line.Substring(2));
                        break;
                }
                moveHead(dx,dy);
            }
            return tail_visited.Count;
        }

        public int part2() {
            foreach(string line in lines) {
                int dx = 0,dy = 0;
                switch(line[0]) {
                    case 'R':
                        dx = 1*Int32.Parse(line.Substring(2));
                        break;
                    case 'L':
                        dx = -1*Int32.Parse(line.Substring(2));
                        break;
                    case 'D':
                        dy = 1*Int32.Parse(line.Substring(2));
                        break;
                    case 'U':
                        dy = -1*Int32.Parse(line.Substring(2));
                        break;
                }
                moveHead(dx,dy,9);
            }
            return tail_visited.Count;
        }
    }
}
