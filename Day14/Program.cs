namespace AdventOfCode.Day14
{
	public static class Day14 {

		public static void Main() {

			var inputFile = "./input.real.txt";

			var template = InputParser.Parse(inputFile, x => x).First();

			var rules = new Dictionary<string, char>();
			int lineNum = 0;

			foreach (string line in InputParser.Parse(inputFile, x => x)) {
				if (lineNum < 2) {
					lineNum++;
					continue;
				}

				var rule = line.Split(" -> ");
				rules.Add(rule[0], rule[1][0]);
			}

			Dictionary<string, long> polymer = new();

			for (int i = 0; i < template.Count()-1; i++) {
				var key = $"{template[i]}{template[i+1]}";
				if(!polymer.ContainsKey(key)) {
					polymer.Add(key, 0);
				}

				polymer[key]++;
			}

			for (int step = 0; step < 40; step++) {
				Dictionary<string, long> next = new();
				foreach (KeyValuePair<string, long> kp in polymer) {
					if (rules.ContainsKey(kp.Key)) {
						char c = rules[kp.Key];
						var key = $"{kp.Key[0]}{c}";
						if(!next.ContainsKey(key)) {
							next.Add(key, 0);
						}

						next[key] += kp.Value;

						key = $"{c}{kp.Key[1]}";
						if(!next.ContainsKey(key)) {
							next.Add(key, 0);
						}

						next[key] += kp.Value;
					} else if (!next.ContainsKey(kp.Key)) {
						next.Add(kp.Key, kp.Value);
					} else {
						next[kp.Key] += kp.Value;
					}
				}

				polymer = next;

				// foreach (KeyValuePair<string, int> kp in polymer) {
				// 	Console.WriteLine($"{kp.Key}: {kp.Value}");
				// }
				// Console.WriteLine();
			}

			Dictionary<char, long> counts = new();
			foreach (KeyValuePair<string, long> kp in polymer) {
				if (!counts.ContainsKey(kp.Key[0])) {
					counts.Add(kp.Key[0], 0);
				}
				counts[kp.Key[0]]+= kp.Value;
			}

			var last = template[template.Count()-1];
			if(!counts.ContainsKey(last)) {
				counts.Add(last, 0);
			}

			counts[last] += 1;

			// foreach (KeyValuePair<char, int> kp in counts) {
			// 	Console.WriteLine($"{kp.Key}: {kp.Value}");
			// }
			// Console.WriteLine();

			Console.WriteLine($"Part 1: {counts.Values.Max()} - {counts.Values.Min()} = {counts.Values.Max() - counts.Values.Min()}");
		}
	}
}