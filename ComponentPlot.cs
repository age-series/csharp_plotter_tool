namespace CircuitPlotter;

internal sealed class ComponentPlot
{
	public List<JsonCell> Components { get; } = new();

	public void Plot(CancellationToken token)
	{
		Console.Clear();

        var smallestX = Components.MinBy(x => x.x)!.x;
        var largestX = Components.MaxBy(x => x.x)!.x;
		var width = largestX - smallestX;
		width = width == 0 ? 1 : width;

		var largestY = Components.MaxBy(x => x.y)!.y + 2;

		var color = 1;

		var history = new Dictionary<char, int>();

		foreach (var component in Components)
		{
			if (token.IsCancellationRequested)
			{
				Console.Title = "Cancelled plot!";
				return;
			}

			Console.SetCursorPosition((int)(component.x / (double)width * (width * 3d)), component.y);


			ConsoleColor GenerateColor()
			{
				var result = (ConsoleColor)(color++ % 15);

				if (result is ConsoleColor.Black)
				{
					result = ConsoleColor.DarkGreen;
				}

				return result;
			}

			Console.ForegroundColor = GenerateColor();

			var count = history.GetValueOrDefault(component.symbol, 0);
			history[component.symbol] = count + 1;
			
			Console.Write($"{component.symbol}{count}");

			Console.SetCursorPosition(0, largestY++);

			Console.WriteLine($"[{component.symbol}/{count}] {component.info}");
		}

		Console.ResetColor();
	}
}
