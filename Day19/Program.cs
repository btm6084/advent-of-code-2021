namespace AdventOfCode.Day19
{
	public static class Day19 {
		public static void Main() {
			Part1();
			// Part2();
		}

		public static (int x, int y, int z) rotateX((int x, int y, int z) pos) {
			// (z, y) -------> (-y, z)
			return (x: pos.x, y: -pos.z, z: pos.y);
		}

		public static (int x, int y, int z) rotateY((int x, int y, int z) pos) {
			// (z, x) -------> (-z, x)
			return (x: -pos.z, y: pos.y, z: pos.x);
		}

		public static (int x, int y, int z) rotateZ((int x, int y, int z) pos) {
			// (x, y) -------> (-y, x)
			return (x: -pos.y, y: pos.x, z: pos.z);
		}

		public static (int x, int y, int z) rotate(int rx, int ry, int rz, (int x, int y, int z) pos) {
			(int x, int y, int z) n = (x: pos.x, y: pos.y, z: pos.z);
			for (int i = 0; i < rx; i++) {
				n = rotateX(n);
			}
			for (int i = 0; i < ry; i++) {
				n = rotateY(n);
			}
			for (int i = 0; i < rz; i++) {
				n = rotateZ(n);
			}

			return (x: n.x, y: n.y, z: n.z);
		}

		private static (int x, int y, int z) getTranslation((int x, int y, int z) a, (int x, int y, int z) b) {
			return (x: a.x - b.x, y: a.y - b.y, z: a.z - b.z);
		}

		public static List<(int x, int y, int z)> orientations(List<(int x, int y, int z)> beacons) {
			List<(int x, int y, int z)> o = new();

			foreach ((int x, int y, int z) b in beacons) {
				for (int rx = 0; rx < 4; rx++) {
					for (int ry = 0; ry < 4; ry++) {
						for (int rz = 0; rz < 4; rz++) {
							o.Add(rotate(rx, ry, rz, b));
						}
					}
				}
			}

			return o.Distinct().ToList();
		}

		public static List<(int x, int y, int z)> translateList((int x, int y, int z) t, List<(int x, int y, int z)> orientations) {
			List<(int x, int y, int z)> tl = new();

			foreach ((int x, int y, int z) o in orientations) {
				tl.Add((o.x+t.x, o.y+t.y, o.z+t.z));
			}

			return tl;
		}

		private static (bool found, int x, int y, int z, int rx, int ry, int rz) FindMatches(List<(int x, int y, int z)> match, List<(int x, int y, int z)> search) {
			var output = (found: false, x: -1, y: -1, z: -1, rx: 0, ry: 0, rz: 0);

			for (int rx = 0; rx < 4; rx++) {
				for (int ry = 0; ry < 4; ry++) {
					for (int rz = 0; rz < 4; rz++) {
						// Rotate it, Translate it, Check for inclusion.
						List<(int x, int y, int z)> trial = new();

						for (int k = 0; k < search.Count(); k++) {
							trial.Add(rotate(rx, ry, rz, search[k]));
						}

						var found = false;
						for (int a = 0; a < match.Count(); a++) {
							if (found) {break;}
							for (int b = 0; b < trial.Count(); b++) {
								var t = getTranslation(match[a], trial[b]);
								var tl = translateList(t, trial);

								if (match.Where(x => tl.Contains(x)).Count() >= 12) {
									Console.WriteLine($"\tFOUND! [{rx},{ry},{rz}] [{t.x},{t.y},{t.z}]");
									return (found: true, x: t.x, y: t.y, z: t.z, rx: rx, ry: ry, rz: rz);
								}
							}
						}


					}
				}
			}

			return output;
		}

		public static void Part1() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();
			List<List<(int x, int y, int z)>> beacons = new();

			int scnNum = -1;
			foreach (string input in inputs) {
				if (input.Contains("--- scanner ")) {
					beacons.Add(new List<(int x, int y, int z)>());
					scnNum ++;
					continue;
				}

				if (input == "") {
					continue;
				}

				var pieces = input.Split(",").Select(x => int.Parse(x)).ToArray();
				beacons[scnNum].Add((x: pieces[0], y: pieces[1], z: pieces[2]));
			}

			Dictionary<int, (int x, int y, int z)> toZero = new();
			toZero.Add(0, (x: 0, y: 0, z: 0));

			Dictionary<(int x, int y), bool> seen = new();

			while (toZero.Count() < beacons.Count()) {
				Dictionary<int, (int x, int y, int z)> known = new();
				foreach (KeyValuePair<int, (int x, int y, int z)> kvp in toZero) {
					known.Add(kvp.Key, kvp.Value);
				}

				foreach (KeyValuePair<int, (int x, int y, int z)> k in known) {
					for (int j = 0; j < beacons.Count(); j++) {
						if (k.Key == j) { continue; }
						if (toZero.ContainsKey(k.Key) && toZero.ContainsKey(j)) { continue; }

						var seenKey = (x: k.Key, y: j);

						if (seen.ContainsKey(seenKey)) { continue; }
						seen.Add(seenKey, true);

						// Console.WriteLine($"[{k.Key},{j}]");

						var t = FindMatches(beacons[k.Key], beacons[j]);
						if (t.found) {
							// Console.WriteLine($"[{k.Key},{j}] found: {t.found} x: {t.x} y: {t.y} z: {t.z}");

							if (toZero.ContainsKey(k.Key)) {
								if (!toZero.ContainsKey(j)) {
									toZero.Add(j, (x: t.x+k.Value.x, y: t.y+k.Value.y, z: t.z+k.Value.z));

									var tz = toZero[j];

									for (int l = 0; l < beacons[j].Count(); l++) {
										beacons[j][l] = rotate(t.rx, t.ry, t.rz, beacons[j][l]);
									}
								}
							}
						}
					}
				}
			}

			List<(int x, int y, int z)> final = new();

			foreach(KeyValuePair<int, (int x, int y, int z)> kvp in toZero) {
				// Console.WriteLine($"{kvp.Key} to 0: [{kvp.Value.x},{kvp.Value.y},{kvp.Value.z}]");

				beacons[kvp.Key] = translateList(kvp.Value, beacons[kvp.Key]);
				for (int i = 0; i < beacons[kvp.Key].Count(); i++) {
					final.Add(beacons[kvp.Key][i]);
				}
			}

			final = final.Distinct().ToList();

			var max = 0;

			foreach(KeyValuePair<int, (int x, int y, int z)> a in toZero) {
				foreach(KeyValuePair<int, (int x, int y, int z)> b in toZero) {
					if (a.Key == b.Key) { continue; }
					max = Math.Max(max, Manhattan(a.Value, b.Value));
				}
			}

			Console.WriteLine($"Total Beacons: {final.Count()}");
			Console.WriteLine($"Max Distance: {max}");
		}

		private static int Manhattan((int x, int y, int z) a, (int x, int y, int z) b) {
			int sum = 0;

			// Console.WriteLine($"A: [{a.x},{a.y},{a.z}]");
			// Console.WriteLine($"B: [{b.x},{b.y},{b.z}]");
			// Console.WriteLine($"C: [{Math.Abs((Math.Max(a.x, b.x) - Math.Min(a.x, b.x)))},{Math.Abs((Math.Max(a.y, b.y) - Math.Min(a.y, b.y)))},{Math.Abs((Math.Max(a.z, b.z) - Math.Min(a.z, b.z)))}]");
			// Console.WriteLine();

			sum += Math.Abs((Math.Max(a.x, b.x) - Math.Min(a.x, b.x)));
			sum += Math.Abs((Math.Max(a.y, b.y) - Math.Min(a.y, b.y)));
			sum += Math.Abs((Math.Max(a.z, b.z) - Math.Min(a.z, b.z)));

			return sum;
		}
	}
}
