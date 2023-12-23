namespace LoadTestConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var hostname = "intellispatialwebapplication.azurewebsites.net";
            var numberOfRequests = 100;
            var httpVersion = new Version(2, 0);

            using HttpClient client = new();

            client.DefaultRequestVersion = httpVersion;

            async Task<HttpResponseMessage> GetWeather()
            {
                var response = await client.GetAsync($"http://{hostname}/weatherforecast");

                return response;
            }

            var tasks = new Task<HttpResponseMessage>[numberOfRequests];

            int i = 0;

            for (i = 0; i < numberOfRequests; i++)
                tasks[i] = GetWeather();

            i = 0;

            HttpResponseMessage[] responses = await Task.WhenAll(tasks);

            foreach (var response in responses)
            {
                i++;

                Console.WriteLine($"[{i}] Response: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[{i}] Content Length: {responseContent.Length}");
            }

            Console.WriteLine("All requests completed");
        }
    }
}
