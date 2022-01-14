namespace AdventOfCode.Day22
{
	public static class Day22 {
		public static void Main() {
			Part1();
			Part2();
		}

		public static void Part1() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();
			var reactor = new Reactor(inputs, true);
		}

		public static void Part2() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();
			var reactor = new Reactor(inputs, false);
		}

		public class Reactor {
			List<Cube> cubes;

			public Reactor(string[] inputs, bool p1) {
				cubes = new();

				for (int i = 0; i < inputs.Length; i++) {
					var pieces = inputs[i].Split(" ");
					var val = pieces[0] == "on" ? 1:0;
					var ranges = pieces[1].Split(",");

					var xRange = ranges[0].Replace("x=", "").Split("..");
					var minX = int.Parse(xRange[0]);
					var maxX = int.Parse(xRange[1]);
					if (p1 && (maxX > 50 || minX < -50)) { continue ; }

					var yRange = ranges[1].Replace("y=", "").Split("..");
					var minY = int.Parse(yRange[0]);
					var maxY = int.Parse(yRange[1]);
					if (p1 && (maxY > 50 || minY < -50)) { continue ; }

					var zRange = ranges[2].Replace("z=", "").Split("..");
					var minZ = int.Parse(zRange[0]);
					var maxZ = int.Parse(zRange[1]);
					if (p1 && (maxZ > 50 || minZ < -50)) { continue ; }

					cubes.Add(new Cube(minX, minY, minZ, maxX, maxY, maxZ, val));
				}

				Console.WriteLine(ScoreGroup(cubes, 0));
			}

			public static ulong ScoreGroup(List<Cube> group, int depth) {
				ulong sum = 0;
				List<Cube> processed = new();

				for (int i = 0; i < group.Count(); i++) {
					var c = group[i];

					List<Cube> overlaps = new();

					foreach(Cube cube in processed) {
						var o = cube.Overlap(c);
						if (o != null) {
							o.val = cube.val;
							overlaps.Add(o);
						}
					}

					if (c.val == 1) { sum += c.volume; }
					var score = ScoreGroup(overlaps, depth+1);
					sum -= score;
					processed.Add(c);
				}

				return sum;
			}
		}

		public class Cube {
			public int minX;
			public int minY;
			public int minZ;

			public int maxX;
			public int maxY;
			public int maxZ;

			public int xLen;
			public int yLen;
			public int zLen;

			public ulong volume;

			public int val;

			public Cube(int oMinX, int oMinY, int oMinZ, int oMaxX, int oMaxY, int oMaxZ, int oVal) {
				minX = oMinX;
				minY = oMinY;
				minZ = oMinZ;

				maxX = oMaxX;
				maxY = oMaxY;
				maxZ = oMaxZ;

				val = oVal;

				xLen = maxX - minX + 1;
				yLen = maxY - minY + 1;
				zLen = maxZ - minZ + 1;

				volume = (ulong)xLen * (ulong)yLen * (ulong)zLen;
			}

			public bool Overlaps(Cube other) {
				if (maxZ < other.minZ) { return false; }
				if (minZ > other.maxZ) { return false; }

				if (maxX < other.minX) { return false; }
				if (minX > other.maxX) { return false; }

				if (maxY < other.minY) { return false; }
				if (minY > other.maxY) { return false; }

				return true;
			}

			public Cube? Overlap(Cube b) {
				if (!Overlaps(b)) {
					return null;
				}

				var c = new Cube(
					Math.Max(minX, b.minX),
					Math.Max(minY, b.minY),
					Math.Max(minZ, b.minZ),
					Math.Min(maxX, b.maxX),
					Math.Min(maxY, b.maxY),
					Math.Min(maxZ, b.maxZ),
					val
				);

				return c;
			}

			public bool Same(Cube b) {
				return
				(minX == b.minX) &&
				(minY == b.minY) &&
				(minZ == b.minZ) &&
				(maxX == b.maxX) &&
				(maxY == b.maxY) &&
				(maxZ == b.maxZ);
			}

			public bool PointInCube(int x, int y, int z) {
				return (x >= minX && x <= maxX) && (y >= minY && y <= maxY) && (z >= minZ && z <= maxZ);
			}

			public string Print() {
				return $"[{minX,6},{minY,6},{minZ,6}] [{maxX,6},{maxY,6},{maxZ,6}] ({volume})";
			}
		}
	}
}
