public class Day6 {
	public static void Main() {
		Day6 d = new Day6();

		System.IO.StreamReader inputFile = new System.IO.StreamReader(@"./input.real.txt");
		string? line;
		int[] fish = new int[0];
		if ((line = inputFile.ReadLine()) != null) {
			fish = Array.ConvertAll(line.Split(","), s => int.Parse(s));
		}

		d.Part1(fish);
		d.Part2(fish);
	}

	private List<int[]> LoadFish(System.IO.StreamReader inputFile) {
		List<int[]> lines = new List<int[]>();

		string? line;
		while ((line = inputFile.ReadLine()) != null) {
			lines.Add(Array.ConvertAll(line.Replace(" -> ", ",").Split(","), s => int.Parse(s)));
		}

		return lines;
	}

	public void Part1(int[] initialFish) {
		int numStates = 9;
		int[] fish = new int[numStates];


		foreach (int f in initialFish) {
			fish[f]++;
		}

		for (int day = 0; day < 80; day++) {
			int[] newDay = new int[numStates];

			newDay[8] += fish[0];
			newDay[6] += fish[0];

			for (int i = 1; i < numStates; i++) {
				newDay[i-1] += fish[i];
			}

			fish = newDay;
		}

		Console.WriteLine($"Part 1: {fish.Sum()}");
	}

	public void Part2(int[] initialFish) {
		int numStates = 9;
		long[] fish = new long[numStates];


		foreach (int f in initialFish) {
			fish[f]++;
		}

		for (int day = 0; day < 256; day++) {
			long[] newDay = new long[numStates];

			newDay[8] += fish[0];
			newDay[6] += fish[0];

			for (int i = 1; i < numStates; i++) {
				newDay[i-1] += fish[i];
			}

			fish = newDay;
		}

		Console.WriteLine($"Part 2: {fish.Sum()}");
	}
};