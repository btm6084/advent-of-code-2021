namespace AdventOfCode.Day20
{
	public static class Day20 {
		private static int exp = 1;

		public static void Main() {
			Part1();
			// Part2();
		}

		public static void Part1() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();
			string algRaw = inputs[0];

			int[] alg = algRaw.Select(x => x == '#' ? 1 : 0).ToArray();

			List<List<int>> img = new();
			for (int i = 2; i < inputs.Count(); i++) {
				List<int> line = new();

				foreach(char c in inputs[i]) {
					line.Add(c == '#' ? 1 : 0);
				}

				img.Add(line);
			}

			// PrintAlg(alg);
			// PrintImg(img);

			int iterations = 50;

			int inf = 0;

			for (int i = 0; i < iterations; i++) {
				Console.WriteLine($"{i+1}: [{inf}]");
				img = Iterate(alg, img, inf);
				// PrintImg(img);

				// The infinite field never swaps.
				if (alg[0] == 0) {
					continue;
				}

				if (i % 2 == 0) {
					inf = alg[0];
				} else {
					inf = alg.Last();
				}
			}

			PrintImg(img, 0);
			Console.WriteLine($"Pixel Count: {CountPixels(img, 0)}");
		}

		public static void PrintAlg(int[] alg) {
			for (int i = 0; i < alg.Count(); i++) {
				Console.Write($"{alg[i]}");
			}

			Console.WriteLine();
			Console.WriteLine();
		}

		public static void PrintImg(List<List<int>> img) {
			for (int row = 0; row < img.Count(); row++) {
				for (int col = 0; col < img[row].Count(); col++) {
					Console.Write($"{(img[row][col] == 1 ? '#' : '.' )}");
				}
				Console.WriteLine();
			}
			Console.WriteLine($"{img.Count()}x{img[0].Count()}");
			Console.WriteLine();
		}

		public static void PrintImg(List<List<int>> img, int cut) {
			for (int row = cut; row < img.Count()-cut; row++) {
				for (int col = cut; col < img[row].Count()-cut; col++) {
					Console.Write($"{(img[row][col] == 1 ? '#' : '.' )}");
				}
				Console.WriteLine();
			}
			Console.WriteLine($"{img.Count()}x{img[0].Count()}");
			Console.WriteLine();
		}

		public static int CountPixels(List<List<int>> img, int cut) {
			int sum = 0;
			for (int row = cut; row < img.Count()-cut; row++) {
				for (int col = cut; col < img[row].Count()-cut; col++) {
					sum += img[row][col];
				}
			}
			return sum;

		}

		public static List<List<int>> Iterate(int[] alg, List<List<int>> img, int inf) {
			List<List<int>> newImg = new();

			for (int row = -exp; row < img.Count()+exp; row++) {
				List<int> line = new();

				for (int col = -exp; col < img[0].Count()+exp; col++) {
					string bin = "";

					// -1,-1
					bin += binVal(row-1, col-1, img, inf);
					// -1, 0
					bin += binVal(row-1, col, img, inf);
					// -1,+1
					bin += binVal(row-1, col+1, img, inf);
					//  0,-1
					bin += binVal(row, col-1, img, inf);
					//  0, 0
					bin += binVal(row, col, img, inf);
					//  0,+1
					bin += binVal(row, col+1, img, inf);
					// +1,-1
					bin += binVal(row+1, col-1, img, inf);
					// +1, 0
					bin += binVal(row+1, col, img, inf);
					// +1,+1
					bin += binVal(row+1, col+1, img, inf);

					int intVal = Convert.ToInt32(bin, 2);

					// Console.WriteLine($"[{row},{col}]: {bin} [{intVal}] [{alg[intVal]}]");

					line.Add(alg[intVal]);
				}

				newImg.Add(line);
			}

			return newImg;
		}

		public static char binVal(int row, int col, List<List<int>> img, int inf) {
			if (row < 0) { return inf.ToString()[0]; }
			if (row >= img.Count()) { return inf.ToString()[0]; }
			if (col < 0) { return inf.ToString()[0]; }
			if (col >= img[row].Count()) { return inf.ToString()[0]; }

			return img[row][col].ToString()[0];
		}
	}
}
