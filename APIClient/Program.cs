// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var httpClinet = new HttpClient();

httpClinet.BaseAddress = new Uri("http://localhost:5225/api/Articles/create");
var i = 0;
var max = 300;

while (i < max)
{
    var response = await httpClinet.GetAsync("http://localhost:5225/api/Articles/create");
    var result = await response.Content.ReadAsStringAsync();
    Console.WriteLine(result);

    Thread.Sleep(1000);
    i++;
}