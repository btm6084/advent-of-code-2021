void Part1() {

	int lineCount = 0;

	IEnumerable<string> f = System.IO.File.ReadLines(@"./input.real.txt");
	int[] sum = new int[f.First().Count()];

	// Sum each column in the input.
	foreach (string line in f) {
		lineCount++;
		for (int i = 0; i < line.Count(); i++) {
			if (line[i] == '1') {
				sum[i]++;
			}
		}
	}

	int check = lineCount / 2;
	int gamma = 0;
	int epsilon = 0;
	for (int i = sum.Count()-1; i >= 0; i--) {
		int magnitude = (int)Math.Pow(2, ((sum.Count()-1) - i));
		gamma += (sum[i] > check ? 1 : 0) * magnitude;
		epsilon += (sum[i] < check ? 1 : 0) * magnitude;
	}

	Console.WriteLine($"Part 1: Gamma: {gamma} Epsilon: {epsilon} Product: {gamma * epsilon}");
}

void Part2() {

	var inputFile = File.ReadAllLines(@"./input.real.txt");
	var input = new List<string>(inputFile);

	List<string> list = Filter(input, 0, true);

	int i = 1;
	while (list.Count() > 1) {
			list = Filter(list, i, true);
			i++;
	}

	int oxy = BinToInt(list[0]);

	list = Filter(input, 0, false);
	i = 1;
	while (list.Count() > 1) {
			list = Filter(list, i, false);
			i++;
	}

	int c02 = BinToInt(list[0]);

	Console.WriteLine($"Part 2: Oxygen: {oxy} C02: {c02} Product: {oxy * c02}");
}

List<string> Filter(List<string> list, int pos, bool mostCommon) {
	List<string> newList = new List<string>();

	int one = 0;
	int zero = 0;
	foreach (string line in list) {
		if (line[pos] == '1') {
			one++;
		} else {
			zero++;
		}
	}

	char keep = one >= zero ? '1' : '0';
	if (!mostCommon) {
		keep = one >= zero ? '0' : '1';
	}

	foreach (string line in list) {
		if (line[pos] == keep) {
			newList.Add(line);
		}
	}
	return newList;
}

int BinToInt(string line) {
	int sum = 0;

	for (int i = line.Count()-1; i >= 0; i--) {
		int magnitude = (int)Math.Pow(2, ((line.Count()-1) - i));
		int val = 0;
		if (int.TryParse(line[i].ToString(), out val)) {
			sum += magnitude * val;
		};
	}

	return sum;
}

Part1();
Part2();