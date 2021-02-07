using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Function;
using System;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.EventStore;
using Microsoft.AspNetCore.Http;

public class Startup
{
    private IStore? _store;

    private IStore Store => _store ?? throw new InvalidOperationException("Event Store is not initialized.");
    
    public void ConfigureServices(IServiceCollection services)
    {
        _store = EventStoreBuilder
            .NewUsing(
                Environment.GetEnvironmentVariable("EventStore_ConnectionString") 
                ?? throw new ArgumentException("EventStore connection string is not provided through environment variable 'EventStore_ConnectionString'."))
            .NewStore();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.Run(async (context) =>
        {
            if (context.Request.Path != "/")
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 - Not Found");
                return;
            }

            if (context.Request.Method != "POST")
            {
                context.Response.StatusCode = 405;
                await context.Response.WriteAsync("405 - Only POST method allowed");
                return;
            }

            try
            {
                var (status, text) = await new FunctionHandler(Store).Handle(context.Request);
                context.Response.StatusCode = status;
                if (!string.IsNullOrEmpty(text))
                    await context.Response.WriteAsync(text);
            }
            catch (NotImplementedException nie)
            {
                context.Response.StatusCode = 501;
                await context.Response.WriteAsync(nie.ToString());
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(ex.ToString());
            }
        });
    }
}