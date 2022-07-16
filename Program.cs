using CircuitPlotter;

var server = new HttpPlotter();
Console.WriteLine("Starting...");
await server.Run();