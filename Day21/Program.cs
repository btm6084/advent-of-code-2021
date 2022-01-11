namespace AdventOfCode.Day21
{
	public static class Day21 {
		public static void Main() {
			Part1();
			Part2();
		}

		public class DeterministicDie {
			int i;
			int rolls;

			public DeterministicDie() {
				i = 1;
				rolls = 0;
			}

			public int Next() {
				var sum = i;
				i++;
				if (i == 101) {i = 1;};
				sum += i;
				i++;
				if (i == 101) {i = 1;};
				sum += i;
				i++;
				if (i == 101) {i = 1;};

				rolls++;

				return sum;
			}

			public int Rolls() {
				return rolls * 3;
			}
		}

		public static void Part1() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();

			var p1Pos = int.Parse(inputs[0].Split(": ")[1])-1;
			var p2Pos = int.Parse(inputs[1].Split(": ")[1])-1;

			var p1Score = 0;
			var p2Score = 0;

			var die = new DeterministicDie();

			var turn = 0;
			while (true) {
				turn++;

				// P1 Turn
				var startPos = p1Pos;
				var roll = die.Next();
				p1Pos += roll;
				p1Pos = p1Pos % 10;
				p1Score += p1Pos+1;

				if (p1Score >= 1000) {
					Console.WriteLine("Player 1 Wins!");
					Console.WriteLine($"Rolls: {die.Rolls()} Player 2 Score: {p2Score}. Output: {p2Score * die.Rolls()}");
					break;
				}

				// P2 Turn
				startPos = p2Pos;
				roll = die.Next();
				p2Pos += roll;
				p2Pos = p2Pos % 10;
				p2Score += p2Pos+1;

				if (p2Score >= 1000) {
					Console.WriteLine("Player 2 Wins!");
					Console.WriteLine($"Rolls: {die.Rolls()} Player 1 Score: {p1Score}. Output: {p1Score * die.Rolls()}");
					break;
				}
			}
		}

		public static void Part2() {
			string[] inputs = InputParser.Parse("./input.real.txt", x => x).ToArray();

			var p1Pos = int.Parse(inputs[0].Split(": ")[1])-1;
			var p2Pos = int.Parse(inputs[1].Split(": ")[1])-1;

			Dictionary<(int p1pos, int p1sc, int p2pos, int p2sc), (long p1, long p2)> winners = new();

			var wins = playTo21(p1Pos, 0, p2Pos, 0, winners);
			Console.WriteLine($"Player 1: {wins.p1} Player 2: {wins.p2} Max: {Math.Max(wins.p1, wins.p2)}");
		}

		public static (long p1, long p2) playTo21(int p1Start, int p1Score, int p2Start, int p2Score, Dictionary<(int p1pos, int p1sc, int p2pos, int p2sc), (long p1, long p2)> winners) {
			if (winners.ContainsKey((p1Start, p1Score, p2Start, p2Score))) {
				return winners[(p1Start, p1Score, p2Start, p2Score)];
			}

			long p1Wins = 0;
			long p2Wins = 0;

			var nextGame = new List<(int p1pos, int p1sc, int p2pos, int p2sc)>();

			for (int a = 1; a < 4; a++) {
				for (int b = 1; b < 4; b++) {
					for (int c = 1; c < 4; c++) {
						var roll1 = (a+b+c);
						var pos1 = (p1Start + roll1) % 10;
						var score1 = p1Score + pos1 + 1;

						if (score1 >= 21) {
							p1Wins++;
							continue;
						}

						for (int d = 1; d < 4; d++) {
							for (int e = 1; e < 4; e++) {
								for (int f = 1; f < 4; f++) {
									var roll2 = (d+e+f);
									var pos2 = (p2Start + roll2) % 10;
									var score2 = p2Score + pos2 + 1;

									if (score2 >= 21) {
										p2Wins++;
										continue;
									}

									// Push it onto the stack for another round.
									nextGame.Add((pos1, score1, pos2, score2));
								}
							}
						}
					}
				}
			}

			Dictionary<(int p1pos, int p1sc, int p2pos, int p2sc), int> distinctGames = new();
			for (int i = 0; i < nextGame.Count(); i++) {
				if (!distinctGames.ContainsKey(nextGame[i])) {
					distinctGames.Add(nextGame[i], 1);
					continue;
				}

				distinctGames[nextGame[i]]++;
			}

			foreach (KeyValuePair<(int p1pos, int p1sc, int p2pos, int p2sc), int> k in distinctGames) {
				var game = k.Key;
				var count = k.Value;

				var wins = playTo21(game.p1pos, game.p1sc, game.p2pos, game.p2sc, winners);
				p1Wins += (wins.p1 * count);
				p2Wins += (wins.p2 * count);
			}

			winners.Add((p1Start, p1Score, p2Start, p2Score), (p1Wins, p2Wins));
			return (p1: p1Wins, p2: p2Wins);
		}
	}
}
