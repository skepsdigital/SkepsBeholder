using MongoDB.Driver;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using SkepsBeholder.Services;
using SkepsBeholder.Services.Email;
using SkepsBeholder.Services.Interfaces;
using SkepsBeholder.Services.Mongo;
using RestEase;
using SkepsBeholder.Infra;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IMongoClient>(serviceProvider => new MongoClient("mongodb+srv://admin:flowchat123456@cluster0.dwcxd.mongodb.net"));
        builder.Logging.ClearProviders().AddConsole().AddFilter("System.Net.Http.HttpClient", LogLevel.None);

        builder.Services.AddSingleton<IEmail, Email>();
        builder.Services.AddSingleton<IMongoService, MongoService>();
        builder.Services.AddSingleton<IProcess, Process>();

        builder.Services.AddHttpClient("BlipBeholderAPI", client =>
        {
            client.BaseAddress = new Uri("https://take-api-beholder.cs.blip.ai/");
        });

        builder.Services.AddSingleton(serviceProvider =>
        {
            var clientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
            var httpClient = clientFactory.CreateClient("BlipBeholderAPI");
            return RestClient.For<IBlipSender>(httpClient);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // Erros transitórios: 5xx e 408
            .OrResult(msg => !msg.IsSuccessStatusCode) // Retenta para status HTTP não bem-sucedidos
            .WaitAndRetryAsync(
                retryCount: 3, // Número de tentativas
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // Backoff exponencial
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    Console.WriteLine($"Tentativa {retryAttempt} falhou. Retentando em {timespan.Seconds} segundos...");
                });
    }
}