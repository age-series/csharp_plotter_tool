namespace CircuitPlotter;

internal sealed record JsonFrame(List<JsonCell> cells);

internal sealed record JsonCell(int x, int y, ConnectionDirection[] connections, char symbol, string info);
