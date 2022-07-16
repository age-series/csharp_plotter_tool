using System.Net;
using System.Text.Json;
namespace CircuitPlotter
{
    internal class HttpPlotter
    {
        private (Task task, CancellationTokenSource cts)? _displayTask;
        private readonly HttpListener _server;

        public HttpPlotter()
        {
            _server = new HttpListener();
            _server.Prefixes.Add("http://*:3141/");
        }

        public Task Run()
        {
            _server.Start();
            return RunAsync();
        }

        private async Task RunAsync()
        {
            while (true)
            {
                var context = await _server.GetContextAsync();
                var request = context.Request;
                var reader = new StreamReader(request.InputStream);
                var data = await reader.ReadToEndAsync();

                if (_displayTask is not null && !_displayTask.Value.task.IsCompleted)
                {
                    // cancel current display
                    _displayTask.Value.cts.Cancel();
                }

                var cts = new CancellationTokenSource();
                var task = Task.Run(() => HandleData(data, cts.Token), cts.Token);

                _displayTask = (task, cts);

                reader.Dispose();
            }
        }

        private static void HandleData(string data, CancellationToken token)
        {
            try
            {
                Console.Title = ("Handling data!");

                var cells = JsonSerializer.Deserialize<JsonFrame>(data)!.cells;

                if (cells.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("No cells received!");
                    Console.ResetColor();
                    return;
                }

                var minX = cells.Min(x => x.x);
                var minY = cells.Min(x => x.y);

                var offsetX = 2 + (minX < 0 ? Math.Abs(minX) : -minX);
                var offsetY = 2 + (minY < 0 ? Math.Abs(minY) : -minY);

                var plotter = new ComponentPlot();

                plotter.Components.AddRange(
                    cells.Select(c => new JsonCell(c.x + offsetX, c.y + offsetY, c.connections, c.symbol, c.info)));

                plotter.Plot(token);

                Console.Title = $"Last plot: {DateTime.Now.ToShortTimeString()}, with {plotter.Components.Count} components!";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
