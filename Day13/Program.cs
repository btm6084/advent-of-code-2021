namespace AdventOfCode.Day13
{
	public static class Day13 {
		public static void Main() {
			var dots = new List<Dot>();
			var folds = new List<Fold>();

			foreach (string line in InputParser.Parse("./input.real.txt", x => x)) {
				if (line == "") {
					continue;
				}

				if (line[0] == 'f') {
					folds.Add(new Fold(line));
					continue;
				}

				dots.Add(new Dot(line));
			}

			Part1(dots, folds);
			Part2(dots, folds);
		}

		private static void Part1(List<Dot> dots, List<Fold> folds) {
			var p = new Paper(dots, folds);
			p.FirstFold();

			Console.WriteLine($"Part 1: {p.Sum()}");
		}

		private static void Part2(List<Dot> dots, List<Fold> folds) {
			var p = new Paper(dots, folds);
			p.Fold();
			p.Print();
		}

		public class Paper {
			List<Dot> dots;
			List<Fold> folds;

			private int[][]? paper;

			public Paper(List<Dot> d, List<Fold> f) {
				dots = d;
				folds = f;

				var mx = dots.MaxBy(x => x.X);
				var my = dots.MaxBy(y => y.Y);

				if (mx == null || my == null) {
					Console.WriteLine("MaxX or MaxY null");
					return;
				}

				paper = new int[my.Y+1][];
				for (int i = 0; i < my.Y+1; i++) {
					paper[i] = new int[mx.X+1];
				}

				foreach (Dot dot in dots) {
					paper[dot.Y][dot.X] = 1;
				}
			}

			public void FirstFold() {
				if (folds[0].dir == 'y') {
					foldVertical(folds[0].val);
				} else {
					foldHorizontal(folds[0].val);
				}
			}
			public void Fold() {
				foreach (Fold fold in folds) {
					if (fold.dir == 'y') {
						foldVertical(fold.val);
					} else {
						foldHorizontal(fold.val);
					}
				}
			}

			private void foldVertical(int pos) {
				if (paper == null) {
					return;
				}

				var half = paper.Count()/2;
				var rows = (pos > half ? (paper.Count() - 1 - pos) : pos);

				// Rows: 6 Fold: 7
				// i: 1 [8,0]  => [6,0]
				// i: 2 [9,0]  => [5,0]
				// i: 3 [10,0] => [4,0]
				// i: 4 [11,0] => [3,0]
				// i: 5 [12,0] => [2,0]
				// i: 6 [13,0] => [1,0]
				// i: 7 [14,0] => [0,0]

				for (int col = 0; col < paper[0].Count(); col++) {
					for (int i = 1; i <= rows; i++) {
						var a = paper[pos-i][col];
						var b = paper[pos+i][col];

						paper[pos-i][col] = paper[pos+i][col] | paper[pos-i][col];
					}
				}

				paper = paper.Take((paper.Count()-1)-rows).ToArray();
			}

			private void foldHorizontal(int pos) {
				if (paper == null) {
					return;
				}

				var half = paper[0].Count()/2;
				var rows = (pos > half ? (paper[0].Count() - 1 - pos) : pos);

				// Console.WriteLine($"Count: {paper[0].Count()} Half: {half} Pos: {pos} Rows: {rows}");

				// Rows: 5 Fold: 5
				// i: 1 [0,8 ] => [0, 6]
				// i: 2 [0,9 ] => [0, 5]
				// i: 3 [0,10] => [0, 4]
				// i: 4 [0,11] => [0, 3]
				// i: 5 [0,12] => [0, 2]
				// i: 6 [0,13] => [0, 1]
				// i: 7 [0,14] => [0, 0]

				for (int row = 0; row < paper.Count(); row++) {
					for (int i = 1; i <= rows; i++) {
						var aCol = pos - i;
						var bCol = pos + i;

						var a = paper[row][aCol];
						var b = paper[row][bCol];

						paper[row][aCol] = paper[row][bCol] | paper[row][aCol];
					}
					// Console.WriteLine();
				}

				for (int row = 0; row < paper.Count(); row++) {
					paper[row] = paper[row].Take((paper[row].Count()-1)-rows).ToArray();
				}
			}

			public void Print() {
				if (paper == null) {
					return;
				}

				for (int row = 0; row < paper.Count(); row++) {
					for (int col = 0; col < paper[row].Count(); col++) {
						if (paper[row][col] == 1) {
							Console.Write($"#");
						} else {
							Console.Write($".");
						}
					}
					Console.WriteLine();
				}
			}

			public int Sum() {
				if (paper == null) {
					return 0;
				}

				return paper.Select(x => x.Sum()).Sum();
			}
		}

		public class Dot {
			public int X;
			public int Y;

			public Dot(string raw) {
				var pieces = raw.Split(",");
				X = int.Parse(pieces[0]);
				Y = int.Parse(pieces[1]);
			}
		}

		public class Fold {
			public char dir;
			public int val;

			public Fold(string raw) {
				var pieces = raw.Replace("fold along ", "").Split("=");
				dir = pieces[0][0];
				val = int.Parse(pieces[1]);
			}
		}
	}
}