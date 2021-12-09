public class Day9 {
	public static void Main() {
		List<int[]> inputs = new List<int[]>();

		using var inputFile = new System.IO.StreamReader(@"./input.real.txt");
		string? line;
		while ((line = inputFile.ReadLine()) != null)
		{
			inputs.Add(Array.ConvertAll(line.ToCharArray(), s => int.Parse(s.ToString())));
		}

		int risk = 0;
		List<int[]> pits = new List<int[]>();
		int[][]? flow = new int[inputs.Count()][];
		for (int row = 0; row < inputs.Count(); row++) {
			flow[row] = new int[inputs[row].Count()];
			for (int col = 0; col < inputs[row].Count(); col++) {
				flow[row][col] = 0; // 0000

				var v = inputs[row][col];
				var n = row == 0 ? 9 : inputs[row-1][col];
				var s = row + 1 == (inputs.Count()) ? 9 : inputs[row+1][col];
				var w = col == 0 ? 9 : inputs[row][col-1];
				var e = col + 1 == (inputs[row].Count()) ? 9 : inputs[row][col+1];

				var np = row == 0 ? true : inputs[row-1][col] > v;
				var sp = row + 1 == (inputs.Count()) ? true : inputs[row+1][col] > v;
				var wp = col == 0 ? true : inputs[row][col-1] > v;
				var ep = col + 1 == (inputs[row].Count()) ? true : inputs[row][col+1] > v;

				if (v != 9) {
					if (n != 9) { flow[row][col] = !np ? flow[row][col] : flow[row][col] | 4;} // 0100
					if (s != 9) { flow[row][col] = !sp ? flow[row][col] : flow[row][col] | 2;} // 0010
					if (e != 9) { flow[row][col] = !ep ? flow[row][col] : flow[row][col] | 1;} // 0001
					if (w != 9) { flow[row][col] = !wp ? flow[row][col] : flow[row][col] | 8;} // 1000
				} else {
					flow[row][col] = 16;
				}

				if (np && sp && ep && wp) {
					risk += inputs[row][col] + 1;
					pits.Add(new int[]{row, col});
				}
			}
		}

		Console.WriteLine($"Part 1: Dips: {risk}");

		if (flow == null) {
			return;
		}

		var basins = new int[pits.Count()];
		for (int i = 0; i < pits.Count(); i++) {
			var pRow = pits[i][0];
			var pCol = pits[i][1];

			var n = pRow == 0 ? 16 : flow[pRow-1][pCol];
			var s = pRow + 1 == (flow.Count()) ? 16 : flow[pRow+1][pCol];
			var w = pCol == 0 ? 16 : flow[pRow][pCol-1];
			var e = pCol + 1 == (flow[pRow].Count()) ? 16 : flow[pRow][pCol+1];

			var b = Basin(flow, new List<string>(), pRow, pCol);
			basins[i] = b;
		}

		int topThree = basins.OrderByDescending(x => x).Take(3).ToArray().Aggregate(1, (a, b) => a * b);

		Console.WriteLine($"Part 2: Top Three Basin Size: {topThree}");
	}

	public static int Basin(int[][] flow, List<string> visited, int row, int col) {

		string key = $"[{row},{col}]";
		if (visited.Contains(key)) {
			return 0;
		}

		visited.Add(key);

		int sum = 1;
		var v = flow[row][col];

		if (v == 0) {
			return 1;
		}

		if ((v & 4) == 4) { // North
			sum += Basin(flow, visited, row-1, col);
		}
		if ((v & 2) == 2) { // South
			sum += Basin(flow, visited, row+1, col);
		}
		if ((v & 1) == 1) { // East
			sum += Basin(flow, visited, row, col+1);
		}
		if ((v & 8) == 8) { // West
			sum += Basin(flow, visited, row, col-1);
		}

		return sum;
	}
};