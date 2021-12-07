public class Day07 {
	public static void Main() {
		Day07 d = new();

		System.IO.StreamReader inputFile = new System.IO.StreamReader(@"./input.real.txt");
		var crabs = inputFile.ReadLine()?.Split(",").Select(int.Parse).ToArray();
		if (crabs is null) {
			Console.WriteLine("No Crabs!");
			return;
		}

		Array.Sort(crabs);
		d.CrabWalk(crabs);
	}

	public void CrabWalk(int[] crabs) {
		int[] cheap = new int[crabs.Count()];
		int[] expensive = new int[crabs.Count()];
		for (int i = 0; i < crabs.Count(); i++) {
			for (int b = 0; b < crabs.Count(); b++) {
				int stepSize = Math.Abs(crabs[b] - i);
				cheap[i] += stepSize;
				expensive[i] += (stepSize * (1 + stepSize)) / 2;
			}
		}

		int cheapLow = -1;
		int cheapPos = 0;
		int expLow = -1;
		int expPos = 0;
		for (int i = 0; i < cheap.Count(); i++) {
			if (cheapLow < 0 || cheap[i] < cheapLow) {
				cheapPos = i;
				cheapLow = cheap[i];
			}
			if (expLow < 0 || expensive[i] < expLow) {
				expPos = i;
				expLow = expensive[i];
			}
		}

		Console.WriteLine($"Part 1: Move All Crabs to {cheapPos} with cost {cheapLow}");
		Console.WriteLine($"Part 2: Move All Crabs to {expPos} with cost {expLow}");
	}
};

