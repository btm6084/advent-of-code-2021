public class Day8 {
	public static void Main() {
		List<Signal> signals = new List<Signal>();

		using var inputFile = new System.IO.StreamReader(@"./input.real.txt");
		string? line;
		while ((line = inputFile.ReadLine()) != null)
		{
			signals.Add(new Signal(line));
		}

		Part1(signals);
		Part2(signals);
	}

	public static void Part1(List<Signal> signals) {
		Console.WriteLine($"Part 1: {signals.Sum(x => x.CountOutputUniques())} unique values in output.");
	}
	public static void Part2(List<Signal> signals) {
		var sum = 0;
		foreach (Signal s in signals) {
			sum+=s.Value;
		}
		Console.WriteLine($"Part 2: Sum: {sum}");
	}
};

public class Signal {
	string[] SignalSet;
	string[] Output;
	public int[] Outputs;

	Dictionary<char, char> Positions;
	Dictionary<string, int> SignalMap;

	public int Value;

	public Signal(string raw) {
		SignalSet = raw.Split(" | ").First().Split(" ").ToArray();
		Output = raw.Split(" | ").Last().Split(" ").ToArray();
		Outputs = new int[Output.Count()];
		Positions = new Dictionary<char, char>();
		var Inputs = new List<string>[8];

		for (int i = 0; i < 8; i++) {
			Inputs[i] = new List<string>();
		}

		SignalSet.ToList().ForEach(x => Inputs[x.Count()].Add(String.Concat(x.OrderBy(x=>x))));
		Output.ToList().ForEach(x => Inputs[x.Count()].Add(String.Concat(x.OrderBy(x=>x))));

		for (int i = 0; i < 8; i++) {
			Inputs[i] = Inputs[i].Distinct().ToList();
		}

		SignalMap = new Dictionary<string, int> {
			{"012456", 0},
			{"25", 1},
			{"02346", 2},
			{"02356", 3},
			{"1235", 4},
			{"01356", 5},
			{"013456", 6},
			{"025", 7},
			{"0123456", 8},
			{"012356", 9},
		};

		Dictionary<char, bool> seen = new Dictionary<char, bool>{
			{'a', false},
			{'b', false},
			{'c', false},
			{'d', false},
			{'e', false},
			{'f', false},
			{'g', false},
		};

		// 0 can't exist in 1
		if (Inputs[3].Count() > 0 && Inputs[2].Count() > 0) {
			for (int i = 0; i < Inputs[3][0].Count(); i++) {
				var c = Inputs[3][0][i];
				if (existsAny(c, Inputs[2])) {
					continue;
				}

				Positions[c] = '0';
				break;
			}
		}

		// One is the only len(2)
		var one = Inputs[2][0];

		// Three is unique among the len(5) in that it is the only one that contains both wires from one.
		var three = FindThree(one, Inputs[5]);

		// If we remove the overlap between three and four, we're left with one wire in 4.
		var four = Inputs[4][0];
		var fourThreeDiff = four.Where(x => !three.Contains(x)).First();
		Positions[fourThreeDiff] = '1';

		// Five is the only len(5) that contains the wire from the 4-3 diff.
		var five = FindFive(fourThreeDiff, Inputs[5]);

		// Two won't contain the wire from the 4-3 diff, and won't be 3.
		var two = FindTwo(fourThreeDiff, three, Inputs[5]);

		// Position 2 is the wire in one that isn't in five.
		var fiveOneDiff = one.Where(x => !five.Contains(x)).First();
		Positions[fiveOneDiff] = '2';

		// Position 5 is the wire that's left over from five-one diff.
		var lastOne = one.Where(x => x != fiveOneDiff).First();
		Positions[lastOne] = '5';

		// Position 3 exists in 4, but hasn't been set yet
		for (int i = 0; i < four.Count(); i++) {
			if (!Positions.ContainsKey(four[i])) {
				Positions[four[i]] = '3';
				break;
			}
		}

		// Position 6 is the wire in five but not seen.
		for (int i = 0; i < five.Count(); i++) {
			if (!Positions.ContainsKey(five[i])) {
				Positions[five[i]] = '6';
				break;
			}
		}

		// Position 4 is the diff between what's seen, and two.
		for (int i = 0; i < two.Count(); i++) {
			if (!Positions.ContainsKey(two[i])) {
				Positions[two[i]] = '4';
				break;
			}
		}

		string[] translate = new string[Output.Count()];
		string values = "";
		for (int i = 0; i < Output.Count(); i++) {

			foreach(char c in Output[i]) {
				translate[i] += Positions[c];
			}

			translate[i] = String.Concat(translate[i].OrderBy(c => c));

			values += SignalMap[translate[i]];
		}

		Value = int.Parse(values);
	}

	public int CountOutputUniques() {
		return Output.Where(s => isOneFourSevenEight(s.Count())).ToArray().Count();
	}

	private bool isOneFourSevenEight(int c) {
		return c == 2 || c == 3 || c == 4 || c == 7;
	}

	private bool existsAny(char seek, List<string> search) {
		foreach (string line in search) {
			if (line.Contains(seek)) {
				return true;
			}
		}

		return false;
	}

	private string FindThree(string one, List<string> search) {
		List<string> filtered = new List<string>();
		for (int i = 0; i < search.Count(); i++) {
			if (search[i].Contains(one[0]) && search[i].Contains(one[1])) {
				return search[i];
			}
		}
		return "";
	}

	private string FindFive(char one, List<string> search) {
		List<string> filtered = new List<string>();
		for (int i = 0; i < search.Count(); i++) {
			if (search[i].Contains(one)) {
				return search[i];
			}
		}
		return "";
	}

	private string FindTwo(char one, string three, List<string> search) {
		List<string> filtered = new List<string>();
		for (int i = 0; i < search.Count(); i++) {
			if (!search[i].Contains(one) && search[i] != three) {
				return search[i];
			}
		}
		return "";
	}

}

