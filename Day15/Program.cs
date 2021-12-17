namespace AdventOfCode.Day15 {
	public static class Day15 {

		public static void Main() {
			int[][] cavern = InputParser.Parse("./input.real.txt", x => x.Select(n => int.Parse(n.ToString())).ToArray()).ToArray();
			Part1(cavern);
			Part2(cavern);
		}

		public static void Part1(int[][] cavern) {
			var graph = RiskToGraph(cavern);
			Console.WriteLine($"Part 1: {Dijkstra(graph, 0)}");
		}

		public static void Part2(int[][] cavern) {
			int my = cavern.Count() * 5;
			int mx = cavern[0].Count() * 5;

			var bigCavern = new int[my][];
			for (int i = 0; i < bigCavern.Count(); i++) {
				bigCavern[i] = new int[mx];
			}

			Console.WriteLine($"Big Cavern: {bigCavern.Count()}x{bigCavern[0].Count()}");

			int rows = cavern.Count();
			int cols = cavern[0].Count();

			for (int rowTile = 0; rowTile < 5; rowTile++) {
				for (int colTile = 0; colTile < 5; colTile++) {
					for (int row = 0; row < cavern.Count(); row++) {
						for (int col = 0; col < cavern[row].Count(); col++) {

							var val = cavern[row][col];
							var dx = (rowTile + colTile);
							var newVal = val + dx;

							bigCavern[row+(rows*rowTile)][col+(cols*colTile)] = (newVal) > 9 ? (newVal) % 9 : (newVal);
						}
					}
				}
			}

			Console.WriteLine("Big Cavern Populated");

			// for (int row = 0; row < bigCavern.Count(); row++) {
			// 	for (int col = 0; col < bigCavern[row].Count(); col++) {
			// 		Console.Write($"{bigCavern[row][col]}");
			// 	}
			// 	Console.WriteLine();
			// }

			// Console.WriteLine($"Big Cavern: {bigCavern.Count()}x{bigCavern[0].Count()}");


			var graph = RiskToGraph(bigCavern);
			Console.WriteLine("Graph Created");

			Console.WriteLine($"Part 2: {Dijkstra(graph, 0)}");
		}

		public static int[][] RiskToGraph(int[][]cavern) {
			int nodeCount = cavern.Count() * cavern[0].Count();

			int[][] graph = new int[nodeCount][];
			for(int i = 0; i < nodeCount; i++) {
				graph[i] = new int[nodeCount];
			}

			int rowOffset = cavern.Count();
			for (int row = 0; row < cavern.Count(); row++) {
			// for (int row = 0; row < 1; row++) {
				for(int col = 0; col < cavern.Count(); col++) {
					var node = (row * rowOffset) + col;

					if (row > 0) {
						// N Edge
						var nNode = ((row-1) * rowOffset) + col;
						graph[node][nNode] = cavern[row-1][col];
					}
					if (row+1 < cavern.Count()) {
						// S Edge
						var sNode = ((row+1) * rowOffset) + col;
						graph[node][sNode] = cavern[row+1][col];
					}
					if (col > 0) {
						// W Edge
						var wNode = (row * rowOffset) + col-1;
						graph[node][wNode] = cavern[row][col-1];
					}
					if (col+1 < cavern[row].Count()) {
						// E Edge
						var eNode = (row * rowOffset) + col+1;
						graph[node][eNode] = cavern[row][col+1];
					}
				}
			}

			return graph;
		}

		public static int Dijkstra(int[][] graph, int start) {
			int[] cost = new int[graph.Count()];
			bool[] visited = new bool[graph.Count()];

			for (int i = 0; i < graph.Count(); i++) {
				cost[i] = int.MaxValue;
				visited[i] = false;
			}

			cost[start] = 0;

			for (int i = 0; i < graph.Count() -1; i++) {
				Console.WriteLine($"Visitng Node: {i} of {graph.Count()-1}");
				var smallest = Min(cost, visited, graph.Count());
				visited[smallest] = true;

				for (int j = 0; j < graph.Count(); j++) {
					var noEdge = graph[smallest][j] == 0;
					var costToStart = cost[smallest] + graph[smallest][j];

					if (visited[j] || noEdge || costToStart >= cost[j]) {
						continue;
					}

					cost[j] = cost[smallest] + graph[smallest][j];
				}
			}

			return cost.Last();
		}

		public static int Min(int[] cost, bool[] visited, int len) {
			int min = int.MaxValue;
			int idx = -1;

			for (int i = 0; i < len; i++) {
				if(!visited[i] && cost[i] <= min) {
					min = cost[i];
					idx = i;
				}
			}

			return idx;

		}
	}
}