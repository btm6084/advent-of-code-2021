public class Day5 {
	public static void Main() {
		Day5 d = new Day5();

		System.IO.StreamReader inputFile = new System.IO.StreamReader(@"./input.real.txt");
		List<int[]> lines = d.LoadLines(inputFile);

		d.Part1(lines);
		d.Part2(lines);
	}

	private List<int[]> LoadLines(System.IO.StreamReader inputFile) {
		List<int[]> lines = new List<int[]>();

		string? line;
		while ((line = inputFile.ReadLine()) != null) {
			lines.Add(Array.ConvertAll(line.Replace(" -> ", ",").Split(","), s => int.Parse(s)));
		}

		return lines;
	}

	public void Part1(List<int[]> lines) {
		Dictionary<string, int> overlaps = new Dictionary<string, int>();

		foreach (int[] points in lines) {
			int x1 = points[0];
			int y1 = points[1];
			int x2 = points[2];
			int y2 = points[3];

			int xSlope = x1 - x2;
			int ySlope = y1 - y2;

			if (xSlope != 0 && ySlope != 0) {
				continue;
			}

			if (xSlope != 0) {
				int xDir = xSlope < 0 ? 1 : -1;
				for (int i = 0; i < Math.Abs(xSlope)+1; i++) {
					string key = $"{x1+(i*xDir)},{y1}";

					int current;
					overlaps.TryGetValue(key, out current);
					overlaps[key] = current+1;
				}
			}

			if (ySlope != 0) {
				int yDir = ySlope < 0 ? 1 : -1;
				for (int i = 0; i < Math.Abs(ySlope)+1; i++) {
					string key = $"{x1},{y1+(i*yDir)}";

					int current;
					overlaps.TryGetValue(key, out current);
					overlaps[key] = current+1;
				}
			}
		}

		// overlaps.Where(p => p.Value > 1).Count() for Salty
		Console.WriteLine($"Part 1: {overlaps.Where(p => p.Value > 1).Count()} overlaps");
	}

	public void Part2(List<int[]> lines) {
		Dictionary<string, int> overlaps = new Dictionary<string, int>();

		foreach (int[] points in lines) {
			int x1 = points[0];
			int y1 = points[1];
			int x2 = points[2];
			int y2 = points[3];

			int xSlope = x1 - x2;
			int ySlope = y1 - y2;
			int xDir = xSlope < 0 ? 1 : -1;
			int yDir = ySlope < 0 ? 1 : -1;

			if (Math.Abs(xSlope) == Math.Abs(ySlope)) {
				for (int i = 0; i < Math.Abs(xSlope)+1; i++) {
					string key = $"{x1+(i*xDir)},{y1+(i*yDir)}";

					int current;
					overlaps.TryGetValue(key, out current);
					overlaps[key] = current+1;
				}
				continue;
			}


			if (xSlope != 0) {
				for (int i = 0; i < Math.Abs(xSlope)+1; i++) {
					string key = $"{x1+(i*xDir)},{y1}";

					int current;
					overlaps.TryGetValue(key, out current);
					overlaps[key] = current+1;
				}

				continue;
			}

			if (ySlope != 0) {
				for (int i = 0; i < Math.Abs(ySlope)+1; i++) {
					string key = $"{x1},{y1+(i*yDir)}";

					int current;
					overlaps.TryGetValue(key, out current);
					overlaps[key] = current+1;
				}

				continue;
			}
		}

		// overlaps.Where(p => p.Value > 1).Count() for Salty
		Console.WriteLine($"Part 2: {overlaps.Where(p => p.Value > 1).Count()} overlaps");
	}
};