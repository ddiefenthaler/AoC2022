using System.Text.RegularExpressions;
using System.Xml;
using QuikGraph;
using QuikGraph.Graphviz;

namespace AoC2022 {

    class Valve {
        public string name = "";
        public int rate;

        public Valve(string name, int rate) {
            this.name = name;
            this.rate = rate;
        }
    }

    class PressureWithOpenedValves {
        public int pressure = 0;
        public HashSet<Valve> opened = new();
        public List<string> openedWhen = new();
        // public List<string> valveTraversal = new();

        public static bool operator>(PressureWithOpenedValves a, PressureWithOpenedValves b) {
            return a.pressure > b.pressure;
        }

        public static bool operator<(PressureWithOpenedValves a, PressureWithOpenedValves b) {
            return a.pressure < b.pressure;
        }
    }

    class Day16 {
        string[] lines;

        Dictionary<string, Valve> valveList = new();
        // Undirected instead?
        AdjacencyGraph<Valve, Edge<Valve>> network = new();
        Dictionary<HashSet<Valve>, PressureWithOpenedValves> helperCache = new(HashSet<Valve>.CreateSetComparer());

        public Day16(string inputFile="input16.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            parseInput();

            string dotGraph = network.ToGraphviz(algorithm => {
                algorithm.FormatVertex += (sender, args) => {
                    args.VertexFormat.Label = $"{args.Vertex.name}";
                };
            });
            File.WriteAllText("network16.dot",dotGraph);

            Console.WriteLine($"Best case pressure: {part1()}");
            Console.WriteLine($"Best case pressure by 2: {part2()}");
        }

        public void parseInput() {
            var valveRegex = new Regex(@"^Valve (..) has flow rate=(\d+); tunnels? leads? to valves? (..+)$");
            foreach(string line in lines) {
                var match = valveRegex.Match(line);
                string valveName = match.Groups[1].Value;
                int valveRate = Int32.Parse(match.Groups[2].Value);
                valveList.Add(valveName, new Valve(valveName,valveRate));
            }
            network.AddVertex(valveList["AA"]);
            foreach(string line in lines) {
                var match = valveRegex.Match(line);
                string   source = match.Groups[1].Value;
                string[] targetValves = match.Groups[3].Value.Split(", ");
                foreach(string target in targetValves) {
                    network.AddVerticesAndEdge(new Edge<Valve>(valveList[source],valveList[target]));
                }
            }
        }

        public PressureWithOpenedValves traverseOrOpenValves(Valve valve, in HashSet<Valve> opened, Valve? prev = null, int depth=30,
                                        int helper=0, Valve? startValve=null, int startDepth=30) {
            if(depth == 0) {
                if(helper > 0) {
                    if(!helperCache.ContainsKey(opened)) {
                        helperCache.Add(opened, traverseValves(startValve, opened, null, startDepth, helper-1, startValve, startDepth));
                        Console.Write($"\r{helperCache.Count}");
                    }
                    PressureWithOpenedValves tmp = helperCache[opened];
                    return new PressureWithOpenedValves{pressure=tmp.pressure};
                    // return traverseValves(startValve, opened, null, startDepth, helper-1, startValve, startDepth);
                }
                return new PressureWithOpenedValves{pressure=0,opened=opened};
            }
            if(!opened.Contains(valve) && valve.rate != 0) {
                PressureWithOpenedValves possiblePressureOpened;
                PressureWithOpenedValves possiblePressureClosed;
                // open valve and move on
                HashSet<Valve> modifiedOpened = new HashSet<Valve>(opened);
                modifiedOpened.Add(valve);
                possiblePressureOpened = traverseValves(valve,modifiedOpened,null,depth-1,helper,startValve,startDepth);
                possiblePressureOpened.pressure += (depth-1)*valve.rate;
                possiblePressureOpened.openedWhen.Add($"{valve.name}: {depth-1}");
                // or is it better to leave it closed?
                possiblePressureClosed = traverseValves(valve,opened,prev,depth-1,helper,startValve,startDepth);
                if(possiblePressureOpened > possiblePressureClosed) {
                    return possiblePressureOpened;
                } else {
                    return possiblePressureClosed;
                }
            } else {
                // no point in opening the valve
                return traverseValves(valve,opened,prev,depth,helper,startValve,startDepth);
            }

        }

        public PressureWithOpenedValves traverseValves(Valve valve, in HashSet<Valve> opened, Valve? prev = null, int depth=30,
                                  int helper=0, Valve? startValve=null, int startDepth=30) {
            if(depth == 0) {
                if(helper > 0) {
                    if(!helperCache.ContainsKey(opened)) {
                        helperCache.Add(opened, traverseValves(startValve, opened, null, startDepth, helper-1, startValve, startDepth));
                        Console.Write($"\r{helperCache.Count}");
                    }
                    PressureWithOpenedValves tmp = helperCache[opened];
                    return new PressureWithOpenedValves{pressure=tmp.pressure};
                    // return traverseValves(startValve, opened, null, startDepth, helper-1, startValve, startDepth);
                }
                return new PressureWithOpenedValves{pressure=0,opened=opened};
            }
            PressureWithOpenedValves max = new PressureWithOpenedValves{pressure=0,opened=opened};
            foreach(var edge in network.OutEdges(valve)) {
                if(edge.Target == prev) {
                    // don't just go back
                    continue;
                }
                PressureWithOpenedValves possiblePressure = traverseOrOpenValves(edge.Target, opened, valve, depth-1,helper,startValve,startDepth);
                if(possiblePressure > max) {
                    max = possiblePressure;
                }
            }
            // Console.Write($"\r{depth,3}, {max,20}");
            return max;
        }

        public int part1() {
            HashSet<Valve> opened = new HashSet<Valve>();
            PressureWithOpenedValves dbg = traverseValves(valveList["AA"],opened);
            return dbg.pressure;
            // return traverseValves(valveList["AA"],opened).pressure;
        }

        public int part2() {
            HashSet<Valve> opened = new HashSet<Valve>();
            PressureWithOpenedValves dbg = traverseValves(valveList["AA"],opened,null,26,1,valveList["AA"],26);
            return dbg.pressure;
            // return traverseValves(valveList["AA"],opened,null,30,1,valveList["AA"],30).pressure;
        }
    }
}


// Minutes           : 21
// Seconds           : 9
// Milliseconds      : 307
// Ticks             : 12693077271
// TotalDays         : 0,01469106165625
// TotalHours        : 0,35258547975
// TotalMinutes      : 21,155128785
// TotalSeconds      : 1269,3077271
// TotalMilliseconds : 1269307,7271
