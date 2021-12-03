void Part1() {

	int lineCount = 0;

	IEnumerable<string> f = System.IO.File.ReadLines(@"./input.real.txt");
	int[] sum = new int[f.First().Count()];

	// Sum each column in the input.
	foreach (string line in f)
	{
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

	Console.WriteLine($"Gamma: {gamma} Epsilon: {epsilon} Product: {gamma * epsilon}");
}

Part1();