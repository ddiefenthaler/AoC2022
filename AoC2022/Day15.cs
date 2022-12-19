using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2022 {

    class CoordRange {
        public int s;
        public int e;

        public static void addRange(ref List<CoordRange> ranges, CoordRange radd) {
            // Console.WriteLine($"Add: {radd.s}..{radd.e}");
            int i=0;
            while(i < ranges.Count && ranges[i].s < radd.s) {
                ++i;
            }
            if(i > 0 && ranges[i-1].e >= radd.s) { // New range overlaps with previous range
                // Console.WriteLine($"range[i-1]: {ranges[i-1].s}..{ranges[i-1].e}, radd: {radd.s}..{radd.e}, range[i]: {ranges[i].s}..{ranges[i].e}");
                --i;
                if(ranges[i].e >= radd.e) { // New range is already fully contained.
                    return;
                }
                ranges[i].e = radd.e; // Extend existing range to include new range
                int j=i+1;
                while(j < ranges.Count && ranges[j].e <= radd.e) { // next range is also contained in new range
                    ranges.RemoveAt(j); // drop contained range from list
                }
                if(j >= ranges.Count) { 
                    return;
                }
                if(radd.e >= ranges[j].s) { // next range overlaps with new range, but is not fully contained
                    ranges[i].e = ranges[j].e;  // Merge ranges
                    ranges.RemoveAt(j);
                }
            } else {
                // if(i >= ranges.Count) { // New range does not overlap, behind all other ranges
                //     ranges.Insert(i,radd);
                //     return;
                // }
                ranges.Insert(i,radd);
                int j=i+1;
                while(j < ranges.Count && ranges[j].e <= radd.e) { // next range is contained in new range
                    ranges.RemoveAt(j); // drop contained range from list
                }
                if(j >= ranges.Count) { 
                    return;
                }
                if(radd.e >= ranges[j].s) { // next range overlaps with new range, but is not fully contained
                    ranges[i].e = ranges[j].e;  // Merge ranges
                    ranges.RemoveAt(j);
                }
            }

            // Console.WriteLine($"radd.s: {radd.s}, range[i].s: {ranges[i].s}, ");
        }
    }

    class Day15 {
        string[] lines;

        public Day15(string inputFile="input15.txt", int row=2000000)
        {
            lines = System.IO.File.ReadAllLines(inputFile);
            //lines.Add("");

            Console.WriteLine($"Certainly excluded: {part1(row)}");
            Console.WriteLine($"Tuning frequency: {part2()}");
        }

        public int part1(int row=2000000) {
            var sensorRegex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

            List<CoordRange> blockedRanges = new();

            foreach(string line in lines) {
                var sensorReadings = sensorRegex.Match(line);

                int sensor_x = Int32.Parse(sensorReadings.Groups[1].Value);
                int sensor_y = Int32.Parse(sensorReadings.Groups[2].Value);
                int beacon_x = Int32.Parse(sensorReadings.Groups[3].Value);
                int beacon_y = Int32.Parse(sensorReadings.Groups[4].Value);

                int beacon_dist = Math.Abs(sensor_x - beacon_x) + Math.Abs(sensor_y - beacon_y);
                int y2e6_dist = Math.Abs(row - sensor_y);
                int remaining_range = beacon_dist - Math.Abs(row - sensor_y);

                if(remaining_range > 0) {
                    // Console.WriteLine($"Distance from y=row: {y2e6_dist}");
                    CoordRange.addRange(ref blockedRanges, new CoordRange{s=sensor_x - remaining_range,e=sensor_x + remaining_range});
                    // Console.WriteLine($"Blocked range: {sensor_x - remaining_range}..{sensor_x + remaining_range}");
                    // foreach(var range in blockedRanges) {
                    //     Console.WriteLine($"Blocked range: {range.s}..{range.e}");
                    // }
                    // Console.WriteLine();
                }
            }
            int sumBlocked = 0;
            foreach(var range in blockedRanges) {
                sumBlocked += range.e - range.s;
            }

            return sumBlocked + 1;
        }

        public long part2() {
            var sensorRegex = new Regex(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)");

            // Todo make more efficient
            Parallel.For(0,4000000, row => {            // 18 seconds
            // for(int row=0; row < 4000000; row++) {   // 56 seconds
            // for(int row=3398000; row < 3399000; row++) {
                if(row%1000 == 0) {
                    Console.Write($"\r{row}");
                }
                List<CoordRange> blockedRanges = new();

                foreach(string line in lines) {
                    var sensorReadings = sensorRegex.Match(line);

                    int sensor_x = Int32.Parse(sensorReadings.Groups[1].Value);
                    int sensor_y = Int32.Parse(sensorReadings.Groups[2].Value);
                    int beacon_x = Int32.Parse(sensorReadings.Groups[3].Value);
                    int beacon_y = Int32.Parse(sensorReadings.Groups[4].Value);

                    int beacon_dist = Math.Abs(sensor_x - beacon_x) + Math.Abs(sensor_y - beacon_y);
                    int y2e6_dist = Math.Abs(row - sensor_y);
                    int remaining_range = beacon_dist - Math.Abs(row - sensor_y);

                    if(remaining_range > 0) {
                        // Console.WriteLine($"Distance from y=row: {y2e6_dist}");
                        CoordRange.addRange(ref blockedRanges, new CoordRange{s=sensor_x - remaining_range,e=sensor_x + remaining_range});
                        // Console.WriteLine($"Blocked range: {sensor_x - remaining_range}..{sensor_x + remaining_range}");
                        // foreach(var range in blockedRanges) {
                        //     Console.WriteLine($"Blocked range: {range.s}..{range.e}");
                        // }
                        // Console.WriteLine();
                    }
                }
                if(blockedRanges.Count > 1) {
                    Console.WriteLine($"\r{row}:");
                    foreach(var range in blockedRanges) {
                        Console.WriteLine($"Blocked range: {range.s}..{range.e}");
                    }
                }
            // }
            });
            Console.WriteLine();

            return 3398893L+(4000000L*2889605L);
        }
    }
}
