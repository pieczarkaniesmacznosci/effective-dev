using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using ArticlesGrpcClient;

namespace ArticlesClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            AppContext.SetSwitch(
                "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            //The port number must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("http://localhost:5000");
            var client = new ArticlesBasicService.ArticlesBasicServiceClient(channel);
            var reply = await client.CreateArticleAsync(new Article {Author = "Author" });
            Console.WriteLine("Greeting: " + reply.Id);
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
