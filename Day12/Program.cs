namespace AdventOfCode.Day12
{
	public static class Day12 {
		public static void Main() {
			// Part1();
			Part2();
		}

		private static void Part1() {
			var edges = new Dictionary<string,List<string>>();
			var data = InputParser.Parse("./input.real.txt", x => x);

			foreach (string line in data) {
				var s = line.Split("-");

				if (!edges.ContainsKey(s[0])) {
					edges[s[0]] = new List<string>();
				}
				if (s[1] != "end" && !edges.ContainsKey(s[1])) {
					edges[s[1]] = new List<string>();
				}

				edges[s[0]].Add(s[1]);
				if (s[0] != "start" && s[1] != "end") {
					edges[s[1]].Add(s[0]);
				}
			}

			var paths = findPath(edges, new List<string>(), "start", "start");
			Console.WriteLine($"Part 1: {paths.Count()}");
		}

		private static void Part2() {
			var edges = new Dictionary<string,List<string>>();
			var data = InputParser.Parse("./input.real.txt", x => x);

			foreach (string line in data) {
				var s = line.Split("-");

				if (!edges.ContainsKey(s[0])) {
					edges[s[0]] = new List<string>();
				}
				if (s[1] != "end" && !edges.ContainsKey(s[1])) {
					edges[s[1]] = new List<string>();
				}

				edges[s[0]].Add(s[1]);
				if (s[0] != "start" && s[1] != "end") {
					edges[s[1]].Add(s[0]);
				}
			}

			var paths = findPath2(edges, new List<string>(), "start", "start");
			Console.WriteLine($"Part 2: {paths.Count()}");
		}

		private static List<string> findPath(Dictionary<string,List<string>> edges, List<string> paths, string path, string key) {
			if (!edges.ContainsKey(key)) {
				paths.Add(path);
				return paths;
			}

			foreach(string edge in edges[key]) {
				if ((isLower(edge) && path.Contains(edge)) || edge == "start") {
					continue;
				}

				if (edge == "end") {
					paths.Add($"{path},{edge}");
					continue;
				}

				findPath(edges, paths, $"{path},{edge}", edge);
			}

			return paths;
		}

		private static List<string> findPath2(Dictionary<string,List<string>> edges, List<string> paths, string path, string key) {
			if (!edges.ContainsKey(key)) {
				paths.Add(path);
				return paths;
			}

			var mustSkip = false;
			var counts = new Dictionary<string, int>();
			foreach (string s in path.Split(",")) {
				if (!isLower(s) || s == "start" || s == "end") {
					continue;
				}
				if (!counts.ContainsKey(s)) {
					counts.Add(s, 0);
				}
				counts[s]++;
				if (counts[s] > 1) {
					mustSkip = true;
				}
			}

			foreach(string edge in edges[key]) {
				if (edge == "end") {
					paths.Add($"{path},{edge}");
					continue;
				}

				if (edge == "start") {
					continue;
				}

				if (!isLower(edge)) {
					findPath2(edges, paths, $"{path},{edge}", edge);
					continue;
				}

				// Something is already at 2
				if (mustSkip && (counts.ContainsKey(edge) && counts[edge] > 0)) {
					continue;
				}

				findPath2(edges, paths, $"{path},{edge}", edge);
			}

			return paths;
		}

		private static bool isLower(string check) {
			foreach(char c in check) {
				if (!Char.IsLower(c)) {
					return false;
				}
			}

			return true;
		}
	}
}