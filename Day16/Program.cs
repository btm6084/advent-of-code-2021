namespace AdventOfCode.Day16 {
	public static class Day16 {

		public static void Main() {
			char[] bits = InputParser.Parse("./input.example.txt", x => x).First().ToCharArray();
			Part1(bits);
			Part2();
		}

		public static void Part1(char[] bits) {
			for(int i = 0; i < bits.Count(); i++) {
				Console.WriteLine(bits[i].ToString());
			}
		}

		public static void Part2() {
		}
	}
}