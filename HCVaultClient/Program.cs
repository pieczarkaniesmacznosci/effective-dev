using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Net.Http.Headers;

class Program
{
    static void Main(string[] args)
    {
        // Setup Host
        var host = CreateDefaultBuilder().Build();

        // Invoke Worker
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var workerInstance = provider.GetRequiredService<Worker>();
        workerInstance.DoWork();

        host.Run();
    }

    static IHostBuilder CreateDefaultBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appSettings.json");
                app.AddEnvironmentVariables(prefix: "VAULT_");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<Worker>();
            });
    }
}


internal class Worker
{
    private readonly IConfiguration configuration;

    public Worker(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Task> DoWork()
    {
        var keyValuePairs = configuration.AsEnumerable().ToList();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("==============================================");
        Console.WriteLine("Configurations...");
        Console.WriteLine("==============================================");
        foreach (var pair in keyValuePairs)
        {
            Console.WriteLine($"{pair.Key} - {pair.Value}");
        }
        Console.WriteLine("==============================================");
        Console.ResetColor();

        var client = new RestClient(configuration.GetSection("Vault").GetSection("Address").Value);
        var request = new RestRequest(configuration.GetSection("Vault").GetSection("MountPath").Value)
            .AddHeader("X-Vault-Token", value: Environment.GetEnvironmentVariable("VAULT_TOKEN", EnvironmentVariableTarget.Machine))
            .AddHeader("Accept", "*/*")
            .AddHeader("Connection","keep-alive");
        var response = client.Get(request);

        Console.ForegroundColor = ConsoleColor.Yellow;// Making the HTTP Get call to consult our Secret
        JObject json = JObject.Parse(response.Content);
        // Printing the response
        // Storing the key-value pairs of our secret from the response
        JToken secrets = json["data"]["data"];
        // Validating the previous statement is true
        // Storing our key-value pairs to a Dictionary for future data manipulation
        Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(secrets.ToString());
        // Looping through our key-value pairs

        Console.WriteLine("==============================================");
        Console.ForegroundColor = ConsoleColor.Red;
        foreach (var item in values)
        {
            // Printing our key-value pairs
            Console.WriteLine($"Key: {item.Key} Value: {item.Value}");
        }
        return Task.CompletedTask;
    }
}