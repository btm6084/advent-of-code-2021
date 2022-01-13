namespace AdventOfCode.Day25
{
	public static class Day25 {
		public static void Main() {
			Part1();
			// Part2();
		}

		public static void Part1() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();

			char[][] grid = new char[inputs.Count()][];
			for (int i = 0; i < inputs.Count(); i++) {
				grid[i] = inputs[i].ToCharArray();
			}

			Console.WriteLine("Initial State");
			printGrid(grid);


			var step = 0;
			while (true) {
			// for (int step = 1; step <= 1; step++) {

				step++;
				bool[][] moved = new bool[grid.Count()][];
				for (int row = 0; row < grid.Count(); row++) {
					moved[row] = new bool[grid[row].Count()];
				}

				// East
				Dictionary<(int row, int col), (int row, int col)> east = new();
				for (int row = 0; row < grid.Count(); row++) {
					for (int col = 0; col < grid[row].Count(); col++) {
						var c = grid[row][col];
						if (c != '>') { continue; }
						if (moved[row][col]) { continue; }

						var next = col+1;
						if (next >= grid[row].Count()) { next = 0; }

						if(grid[row][next] == '.') {
							east.Add((row: row, col: col), (row: row, col: next));
						}
					}
				}

				foreach (KeyValuePair<(int row, int col), (int row, int col)> m in east) {
					grid[m.Key.row][m.Key.col] = '.';
					grid[m.Value.row][m.Value.col] = '>';
				}

				// South
				Dictionary<(int row, int col), (int row, int col)> south = new();
				for (int row = 0; row < grid.Count(); row++) {
					for (int col = 0; col < grid[row].Count(); col++) {
						var c = grid[row][col];
						if (c != 'v') { continue; }
						if (moved[row][col]) { continue; }

						var next = row+1;
						if (next >= grid.Count()) { next = 0; }

						if(grid[next][col] == '.') {
							south.Add((row: row, col: col), (row: next, col: col));
						}
					}
				}

				foreach (KeyValuePair<(int row, int col), (int row, int col)> m in south) {
					grid[m.Key.row][m.Key.col] = '.';
					grid[m.Value.row][m.Value.col] = 'v';
				}

				Console.WriteLine($"Step {step}");
				// printGrid(grid);

				if (east.Count() == 0 && south.Count() == 0) {
					break;
				}
			}

			printGrid(grid);

		}

		public static void printGrid(char[][] grid) {
			for (int row = 0; row < grid.Count(); row++) {
				for (int col = 0; col < grid[row].Count(); col++) {
					Console.Write($"{grid[row][col]}");
				}
				Console.WriteLine();
			}
			Console.WriteLine();
		}

		public static void Part2() {
			string[] inputs = InputParser.Parse("./input.example.txt", x => x).ToArray();
		}
	}
}
