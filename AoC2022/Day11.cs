using System.Text.RegularExpressions;

namespace AoC2022 {

    class Monkey {
        List<Int64> items = new();
        char op;
        string operand2;
        Int64 operand2_used;
        int divisor;
        int target1;
        int target2;
        public int inspectCount = 0;

        public Monkey(string[] definition) {
            var items_regex = new Regex(@"^  Starting items: ?(.*)$");
            var operation_regex = new Regex(@"  Operation: new = old (.) (.+)");
            var test_regex = new Regex(@"  Test: divisible by (\d+)");
            var target1_regex = new Regex(@"    If true: throw to monkey (\d+)");
            var target2_regex = new Regex(@"    If false: throw to monkey (\d+)");

            var match = items_regex.Match(definition[0]);
            string starting_items = match.Groups[1].Value;
            foreach(string item in starting_items.Split(new[] { ',', ' ' },StringSplitOptions.RemoveEmptyEntries)) {
                items.Add(Int32.Parse(item));
            }
            match = operation_regex.Match(definition[1]);
            op = match.Groups[1].Value[0];
            operand2 = match.Groups[2].Value;
            Int64.TryParse(match.Groups[2].Value, out operand2_used);
            match = test_regex.Match(definition[2]);
            divisor = Int32.Parse(match.Groups[1].Value);
            match = target1_regex.Match(definition[3]);
            target1 = Int32.Parse(match.Groups[1].Value);
            match = target2_regex.Match(definition[4]);
            target2 = Int32.Parse(match.Groups[1].Value);
        }

        private void inspect(int worry_divider) {
            for(int i=0; i < items.Count; i++) {
                if(operand2 == "old") {
                    operand2_used = items[i];
                }
                switch(op) {
                    case '+':
                        items[i] += operand2_used;
                        // todo calculate from all monkey divisors;
                        items[i] %= 9699690;
                        // items[i] %= 96577;
                        break;
                    case '-':
                        items[i] -= operand2_used;
                        items[i] %= 9699690;
                        // items[i] %= 96577;
                        break;
                    case '*':
                        items[i] *= operand2_used;
                        items[i] %= 9699690;
                        // items[i] %= 96577;
                        break;
                    case '/':
                        items[i] /= operand2_used;
                        items[i] %= 9699690;
                        // items[i] %= 96577;
                        break;
                }
                inspectCount++;

                items[i] /= worry_divider;
            }
        }

        private void throwItem(Monkey[] targets, int item) {
            if(item % divisor == 0) {
                targets[target1].catchItem(item);
            } else {
                targets[target2].catchItem(item);
            }
        }

        public void throw_all(Monkey[] targets, int worry_divider = 3) {
            inspect(worry_divider);

            foreach(int item in items) {
                throwItem(targets, item);
            }

            items.Clear();
        }

        public void catchItem(int item) {
            items.Add(item);
        }
    }

    class Day11 {
        string[] lines;
        Monkey[] monkeys;

        public Day11(string inputFile="input11.txt")
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            parseInput();

            // Console.WriteLine($"Monkey business after     20: {part1()}");
            // Todo reset monkeys;
            Console.WriteLine($"Monkey business after 10.000: {part2()}");
        }

        public void parseInput() {
            int monkey_count = (lines.Length+1) / 7;
            monkeys = new Monkey[monkey_count];
            for(int i=0; i < monkey_count; i++) {
                // Not done: accept arbitrary monkey order
                monkeys[i] = new Monkey(lines[(i*7 + 1) .. (i*7 + 6)]);
            }
        }

        public int part1() {
            for(int round=0; round < 20; round++) {
                foreach(var monkey in monkeys) {
                    monkey.throw_all(monkeys);
                }
            }
            List<int> monkey_business = new ();
            foreach(var monkey in monkeys) {
                monkey_business.Add(monkey.inspectCount);
            }
            monkey_business.Sort();
            return monkey_business[^1]*monkey_business[^2];
        }

        public Int64 part2() {
            for(int round=0; round < 10000; round++) {
                foreach(var monkey in monkeys) {
                    monkey.throw_all(monkeys, 1);
                }
            }
            List<Int64> monkey_business = new ();
            foreach(var monkey in monkeys) {
                monkey_business.Add(monkey.inspectCount);
            }
            monkey_business.Sort();
            return monkey_business[^1]*monkey_business[^2];
        }
    }
}
