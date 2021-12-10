namespace AdventOfCode.Day10
{
	public static class Day10
	{
		static Dictionary<char, char> closeMap = new Dictionary<char, char>{
			{'(', ')'},
			{'{', '}'},
			{'[', ']'},
			{'<', '>'},
		};
		static Dictionary<char, bool> openers = new Dictionary<char, bool>{
			{'(', true},
			{'{', true},
			{'[', true},
			{'<', true},
		};
		static Dictionary<char, bool> closers = new Dictionary<char, bool>{
			{')', true},
			{'}', true},
			{']', true},
			{'>', true},
		};
		static Dictionary<char, int> costMap = new Dictionary<char, int>{
			{')',     3},
			{']',    57},
			{'}',  1197},
			{'>', 25137},
		};
		static Dictionary<char, int> costMap2 = new Dictionary<char, int>{
			{')', 1},
			{']', 2},
			{'}', 3},
			{'>', 4},
		};

		public static void Main() {
			var data = InputParser.Parse("./input.real.txt", x => x);
			Part1(data.Where(x => isCorrupt(x)));
			Part2(data.Where(x => !isCorrupt(x)));
		}

		public static void Part1(IEnumerable<string> data) {
			var corrupt = new List<char>();
			data.ToList().ForEach(x => corrupt.Add(Day10.corruptCharacter(x)));
			Console.WriteLine($"Part 1: Cost: {corrupt.Where(x => x != '0').Select(x => costMap[x]).Sum()}");
		}

		public static void Part2(IEnumerable<string> incomplete) {
			var repairs = new List<string>();
			var repairCosts = new List<long>();
			incomplete.ToList().ForEach(x => repairs.Add(repair(x)));
			repairs.ForEach(x => repairCosts.Add(repairCost(x)));
			var middle = repairCosts.OrderBy(n=>n).ElementAt((repairCosts.Count() / 2));

			Console.WriteLine($"Part 2: Cost: {middle}");
		}

		public static char corruptCharacter(string search) {
			var stack = new Stack<char>();
			foreach (char c in search) {
				if (closers.ContainsKey(c)) {
					if (stack.Count() == 0) {
						return c;
					}
					if (c != closeMap[stack.Peek()]) {
						return c;
					}

					stack.Pop();
				}

				if (openers.ContainsKey(c)) {
					stack.Push(c);
				}
			}

			return '0';
		}

		public static bool isCorrupt(string search) {
			var stack = new Stack<char>();
			foreach (char c in search) {
				if (closers.ContainsKey(c)) {
					if (stack.Count() == 0) {
						return true;
					}
					if (c != closeMap[stack.Peek()]) {
						return true;
					}

					stack.Pop();
				}

				if (openers.ContainsKey(c)) {
					stack.Push(c);
				}
			}

			return false;
		}

		public static string repair(string search) {
			var stack = new Stack<char>();
			foreach (char c in search) {
				if (closers.ContainsKey(c)) {
					stack.Pop();
				}

				if (openers.ContainsKey(c)) {
					stack.Push(c);
				}
			}

			return String.Concat(stack.Select(x => closeMap[x]));
		}

		public static long repairCost(string repairString) {
			long sum = 0;
			foreach (char c in repairString) {
				sum *= 5;
				sum += costMap2[c];
			}

			return sum;
		}
	}
}