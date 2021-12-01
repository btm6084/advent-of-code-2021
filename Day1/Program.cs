void Part1()
{
	int count = 0;
	int prev = -1;

	foreach (int depth in ReadInput("./input.txt")) {
		if (prev > 0 && depth > prev) {
			count++;
		}
		prev = depth;
	}

	Console.WriteLine(String.Format("Part 1: Depth Increased {0} times", count));
}

void Part2() {
	List<int> input = new List<int>();

	// Load input.
	foreach (int depth in ReadInput("./input.txt")) {
		input.Add(depth);
	}

	int prev = -1;
	int count = 0;
	for (int i = 0; i < input.Count - 2; i++) {
		List<int> window = input.GetRange(i, 3);
		int sum = window.Sum();
		if (prev > 0 && sum > prev) {
			count++;
		}

		prev = sum;
	}

	Console.WriteLine(String.Format("Part 2: Depth Increased {0} times", count));
}

IEnumerable<int> ReadInput(string filename) {
	int val = 0;
	foreach(string line in File.ReadLines(filename)){
		if (int.TryParse(line, out val)) {
			yield return val;
		}
	}
}

Part1();
Part2();