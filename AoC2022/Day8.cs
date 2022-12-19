using System.Collections;

namespace AoC2022 {

    class Day8 {
        string[] lines;

        public Day8(string inputFile="input8.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            Console.WriteLine($"Visible trees: {part1()}");
            Console.WriteLine($"Best scenic score: {part2()}");
        }

public static Int32 GetCardinality(BitArray bitArray)
{

    Int32[] ints = new Int32[(bitArray.Count >> 5) + 1];

    bitArray.CopyTo(ints, 0);

    Int32 count = 0;

    // fix for not truncated bits in last integer that may have been set to true with SetAll()
    ints[ints.Length - 1] &= ~(-1 << (bitArray.Count % 32));

    for (Int32 i = 0; i < ints.Length; i++)
    {

        Int32 c = ints[i];

        // magic (http://graphics.stanford.edu/~seander/bithacks.html#CountBitsSetParallel)
        unchecked
        {
        c = c - ((c >> 1) & 0x55555555);
        c = (c & 0x33333333) + ((c >> 2) & 0x33333333);
        c = ((c + (c >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
        }

        count += c;

    }

    return count;

}

        public int part1() {
            int size = lines.Length;
            BitArray visible = new BitArray(size*size);
            for(int i=0; i < size; i++) {
                // line-wise ->
                int max=lines[i][0];
                visible.Set(i*size + 0, true);
                for(int j=1; j < size-1; j++) {
                    if(lines[i][j] > max) {
                        max = lines[i][j];
                        visible.Set(i*size + j, true);
                    }
                }
                // line-wise <-
                max=lines[i][size-1];
                visible.Set(i*size + size-1, true);
                for(int j=size-2; j > 0; j--) {
                    if(lines[i][j] > max) {
                        max = lines[i][j];
                        visible.Set(i*size + j, true);
                    }
                }
            }
            for(int j=0; j < size; j++) {
                // col-wise down
                int max=lines[0][j];
                visible.Set(0*size + j, true);
                for(int i=1; i < size-1; i++) {
                    if(lines[i][j] > max) {
                        max = lines[i][j];
                        visible.Set(i*size + j, true);
                    }
                }
                // col-wise up
                max=lines[size-1][j];
                visible.Set((size-1)*size + j, true);
                for(int i=size-2; i > 0; i--) {
                    if(lines[i][j] > max) {
                        max = lines[i][j];
                        visible.Set(i*size + j, true);
                    }
                }
            }
            return GetCardinality(visible);
        }

        public int findScenicScore(int size, int x, int y, out int r, out int l, out int d, out int u) {
            r = l = d = u = 0;
            int thresh = lines[y][x];
            int scenicScore = 1;
            int dirs = 15;
            int right = 1 << 0;
            int left  = 1 << 1;
            int down  = 1 << 2;
            int up    = 1 << 3;
            for(int i=1; dirs != 0; i++) {
                if((dirs & right) != 0) {
                    if(x+i == size-1 || lines[y][x+i] >= thresh) {
                        scenicScore *= i;
                        r=i;
                        dirs &= ~right;
                    }
                }
                if((dirs & left) != 0) {
                    if(x-i == 0 || lines[y][x-i] >= thresh) {
                        scenicScore *= i;
                        l=i;
                        dirs &= ~left;
                    }
                }
                if((dirs & down) != 0) {
                    if(y+i == size-1 || lines[y+i][x] >= thresh) {
                        scenicScore *= i;
                        d=i;
                        dirs &= ~down;
                    }
                }
                if((dirs & up) != 0) {
                    if(y-i == 0 || lines[y-i][x] >= thresh) {
                        scenicScore *= i;
                        u=i;
                        dirs &= ~up;
                    }
                }

                if(dirs == 0) {
                    break;
                }
            }

            return scenicScore;
        }

        public int part2() {
            int size = lines.Length;
            int max = 0;
            for(int i=1; i < size-1; i++) {
                for(int j=1; j < size-1; j++) {
                    int r,l,d,u;
                    int scenicScore = findScenicScore(size, i, j, out r, out l, out d, out u);
                    if(scenicScore > max) {
                        Console.WriteLine($"{i},{j}: {r}*{l}*{d}*{u} = {scenicScore}");
                        max = scenicScore;
                    }
                }
            }
            return max;
        }
    }
}
