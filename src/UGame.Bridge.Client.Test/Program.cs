using Microsoft.Extensions.Hosting;
using AiUo;
using UGame.Bridge.Client;

AiUoHost.CreateBuilder().Build().UseAiUo().Start();

var client = new XxyyProviderClient();
await client.AppUrl(null);