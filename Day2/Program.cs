using System.Text.RegularExpressions;

IEnumerable<string> ReadInput(string filename) {
	foreach(string line in File.ReadLines(filename)){
		yield return line;
	}
}

void Part1() {
	Regex inst = new Regex(@"([A-Za-z]+) ([0-9]+)");
	int h = 0;
	int d = 0;

	string pattern = @"^(forward|down|up) ([0-9]+)";

	foreach (string data in ReadInput("./input.real.txt")) {
		Match match = Regex.Match(data, pattern);

		if (!match.Success) {
			Console.WriteLine(String.Format("Invalid Instruction: {0}", data));
			return;
		}

		string dir = match.Groups[1].Value;
		int val = 0;

		if (!int.TryParse(match.Groups[2].Value, out val)) {
			Console.WriteLine(String.Format("Failed to Convert Int: {0}", match.Groups[2].Value));
			return;
		}

		switch (dir) {
			case "forward":
				h += val;
				break;
			case "down":
				d += val;
				break;
			case "up":
				d -= val;
				break;
			default:
				Console.WriteLine(String.Format("Invalid Direction: {0}", dir));
				return;
		}
	}

	Console.WriteLine(String.Format("Part 1: Position: [{0},{1}] Value: {2}", h, d, h*d));
}

void Part2() {
	Regex inst = new Regex(@"([A-Za-z]+) ([0-9]+)");
	int h = 0;
	int a = 0;
	int d = 0;

	string pattern = @"^(forward|down|up) ([0-9]+)";

	foreach (string data in ReadInput("./input.real.txt")) {
		Match match = Regex.Match(data, pattern);

		if (!match.Success) {
			Console.WriteLine(String.Format("Invalid Instruction: {0}", data));
			return;
		}

		string dir = match.Groups[1].Value;
		int val = 0;

		if (!int.TryParse(match.Groups[2].Value, out val)) {
			Console.WriteLine(String.Format("Failed to Convert Int: {0}", match.Groups[2].Value));
			return;
		}

		switch (dir) {
			case "forward":
				h += val;
				d += val * a;
				break;
			case "down":
				a += val;
				break;
			case "up":
				a -= val;
				break;
			default:
				Console.WriteLine(String.Format("Invalid Direction: {0}", dir));
				return;
		}
	}

	Console.WriteLine(String.Format("Part 2: Position: [{0},{1}] Value: {2}", h, d, h*d));
}

Part1();
Part2();