public class Day4 {
	public static void Main() {
		Day4 d = new Day4();

		System.IO.StreamReader inputFile = new System.IO.StreamReader(@"./input.real.txt");

		int[]? calls = d.LoadCalls(inputFile);
		if (calls == null) {
			Console.WriteLine("Error Loading Calls From File");
			return;
		}

		List<Board> boards = d.LoadBoards(inputFile);
		if (boards.Count() < 1) {
			Console.WriteLine("Error Loading Boards From File");
			return;
		}


		d.Part1(calls, boards);

		for (int i = 0; i < boards.Count(); i++) {
			boards[i].Reset();
		}

		d.Part2(calls, boards);
	}

	private int[]? LoadCalls(System.IO.StreamReader inputFile) {
		if (inputFile == null) {
			Console.WriteLine("No Input");
			return null;
		}

		string? fl = inputFile.ReadLine();
		if (fl == null) {
			Console.WriteLine("No First Line");
			return null;
		}

		return Array.ConvertAll(fl.Split(","), s => int.Parse(s));
	}

	private List<Board> LoadBoards(System.IO.StreamReader inputFile) {
		List<Board> boards = new List<Board>();

		// Consume the empty line
		while (true) {
			string? empty = inputFile.ReadLine();

			if (empty == null) {
				break;
			}

			string[] input = new string[5];
			for (int i = 0; i < 5; i++) {
				string? line = inputFile.ReadLine();

				if (line == null) {
					Console.WriteLine("Expected Board Line, got EOF");
					return boards;
				}

				input[i] = line;
			}

			boards.Add(new Board(input));
		}

		return boards;
	}



	public void Part1(int[] calls, List<Board> boards) {
		foreach (int call in calls) {
			for (int i = 0; i < boards.Count(); i++) {
				if (boards[i].Mark(call)) {
					Console.WriteLine($"Part 1: Call: {call} Sum: {boards[i].USum} Product: {call*boards[i].USum}");
					return;
				};
			}
		}
	}

	public void Part2(int[] calls, List<Board> boards) {
		int remaining = boards.Count();
		bool[] winners = new bool[boards.Count()];
		int lastWin = -1;
		int lastCall = -1;
		int active = boards.Count();

		foreach (int call in calls) {
			lastCall = call;
			for (int i = 0; i < boards.Count(); i++) {
				if (winners[i]) {
					continue;
				}

				if (boards[i].Mark(call)) {
					if (!winners[i]) {
						winners[i] = true;
						lastWin = i;
						active--;

						if(active == 0) {
							Console.WriteLine($"Part 2: Call: {call} Sum: {boards[i].USum} Product: {call*boards[i].USum}");
							return;
						}
					}
				};
			}
		}

		if (lastWin < 0) {
			Console.WriteLine("No Last Winner Found");
			return;
		}

		Console.WriteLine($"Part 2: Call: {lastCall} Sum: {boards[lastWin].USum} Product: {lastCall*boards[lastWin].USum}");
		return;
	}
};

public class Board {
	private int[][] board;
	private bool[][] marked;

	// Scores
	private int[] byRow;
	private int[] byCol;

	public bool Winner;

	public int USum;

	public Board(string[] input) {
		if (input.Count() != 5) {
			Console.WriteLine("Expected 5 Lines to Initialize a Board");
		}

		board = new int[5][];
		marked = new bool[5][];

		for (int i = 0; i < 5; i++) {
			board[i] = Array.ConvertAll(input[i].Replace("  ", " ").Trim().Split(" "), s => int.Parse(s));
			marked[i] = new bool[5]{false, false, false, false, false};
		}

		byRow = new int[5];
		byCol = new int[5];
	}

	public void Reset() {
		Winner = false;
		for (int row = 0; row < 5; row++) {
			byRow[row] = 0;
			for (int col = 0; col < 5; col++) {
				marked[row][col] = false;
				byCol[col] = 0;
			}
		}
	}

	public override string ToString() {
		string output = "";

		for (int row = 0; row < 5; row++) {
			for (int col = 0; col < 5; col++) {
				if (marked[row][col]) {
					output += $"{this.board[row][col],3}*";
				} else {
					output += $"{this.board[row][col],4}";
				}
			}
			output += "\n";
		}

		return output;
	}

	private bool mark(int call) {
		for (int row = 0; row < 5; row++) {
			for (int col = 0; col < 5; col++) {
				if (board[row][col] == call) {
					marked[row][col] = true;

					byRow[row]++;
					byCol[col]++;

					if(byRow[row] == 5) {
						return true;
					}

					if(byCol[col] == 5) {
						return true;
					}

					return false;
				}
			}
		}

		return false;
	}

	public bool Mark(int call) {
		bool winner = mark(call);
		if (winner) {
			int uSum = 0;

			for (int row = 0; row < 5; row++) {
				for (int col = 0; col < 5; col++) {
					if (!marked[row][col]) {
						uSum += board[row][col];
					}
				}
			}

			USum = uSum;
		}


		return winner;
	}
}