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

			List<Cube> wholeCubes;

			public Reactor(string[] inputs, bool p1) {
				cubes = new();
				wholeCubes = new();

				for (int i = 0; i < inputs.Length; i++) {
					Console.WriteLine($"Cube {i} of {inputs.Length}");
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

					wholeCubes.Add(new Cube(minX, minY, minZ, maxX, maxY, maxZ, val));
				}

				ulong sum = 0;

				for (int i = 0; i < wholeCubes.Count(); i++) {
					var found = false;
					var wc = wholeCubes[i];

					for (int j = 0; j < wholeCubes.Count(); j++) {
						if (i == j) { continue; }

						if(wholeCubes[i].Overlap(wholeCubes[j])) {
							found = true;
							for (int z = wc.minZ; z <= wc.maxZ; z++) {
								cubes.Add(new Cube(wc.minX, wc.minY, z, wc.maxX, wc.maxY, z, wc.val));
							}

							break;
						}
					}

					if (!found) {
						Console.WriteLine($"No overlap: {i}");
						if (wc.val == 1) {
							sum += wc.volume;
						}
					}
				}

				overlaps = new();
				for (int i = 0; i < cubes.Count(); i++) {
					if (i % 10000 == 0) {
						Console.WriteLine($"Computing Overlap {i} of {cubes.Count()} [{cubes.Count()}]");
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
				if (maxZ < other.minZ) { return false; }
				if (minZ > other.maxZ) { return false; }

				if (maxX < other.minX) { return false; }
				if (minX > other.maxX) { return false; }

				if (maxY < other.minY) { return false; }
				if (minY > other.maxY) { return false; }

				return true;
			}
		}
	}
}
