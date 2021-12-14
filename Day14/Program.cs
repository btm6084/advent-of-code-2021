using System.Diagnostics;

namespace AdventOfCode.Day14
{
	public static class Day14 {

		public static void Main() {

			var inputFile = "./input.real.txt";

			var template = InputParser.Parse(inputFile, x => x).First();

			var rules = new Dictionary<string, char>();
			int i = 0;

			foreach (string line in InputParser.Parse(inputFile, x => x)) {
				if (i < 2) {
					i++;
					continue;
				}

				var rule = line.Split(" -> ");
				rules.Add(rule[0], rule[1][0]);
			}

			Part1(template, rules);
			Part2(template, rules);
		}

		private static void Part1(string template, Dictionary<string, char> rules) {
			var after = insert(template, rules, 10);


			var counts = new Dictionary<char, int>();
			foreach (char c in after) {
				if(!counts.ContainsKey(c)) {
					counts.Add(c, 0);
				}

				counts[c]++;
			}

			Console.WriteLine($"Part 1: {counts.Values.Max()} - {counts.Values.Min()} = {counts.Values.Max() - counts.Values.Min()}");
		}

		private static void Part2(string template, Dictionary<string, char> rules) {
			var after = insert(template, rules, 40);


			var counts = new Dictionary<char, long>();
			foreach (char c in after) {
				if(!counts.ContainsKey(c)) {
					counts.Add(c, 0);
				}

				counts[c]++;
			}

			Console.WriteLine($"Part 2: {counts.Values.Max()} - {counts.Values.Min()} = {counts.Values.Max() - counts.Values.Min()}");
		}

		private static string insert(string template, Dictionary<string, char> rules, int steps) {
			string output = "";
			for (int step = 0; step < steps; step++) {
				Stopwatch clock = Stopwatch.StartNew();
				for (int i = 0; true; i++) {
					if (i >= template.Count()) {
						break;
					}

					output += template[i];

					if (i + 1 >= template.Count()) {
						break;
					}

					var check = template.Substring(i, 2);
					if (rules.ContainsKey(check)) {
						output += rules[check];
					}
				}

				template = output;
				output = "";

				clock.Stop();
				Console.WriteLine($"After Step {step+1}: {template.Count()} {clock.Elapsed}");
			}

			return template;
		}

		private static void Part2() {
		}
	}
}