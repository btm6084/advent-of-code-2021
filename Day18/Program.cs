namespace AdventOfCode.Day18
{
	public static class Day18 {
		public static void Main() {
			Part1();
			Part2();
		}

		public static void Part1() {
			string[] terms = InputParser.Parse("./input.real.txt", x => x).ToArray();

			var term = terms[0];

			for (int i = 1; i < terms.Count(); i++) {
				term = AddAndReduce(term, terms[i]);
			}

			Console.WriteLine($"Term: {term} with Magnitude: {Magnitude(term)}");
		}

		public static void Part2() {
			List<long> mags = new();

			string[] terms = InputParser.Parse("./input.real.txt", x => x).ToArray();

			for (int i = 0; i < terms.Count(); i++) {
				for (int j = 0; j < terms.Count(); j++) {
					if (i == j) {
						continue;
					}

					mags.Add(Magnitude(AddAndReduce(terms[i], terms[j])));
				}
			}

			Console.WriteLine($"Max Magnitude: {mags.Max()}");
		}

		public static string AddAndReduce(string term, string add) {
			term = Add(term, add);

			while(true) {
				string check = term;

				var et = Explode(term);
				if (et != term) {
					term = et;
					continue;
				}

				term = et;

				var st = Split(term);
				if (st != term) {
					term = st;
					continue;
				}

				term = st;

				if (term == check) {
					break;
				}
			}

			return term;
		}

		public static string Add(string a, string b) {
			return $"[{a},{b}]";
		}

		private static (int lhs, int rhs, int bytes) numericPair(string term, int pos) {
			if (pos >= term.Length) {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			var start = pos;

			// TO be numeric pair, must not have a nested child as either side.
			var num = "";
			while (pos < term.Length && isNumeric(term[pos])) {
				num += term[pos];
				pos++;
			}

			if (num.Length == 0) {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			if (pos >= term.Length) {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			if (term[pos] != ',') {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			var lhs = int.Parse(num);

			pos++;

			if (pos >= term.Length) {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			num = "";
			while (isNumeric(term[pos])) {
				num += term[pos];
				pos++;
			}

			if (num.Length == 0) {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			if (term[pos] != ']') {
				return (lhs: -1, rhs: -1, bytes: 0);
			}

			var rhs = int.Parse(num);

			return (lhs: lhs, rhs: rhs, bytes: pos-start);
		}

		public static string Explode(string term) {
			int open = 0;
			string output = "";

			for (int i = 0; i < term.Count(); i++) {
				if (term[i] == '[') {
					open++;
					output += term[i];
					continue;
				}
				if (term[i] == ']') {
					open--;
					output += term[i];
					continue;
				}

				if (open > 4 && isNumeric(term[i])) {

					(int lhs, int rhs, int bytes) p = numericPair(term, i);

					if (p.bytes == 0) {
						output += term[i];
						continue;
					}

					// Add to the left number.
					var stack = new Stack<char>();
					var pos = -1;
					for (int j = output.Length-1; j >= 0; j--) {
						if (isNumeric(output[j])) {
							pos = j;
							break;
						}
						stack.Push(output[j]);
					}

					if (pos >= 0) {
						// Find that number.
						var num = "";
						while(isNumeric(output[pos])) {
							num = output[pos] + num;
							pos--;
						}

						output = output.Substring(0, pos+1);

						int lt = int.Parse(num);
						int nt = lt + p.lhs;
						string bytes = nt.ToString();

						output += bytes;

						int count = stack.Count();
						for (int j = 0; j < count; j++) {
							output += stack.Pop();
						}
					}

					// Move i past the pair. Back out the previous [ add, as we don't need it now.
					output = output.Remove(output.Length-1, 1);
					output += '0';
					i += p.bytes + 1;

					// Finish the string, adding the right number to the next numeric.
					bool once = false;
					for (; i < term.Length; i++) {
						if (once || !isNumeric(term[i])) {
							output += term[i];
							continue;
						}

						once = true;

						var num = "";
						while (isNumeric(term[i])) {
							num += term[i];
							i++;
						}

						int rt = int.Parse(num);
						int nt = rt + p.rhs;
						string bytes = nt.ToString();

						output += bytes;

						output += term[i];
					}

					return output;
				}

				output += term[i];
			}

			return output;
		}

		public static string Split(string term) {
			string output = "";
			bool once = false;

			for (int i = 0; i < term.Length; i++) {
				if (once || !isNumeric(term[i])) {
					output += term[i];
					continue;
				}

				var num = "";
				while (isNumeric(term[i])) {
					num += term[i];
					i++;
				}

				i--; // Undo the last increment while finding numbers.

				var val = int.Parse(num);
				if (val < 10) {
					output += num;
					continue;
				}

				once = true;

				var lhs = val / 2;
				var rhs = val % 2 == 1 ? lhs + 1 : lhs;

				output += $"[{lhs},{rhs}]";
			}

			return output;
		}



		private static long Magnitude(string term) {
			return long.Parse(reduce(term));
		}

		private static string reduce(string term) {
			while (true) {
				string output = "";
				bool reduced = false;

				for (int i = 0; i < term.Length; i++) {
					if (!isNumeric(term[i])) {
						output += term[i];
						continue;
					}

					(int lhs, int rhs, int bytes) p = numericPair(term, i);
					if (p.bytes == 0) {
						output += term[i];
						continue;
					}

					// Remove the last [, add the new value, move i forward.
					output = output.Remove(output.Length-1, 1);
					output += ((p.lhs * 3) + (p.rhs * 2)).ToString();
					i += p.bytes + 1;

					for (; i < term.Length; i++) {
						output += term[i];
					}

					reduced = true;
					term = output;
					break;
				}

				if (!reduced) {
					return output;
				}
			}
		}

		private static bool isNumeric(char n) {
			return n == '0' || n == '1' || n == '2' || n == '3' || n == '4' || n == '5' || n == '6' || n == '7' || n == '8' || n == '9';
		}
	}


}

