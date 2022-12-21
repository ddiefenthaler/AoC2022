using System.Text.RegularExpressions;

namespace AoC2022 {

    class DirTree : Dictionary<string, DirTree> {
        public bool isDir = false;
        public int size;
        public DirTree parent;

        public DirTree(DirTree? _parent = null, int _size=0) {
            size = _size;
            if(size == 0) {
                isDir = true;
            }
            if(_parent == null) {
                parent = this;
            } else {
                parent = _parent;
            }
        }
    }

    class Day7 {
        List<string> lines = new();
        DirTree root = new();

        public Day7(string inputFile="input7.txt")
        {
            lines.AddRange(System.IO.File.ReadAllLines(inputFile));
            //lines.Add("");

            parseInput();

            initDirSize(root);
            //printDir(root);

            Console.WriteLine($"Sum of smaller dirs: {part1()}");
            Console.WriteLine($"Directory to delete: {part2()}");
        }

        public void parseInput() {
            DirTree current = root;
            var cd = new Regex(@"^\$ cd (.+)$");
            var ls = new Regex(@"^\$ ls$");
            var dir = new Regex(@"^dir (.+)$");
            var file = new Regex(@"^(\d+) (.+$)");
            foreach(string line in lines) {
                if(line[0] == '$') {
                    var match = cd.Match(line);
                    if(match.Success) {
                        switch(match.Groups[1].Value) {
                            case "/":
                                current = root;
                                break;
                            case "..":
                                current = current.parent;
                                break;
                            default:
                                string dirname = match.Groups[1].Value;
                                if(!current.ContainsKey(dirname)) {
                                    current.Add(dirname, new DirTree(current));
                                }
                                current = current[dirname];
                                break;
                        }
                    }
                    // if(ls.Match(line).Success) {
                    //     continue;
                    // }
                } else {
                    var match = dir.Match(line);
                    if(match.Success) {
                        string dirname = match.Groups[1].Value;
                        if(!current.ContainsKey(dirname)) {
                            current.Add(dirname, new DirTree(current));
                        }
                        continue;
                    }
                    match = file.Match(line);
                    if(match.Success) {
                        string filename = match.Groups[2].Value;
                        int size = Int32.Parse(match.Groups[1].Value);
                        if(!current.ContainsKey(filename)) {
                            current.Add(filename, new DirTree(current,size));
                        }
                        continue;
                    }
                }
            }
        }

        public static void printDir(DirTree dirs, string name = "/", int depth = 0) {
            string indent = new string(' ', depth);
            Console.WriteLine($"{indent}- {name}: {dirs.size}");
            foreach(var item in dirs) {
                printDir(item.Value,item.Key,depth+1);
            }
        }

        public static void initDirSize(DirTree dirs) {
            int sum = 0;
            foreach(DirTree dir in dirs.Values) {
                if(dir.size == 0) {
                    initDirSize(dir);
                }
                sum += dir.size;
            }
            dirs.size = sum;
        }

        public static int getSmallDirs(DirTree dirs) {
            int sum = 0;
            if(dirs.size < 100000) {
                sum = dirs.size;
            }
            foreach(var item in dirs.Where(i => i.Value.isDir)) {
                sum += getSmallDirs(item.Value);
            }
            return sum;
        }

        // static string currentBest = "";
        // static int diffBest = Int32.MaxValue;
        int currentBest = Int32.MaxValue;
        public int getSufficientDir(DirTree dirs, int missing, string name = "/") { // use ref instead of return and class member
            // int over = dirs.size - missing;
            // if(over >= 0 && over < diffBest) {
            //     currentBest = name;
            //     diffBest = over;
            //     // Console.WriteLine($"{currentBest}: {over}");
            // }
            if(dirs.size >= missing && dirs.size < currentBest) {
                currentBest = dirs.size;
            }
            foreach(var item in dirs.Where(i => i.Value.isDir)) {
                getSufficientDir(item.Value, missing, item.Key);
            }
            return currentBest;
        }

        public int part1() {
            return getSmallDirs(root);
        }

        public int part2() {
            int total = 70000000;
            int required = 30000000;
            int unused = total - root.size;
            int missing = required - unused;
            // Console.WriteLine($"missing: {missing}");

            return getSufficientDir(root,missing);
        }
    }
}
