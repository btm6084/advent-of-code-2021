namespace AdventOfCode
{
	public static class InputParser
	{
		public static IEnumerable<T> Parse<T>(string path, Func<string, T> parser)
		{
			if (!File.Exists(path)) {
				throw new ArgumentException($"'{path}' does not exist");
			}

			foreach (string line in File.ReadLines(path)) {
				yield return parser(line);
			}
		}
	}
}