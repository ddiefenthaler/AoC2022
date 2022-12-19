using System.Text.RegularExpressions;

namespace AoC2022 {

    class Day18 {
        string[] lines;

        bool[,,] droplet;
        bool[,,] exterior;

        public Day18(string inputFile="input18.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            droplet = new bool[24,24,24];
            exterior = new bool[24,24,24];
            parseInput();
            markExterior();

            Console.WriteLine($"Surface area: {part1()}");
            Console.WriteLine($"Outer surface area: {part2()}");
        }

        public void parseInput() {
            var coords = new Regex(@"(\d+),(\d+),(\d+)");
            // int min=30;
            // int max=0;
            foreach(string line in lines) {
                var match = coords.Match(line);
                int x = Byte.Parse(match.Groups[1].Value)+1;
                int y = Byte.Parse(match.Groups[2].Value)+1;
                int z = Byte.Parse(match.Groups[3].Value)+1;
                droplet[x,y,z] = true;
                // if(x > max) max = x;
                // if(y > max) max = y;
                // if(z > max) max = z;
                // if(x < min) min = x;
                // if(y < min) min = y;
                // if(z < min) min = z;
            }
            // Console.WriteLine($"Max: {max}, Min: {min}");
        }

        public void markExterior() {
            for(int i=0; i < 24; i++) {
                for(int j=0; j < 24; j++) {
                    exterior[0,i,j] = true;
                    exterior[23,i,j] = true;
                    exterior[i,0,j] = true;
                    exterior[i,23,j] = true;
                    exterior[i,j,0] = true;
                    exterior[i,j,23] = true;
                }
            }
        }

        public int getSurfaces(int x, int y, int z) {
            int exposedSurfaces = 6;
            if(droplet[x+1,y,z]) exposedSurfaces--;
            if(droplet[x-1,y,z]) exposedSurfaces--;
            if(droplet[x,y+1,z]) exposedSurfaces--;
            if(droplet[x,y-1,z]) exposedSurfaces--;
            if(droplet[x,y,z+1]) exposedSurfaces--;
            if(droplet[x,y,z-1]) exposedSurfaces--;
            return exposedSurfaces;
        }

        public int part1() {
            int sum=0;
            for(int x=1; x < 23; x++) {
                for(int y=1; y < 23; y++) {
                    for(int z=1; z < 23; z++) {
                        if(droplet[x,y,z]) {
                            sum += getSurfaces(x,y,z);
                        }
                    }
                }
            }
            return sum;
        }

        public static void mergeAreas(ref bool[,,] area, List<(int x,int y,int z)> candidateArea) {
            foreach(var coord in candidateArea) {
                area[coord.x,coord.y,coord.z] = true;
            }
        }

        public static (int x,int y,int z)[] getNeighbors((int x,int y,int z)c) {
            (int x,int y,int z)[]neighbors = new (int x,int y,int z)[6];
            neighbors[0] = (c.x+1,c.y,c.z);
            neighbors[1] = (c.x-1,c.y,c.z);
            neighbors[2] = (c.x,c.y+1,c.z);
            neighbors[3] = (c.x,c.y-1,c.z);
            neighbors[4] = (c.x,c.y,c.z+1);
            neighbors[5] = (c.x,c.y,c.z-1);
            return neighbors;
        }

        public void fillInterior(int x,int y,int z) {
            List<(int x,int y,int z)> candidates = new();
            candidates.Add((x,y,z));
            bool[,,] candidateAreaBool = new bool[24,24,24];
            List<(int x,int y,int z)> candidateArea = new();
            candidateAreaBool[x,y,z] = true;
            candidateArea.Add((x,y,z));
            while(candidates.Count > 0) {
                foreach(var neigh in getNeighbors(candidates[0])) {
                    if(isExterior(neigh)) {
                        mergeAreas(ref exterior,candidateArea);
                        return;
                    }
                    if(!isClassified(neigh,candidateAreaBool)) {
                        candidates.Add(neigh);
                        candidateArea.Add(neigh);
                        candidateAreaBool[neigh.x,neigh.y,neigh.z] = true;
                    }
                }
                candidates.RemoveAt(0);
            }
            mergeAreas(ref droplet, candidateArea);
        }

        public bool isExterior((int x,int y,int z)c) {
            if(exterior[c.x,c.y,c.z]) return true;
            return false;
        }

        public bool isClassified((int x,int y,int z)c, bool[,,]? extra = null) {
            if(exterior[c.x,c.y,c.z] || droplet[c.x,c.y,c.z]) return true;
            if(extra != null && extra[c.x,c.y,c.z]) return true;
            return false;
        }

        public int part2() {
            for(int x=1; x < 23; x++) {
                for(int y=1; y < 23; y++) {
                    for(int z=1; z < 23; z++) {
                        if(!isClassified((x,y,z))) {
                            fillInterior(x,y,z);
                        }
                    }
                }
            }

            int sum=0;
            for(int x=1; x < 23; x++) {
                for(int y=1; y < 23; y++) {
                    for(int z=1; z < 23; z++) {
                        if(droplet[x,y,z]) {
                            sum += getSurfaces(x,y,z);
                        }
                    }
                }
            }
            return sum;
        }
    }
}
