namespace AdventOfCode.Day22
{
	public static class Day22 {
		public static void Main() {
			// Part1();
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

			List<List<int>> overlaps;

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

					for (int z = minZ; z <= maxZ; z++) {
						cubes.Add(new Cube(minX, minY, z, maxX, maxY, z, val));
					}
				}

				Console.WriteLine($"Done Processing Cubes: {cubes.Count()}");

				overlaps = new();

				for (int i = 0; i < cubes.Count(); i++) {
					if (i % 10000 == 0) {
						Console.WriteLine($"Computing Overlap {i} of {cubes.Count()}");
					}

					if (overlaps.Count() == 0) {
						var group = new List<int>();
						group.Add(i);
						overlaps.Add(group);
						continue;
					}

					var placed = false;
					for (int og = 0; og < overlaps.Count(); og++) {
						if (placed) {break;}
						for (int c = 0; c < overlaps[og].Count(); c++) {
							if(cubes[i].minZ != cubes[overlaps[og][c]].minZ) {
								break; // Not in the same Z space, so no point in checking this group.
							}

							if(cubes[i].Overlap(cubes[overlaps[og][c]])) {
								overlaps[og].Add(i);
								placed = true;
								break;
							}
						}
					}

					if (!placed) {
						var group = new List<int>();
						group.Add(i);
						overlaps.Add(group);
						continue;
					}
				}

				Console.WriteLine($"Cubes: {cubes.Count()} Overlap Groups: {overlaps.Count()}");

				ulong sum = 0;
				for (int i = 0; i < overlaps.Count(); i++) {
					sum += sumGroup(cubes, overlaps[i]);
				}

				Console.WriteLine($"Sum: {sum}");
			}

			public static ulong sumGroup(List<Cube> cubes, List<int> group) {
				if (group.Count() == 1) {
					return cubes[group[0]].val == 1 ? cubes[group[0]].volume : 0;
				}

				Dictionary<(int x, int y), bool> overlap = new();

				for (int c = 0; c < group.Count(); c++) {
					var cube = cubes[group[c]];

					for (int x = cube.minX; x <= cube.maxX; x++) {
						for (int y = cube.minY; y <= cube.maxY; y++) {
							var key = (x: x, y: y);

							if(cube.val == 1 && !overlap.ContainsKey(key)) {
								overlap.Add(key, true);
							}

							if(cube.val == 0 && overlap.ContainsKey(key)) {
								overlap.Remove(key);
							}
						}
					}
				}

				return (ulong)overlap.LongCount();
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

			// All cubes are X by Y by 1.
			public bool Overlap(Cube other) {
				if (minZ != maxZ) { Console.WriteLine("PANIC"); return false; }

				if (minZ != other.minZ) { return false; }
				if (maxZ != other.maxZ) { return false; }

				if (maxX < other.minX) { return false; }
				if (minX > other.maxX) { return false; }

				if (maxY < other.minY) { return false; }
				if (minY > other.maxY) { return false; }

				return true;
			}
		}
	}
}
