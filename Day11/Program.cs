namespace AdventOfCode.Day11
{
	public static class Day11 {
		public static void Main() {
			Part1(InputParser.Parse("./input.real.txt", x => x.Select(n => int.Parse(n.ToString())).ToList()).ToList());
			Part2(InputParser.Parse("./input.real.txt", x => x.Select(n => int.Parse(n.ToString())).ToList()).ToList());
		}

		public static void Part1(List<List<int>> data) {
			var sum = 0;
			for (int i = 0; i < 100; i++) {
				int[][]flashes = new int[data.Count()][];
				for (int row = 0; row < data.Count(); row++) {
					flashes[row] = new int[data[row].Count()];
				}

				for (int row = 0; row < data.Count(); row++) {
					for (int col = 0; col < data.Count(); col++) {
						mark(data, flashes, row, col);
					}
				}

				sum += flashes.Select(x => x.Sum()).Sum();
			}

			Console.WriteLine($"Part 1: {sum}");
		}

		public static void Part2(List<List<int>> data) {
			var seek = data.Count() * data[0].Count();

			int i = 0;
			while (true) {
				i++;
				int[][]flashes = new int[data.Count()][];
				for (int row = 0; row < data.Count(); row++) {
					flashes[row] = new int[data[row].Count()];
				}

				for (int row = 0; row < data.Count(); row++) {
					for (int col = 0; col < data.Count(); col++) {
						mark(data, flashes, row, col);
					}
				}

				if (flashes.Select(x => x.Sum()).Sum() == seek) {
					Console.WriteLine($"Part 2: {i}");
					return;
				};
			}
		}

		public static void mark(List<List<int>> data, int[][] flashes, int row, int col) {
			if (flashes[row][col] > 0) {
				data[row][col] = 0;
				return;
			}

			data[row][col]++;

			if (data[row][col] < 10) {
				return;
			}

			int r = data.Count();
			int c = data[row].Count();

			flashes[row][col] = 1;

			// -1,-1 -1,-0 -1,+1
			// +0,-1 +0,-0 +0,+1
			// +1,-1 +1,-0 +1,+1

			if (row > 0 && col > 0)     { mark(data, flashes, row-1, col-1); }
			if (row > 0)                { mark(data, flashes, row-1, col); }
			if (row > 0 && col+1 < c)   { mark(data, flashes, row-1, col+1); }

			if (col > 0)                { mark(data, flashes, row, col-1); }
			// SELF POSITION
			if (col+1 < c)              { mark(data, flashes, row, col+1); }

			if (row+1 < r && col > 0)   { mark(data, flashes, row+1, col-1); }
			if (row+1 < r)              { mark(data, flashes, row+1, col); }
			if (row+1 < r && col+1 < c) { mark(data, flashes, row+1, col+1); }
			data[row][col] = 0;
		}

		public static void print(List<List<int>> data) {
			for (int row = 0; row < data.Count(); row++) {
				for (int col = 0; col < data.Count(); col++) {
					Console.Write($"{data[row][col], 3}");
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		public static void print(List<List<int>> data, int[][] flashes) {
			for (int row = 0; row < data.Count(); row++) {
				for (int col = 0; col < data.Count(); col++) {
					if (flashes[row][col] == 1) {
						Console.Write($"{data[row][col], 2}*");
					} else {
						Console.Write($"{data[row][col], 2} ");
					}
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}
	}
}