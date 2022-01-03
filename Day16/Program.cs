namespace AdventOfCode.Day16 {
	public static class Day16 {

		public static Dictionary<char, string> hexMap = new(){
			{'0', "0000"},
			{'1', "0001"},
			{'2', "0010"},
			{'3', "0011"},
			{'4', "0100"},
			{'5', "0101"},
			{'6', "0110"},
			{'7', "0111"},
			{'8', "1000"},
			{'9', "1001"},
			{'A', "1010"},
			{'B', "1011"},
			{'C', "1100"},
			{'D', "1101"},
			{'E', "1110"},
			{'F', "1111"},
		};

		public static void Main() {
			char[] bits = InputParser.Parse("./input.real.txt", x => x).First().ToCharArray();
			var bin = HexToBinary(bits);

			Part1(bin);
			Part2(bin);
		}

		public static void Part1(char[] bin) {
			var p = new Packet(bin);

			// Console.WriteLine($"Version: {p.Version} TypeID: {p.TypeID} Type4Literal: {p.Type4Literal}");
			// if (p.TypeID != 4) {
			// 	Console.WriteLine($"LengthTypeID: {p.LengthTypeID} SubPackets: {p.SubPackets.Count}");

			// 	p.SubPackets.ForEach(p => Console.WriteLine($"SubPacket: V: {p.Version} T: {p.TypeID} T4L: {p.Type4Literal}"));
			// }

			Console.WriteLine($"Version Sums: {Packet.SumVersions(p)}");
		}

		public static void Part2(char[] bin) {
			var p = new Packet(bin);

			Console.WriteLine($"Packet Value: {Packet.ValuePackets(p)}");
		}

		public static char[] HexToBinary(char[] hex) {
			var bin = "";
			for (long i = 0; i < hex.Count(); i++) {
				bin += hexMap[hex[i]];
			}

			return bin.ToCharArray();
		}


	}

	public class Packet {
		public char[] Binary;

		public long Type4Literal;

		public long Version;
		public long TypeID;

		public long LengthTypeID;

		public List<Packet> SubPackets;

		private int BitLength;

		public Packet(char[] bin) {
			// Console.WriteLine($"NewPacket: {new string(bin)}");

			Binary = bin;
			SubPackets = new List<Packet>();
			BitLength = 0;

			// Console.WriteLine($"VB: {new string(bin.Take(3).ToArray())}");
			Version = BinToInt(new string(bin.Take(3).ToArray()), 2);
			BitLength += 3;
			bin = bin.Skip(3).ToArray();

			// Console.WriteLine($"TB: {new string(bin.Take(3).ToArray())}");
			TypeID = BinToInt(new string(bin.Take(3).ToArray()), 2);
			BitLength += 3;
			bin = bin.Skip(3).ToArray();

			// Console.WriteLine($"Version: {Version} TypeId: {TypeID}");

			if (TypeID == 4) {
				var binString = "";

				while (true) {
					binString += new string(bin.Skip(1).Take(4).ToArray());
					BitLength += 5;

					if (bin.FirstOrDefault() == '0') {
						break;
					}

					bin = bin.Skip(5).ToArray();
				}

				Type4Literal = BinToInt(new string(binString.ToArray()), 2);
				return;
			}

			LengthTypeID = BinToInt(new string(bin.Take(1).ToArray()), 2);
			BitLength += 1;
			bin = bin.Skip(1).ToArray();

			// Console.WriteLine($"LenTypeID: {LengthTypeID}");

			if (LengthTypeID == 0) {
				var len = BinToInt(new string(bin.Take(15).ToArray()));
				// Console.WriteLine($"Length: {len} {new string(bin.Take(15).ToArray())}");
				BitLength += 15;
				bin = bin.Skip(15).ToArray();

				var subPacketsBin = bin.Take((int)len).ToArray();
				var read = 0;

				while (read < len) {
					var p = new Packet(subPacketsBin);
					BitLength += p.BitLength;
					read += p.BitLength;
					subPacketsBin = subPacketsBin.Skip(p.BitLength).ToArray();
					SubPackets.Add(p);

					// Console.WriteLine($"SubPacket: V: {p.Version} T: {p.TypeID} Literal: {p.Type4Literal} Len: {p.BitLength} Read: {read} Length: {len}");
				}
			} else {
				var len = BinToInt(new string(bin.Take(11).ToArray()));
				// Console.WriteLine($"Length: {len} {new string(bin.Take(11).ToArray())}");
				BitLength += 11;
				bin = bin.Skip(11).ToArray();

				for (int i = 0; i < len; i++) {
					var p = new Packet(bin);
					BitLength += p.BitLength;

					bin = bin.Skip(p.BitLength).ToArray();
					SubPackets.Add(p);

					// Console.WriteLine($"SubPacket2: V: {p.Version} T: {p.TypeID} Literal: {p.Type4Literal} Len: {p.BitLength} Read: {i}");
				}
			}
		}

		public static long SumVersions(Packet p) {
			long sum = p.Version;

			for (int i = 0; i < p.SubPackets.Count; i++) {
				sum += SumVersions(p.SubPackets[i]);
			}

			return sum;
		}

		public static long ValuePackets(Packet p) {
			switch (p.TypeID) {
				case 0:
					long sum = 0;
					for (int i = 0; i < p.SubPackets.Count; i++) {
						sum += ValuePackets(p.SubPackets[i]);
					}

					return sum;
				case 1:
					long product = 1;
					for (int i = 0; i < p.SubPackets.Count; i++) {
						product *= ValuePackets(p.SubPackets[i]);
					}

					return product;
				case 2:
					long min = long.MaxValue;
					for (int i = 0; i < p.SubPackets.Count; i++) {
						min = Math.Min(min, ValuePackets(p.SubPackets[i]));
					}

					return min;
				case 3:
					long max = 0;
					for (int i = 0; i < p.SubPackets.Count; i++) {
						max = Math.Max(max, ValuePackets(p.SubPackets[i]));
					}

					return max;
				case 4:
					return p.Type4Literal;
				case 5:
					if (ValuePackets(p.SubPackets[0]) > ValuePackets(p.SubPackets[1])) {
						return 1;
					}
					return 0;
				case 6:
					if (ValuePackets(p.SubPackets[0]) < ValuePackets(p.SubPackets[1])) {
						return 1;
					}
					return 0;
				case 7:
					if (ValuePackets(p.SubPackets[0]) == ValuePackets(p.SubPackets[1])) {
						return 1;
					}
					return 0;
				default: return 0;
			}
		}

		public static long Type4Value(char []rest) {
			var bin = "";

			while (true) {
				bin += new string(rest.Skip(1).Take(4).ToArray());

				if (rest.FirstOrDefault() == '0') {
					break;
				}

				rest = rest.Skip(5).ToArray();
			}
			return BinToInt(new string(bin.ToArray()), 2);
		}

		public static long BinToInt(string line, int _) {
			return BinToInt(line);
		}

		public static long BinToInt(string line) {
			long sum = 0;

			for (int i = line.Count()-1; i >= 0; i--) {
				long magnitude = (long)Math.Pow(2, ((line.Count()-1) - i));
				long val = 0;
				if (long.TryParse(line[i].ToString(), out val)) {
					sum += magnitude * val;
				};
			}

			return sum;
		}
	}


}

