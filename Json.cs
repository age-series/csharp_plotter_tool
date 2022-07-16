using C = CircuitPlotter.ConnectionDirection;
namespace CircuitPlotter;

internal sealed class JsonFrame
{
	public JsonFrame(List<JsonCell> cells)
	{
		this.cells = cells;
	}

	public List<JsonCell> cells { get; set; }
}

internal sealed class JsonCell
{
	public JsonCell(int x, int y, C[] connections, char symbol, string info)
	{
		this.x = x;
		this.y = y;
		this.connections = connections;
		this.symbol = symbol;
		this.info = info;
	}

	public int x { get; set; }

	public int y { get; set; }

	public C[] connections { get; }

	public char symbol { get; set; }

	public string info { get; set; }
}
