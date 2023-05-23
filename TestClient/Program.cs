using ClansGrpcService.Protos;
using Grpc.Net.Client;

namespace TestClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7042");
            var client = new Clan.ClanClient(channel);
            //var reply1 = await client.AddClanAsync(new AddClanRequest() { Name = "Sk", Leader = "Adrian", Location = new Location() });
            //var reply2 = await client.AddClanAsync(new AddClanRequest() { Name = "Porsgrunn", Leader = "Adrian", Location = new Location() });
            var reply3 = await client.AddClanAsync(new AddClanRequest() { Name = "Mo I Rana", Leader = "TestAdrian", Location = new Location() { World = "test" } });

            Console.ReadKey();
        }
    }
}