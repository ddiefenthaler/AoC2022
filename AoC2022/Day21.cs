using System.Text.RegularExpressions;

namespace AoC2022 {

    class MonkeySolver {
        public long solution;
        public bool solved = false;
        public string delayedBy = "";
        public string op1 = "";
        public char operand;
        public string op2 = "";

        public MonkeySolver(long solution) {
            this.solution = solution;
            this.solved = true;
        }

        public MonkeySolver(string op1, char operand, string op2) {
            this.op1 = op1;
            this.op2 = op2;
            this.operand = operand;
            this.solved = false;
        }
    }

    class Day21 {
        string[] lines;

        Dictionary<string, MonkeySolver> entries = new();

        public Day21(string inputFile="input21.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");
            
            Console.WriteLine($"Yelled by root: {part1()}");
            Console.WriteLine($"Yelled by humn: {part2()}");
        }

        public string trySolve(MonkeySolver solver) {
            if(solver.solved) {
                return "";
            }
            if(!entries.ContainsKey(solver.op1)) {
                return solver.op1;
            }
            if(!entries.ContainsKey(solver.op2)) {
                return solver.op2;
            }
            if(solver.delayedBy != "" && !entries.ContainsKey(solver.delayedBy)) {
                return solver.delayedBy;
            }

            MonkeySolver op1 = entries[solver.op1];
            MonkeySolver op2 = entries[solver.op2];
            string maybeDelay = trySolve(op1);
            if(maybeDelay != "") {
                solver.delayedBy = maybeDelay;
                return maybeDelay;
            }
            maybeDelay = trySolve(op2);
            if(maybeDelay != "") {
                solver.delayedBy = maybeDelay;
                return maybeDelay;
            }

            switch(solver.operand) {
                case '+':
                    solver.solution = op1.solution + op2.solution;
                    break;
                case '-':
                    solver.solution = op1.solution - op2.solution;
                    break;
                case '*':
                    solver.solution = op1.solution * op2.solution;
                    break;
                case '/':
                    solver.solution = op1.solution / op2.solution;
                    break;
                default:
                    break;
            }
            solver.solved = true;
            return "";
        }

        public long part1() {
            var yeller = new Regex(@"(....): (-?\d+)");
            var computer = new Regex(@"(....): (....) ([+\-*/]) (....)");
            foreach(string line in lines) {
                var match = yeller.Match(line);
                if(match.Success) {
                    var monkey = new MonkeySolver(Int64.Parse(match.Groups[2].Value));
                    entries.Add(match.Groups[1].Value,monkey);
                } else {
                    match = computer.Match(line);
                    if(match.Success) {
                        var monkey = new MonkeySolver(match.Groups[2].Value,match.Groups[3].Value[0],match.Groups[4].Value);
                        trySolve(monkey);
                        entries.Add(match.Groups[1].Value,monkey);
                    }
                }
            }

            // foreach(MonkeySolver solver in entries.Values) {
            //     if(!solver.solved) trySolve(solver);
            // }
            trySolve(entries["root"]);
            return entries["root"].solution;
        }

        public string trySolveHumn(MonkeySolver solver, string name, bool reverse=false, long expectedSolution=0) {
            if(name == "humn") {
                if(reverse) {
                    solver.solution = expectedSolution;
                    solver.solved = true;
                }
                return "humn";
            }
            if(solver.solved) {
                return "";
            }
            if(!entries.ContainsKey(solver.op1)) {
                return solver.op1;
            }
            if(!entries.ContainsKey(solver.op2)) {
                return solver.op2;
            }
            if(!reverse && solver.delayedBy == "humn") {
                return "humn";
            }
            if(solver.delayedBy != "" && !entries.ContainsKey(solver.delayedBy)) {
                return solver.delayedBy;
            }

            if(name == "root") {
                MonkeySolver op1 = entries[solver.op1];
                MonkeySolver op2 = entries[solver.op2];
                MonkeySolver reverser = op1;// = null;
                MonkeySolver target = op2;// null;
                string reverserName = solver.op1;
                string maybeDelay = trySolveHumn(op1,solver.op1);
                if(maybeDelay != "" && maybeDelay != "humn") {
                    solver.delayedBy = maybeDelay;
                    return maybeDelay;
                }
                if(maybeDelay == "humn") {
                    reverser = op1;
                    target = op2;
                    reverserName = solver.op1;
                }
                maybeDelay = trySolveHumn(op2,solver.op2);
                if(maybeDelay != "" && maybeDelay != "humn") {
                    solver.delayedBy = maybeDelay;
                    return maybeDelay;
                }
                if(maybeDelay == "humn") {
                    reverser = op2;
                    target = op1;
                    reverserName = solver.op2;
                }
                return trySolveHumn(reverser,reverserName,true,target.solution);
            }

            if(!reverse) {
                MonkeySolver op1 = entries[solver.op1];
                MonkeySolver op2 = entries[solver.op2];
                string maybeDelay = trySolveHumn(op1,solver.op1);
                if(maybeDelay != "") {
                    solver.delayedBy = maybeDelay;
                    return maybeDelay;
                }
                maybeDelay = trySolveHumn(op2,solver.op2);
                if(maybeDelay != "") {
                    solver.delayedBy = maybeDelay;
                    return maybeDelay;
                }

                switch(solver.operand) {
                    case '+':
                        solver.solution = op1.solution + op2.solution;
                        break;
                    case '-':
                        solver.solution = op1.solution - op2.solution;
                        break;
                    case '*':
                        solver.solution = op1.solution * op2.solution;
                        break;
                    case '/':
                        solver.solution = op1.solution / op2.solution;
                        break;
                    default:
                        break;
                }
                solver.solved = true;
                return "";
            } else {
                MonkeySolver op1 = entries[solver.op1];
                MonkeySolver op2 = entries[solver.op2];
                MonkeySolver? reverser = op1;// = null;
                string reverserName = solver.op1;
                MonkeySolver? otherOperand = op2;// null;
                byte reverseOperand  = 1;
                long requiredOperand = 0;
                string maybeDelay = trySolveHumn(op1,solver.op1);
                if(maybeDelay != "" && maybeDelay != "humn") {
                    solver.delayedBy = maybeDelay;
                    return maybeDelay;
                }
                if(maybeDelay == "humn") {
                    reverseOperand = 1;
                    reverser = op1;
                    otherOperand = op2;
                    reverserName = solver.op1;
                }
                maybeDelay = trySolveHumn(op2,solver.op2);
                if(maybeDelay != "" && maybeDelay != "humn") {
                    solver.delayedBy = maybeDelay;
                    return maybeDelay;
                }
                if(maybeDelay == "humn") {
                    reverseOperand = 2;
                    reverser = op2;
                    otherOperand = op1;
                    reverserName = solver.op2;
                }

                switch(solver.operand) {
                    case '+':
                        requiredOperand = expectedSolution - otherOperand.solution;
                        break;
                    case '-':
                        if(reverseOperand == 1)
                            requiredOperand = expectedSolution + op2.solution;
                        else
                            requiredOperand = op1.solution - expectedSolution;
                        break;
                    case '*':
                        requiredOperand = expectedSolution / otherOperand.solution;
                        break;
                    case '/':
                        if(reverseOperand == 1)
                            requiredOperand = expectedSolution * op2.solution;
                        else
                            requiredOperand = op1.solution / expectedSolution;
                        break;
                    default:
                        break;
                }

                return trySolveHumn(reverser,reverserName,true,requiredOperand);
            }
        }

        public long part2() {
            entries.Clear();
            var yeller = new Regex(@"(....): (-?\d+)");
            var computer = new Regex(@"(....): (....) ([+\-*/]) (....)");
            foreach(string line in lines) {
                var match = yeller.Match(line);
                if(match.Success) {
                    var monkey = new MonkeySolver(Int64.Parse(match.Groups[2].Value));
                    if(match.Groups[1].Value == "humn") {
                        monkey.solved = false;
                    }
                    entries.Add(match.Groups[1].Value,monkey);
                } else {
                    match = computer.Match(line);
                    if(match.Success) {
                        var monkey = new MonkeySolver(match.Groups[2].Value,match.Groups[3].Value[0],match.Groups[4].Value);
                        // trySolveHumn(monkey,match.Groups[1].Value);
                        entries.Add(match.Groups[1].Value,monkey);
                    }
                }
            }

            // foreach(var solver in entries) {
            //     if(!solver.Value.solved) trySolveHumn(solver.Value,solver.Key);
            // }
            trySolveHumn(entries["root"],"root");
            return entries["humn"].solution;
        }
    }
}
