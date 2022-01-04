void Part1()
{
	System.IO.StreamReader inputFile = new System.IO.StreamReader(@"./input.real.txt");

	var input = inputFile.ReadLine();
	if (input == null) {
		Console.WriteLine("Invalid Input");
		return;
	}

	input = input.Replace("target area: ", "");
	var pieces = input.Split(", ");
	var xPieces = pieces[0].Replace("x=", "").Split("..").Select(n => int.Parse(n)).ToArray();
	var yPieces = pieces[1].Replace("y=", "").Split("..").Select(n => int.Parse(n)).ToArray();

	var left = xPieces[0];
	var right = xPieces[1];
	var bottom = yPieces[0];
	var top = yPieces[1];

	Console.WriteLine($"Top: {top} Left: {left} Bottom: {bottom} Right: {right}");

	var peakX = 0;
	var peakY = 0;
	var peak = 0;
	var hits = 0;
	for (int ix = 1; ix <= right+1; ix++) {
		for (int iy = bottom; iy < Math.Abs((bottom+1)*2); iy++) {
			var x = 0;
			var y = 0;

			var dx = ix;
			var dy = iy;

			var localPeak = 0;

			while(x < right && y > bottom) {
				x += dx;
				y += dy;

				if (y > localPeak) {
					localPeak = y;
				}

				dx = dx > 0 ? dx-1 : dx < 0 ? dx+1 : 0;
				dy--;

				if (x >= left && x <= right && y <= top && y >= bottom) {
					// HIT
					hits++;
					if (localPeak > peak) {
						peak = localPeak;
						peakX = ix;
						peakY = iy;
					}
					break;
				}
			}
		}
	}

	Console.WriteLine($"Peak Found: {peak} with Initial Velocity:[{peakX},{peakY}]. Hit target {hits} times.");
}

void Part2() {
}

Part1();
Part2();