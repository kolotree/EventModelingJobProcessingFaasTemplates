using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Function;
using System;
using JobProcessing.Abstractions;
using JobProcessing.Infrastructure.EventStore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

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
        app.Run(async context =>
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
                var functionResult = await new FunctionHandler(Store).Handle(context.Request);
                context.Response.StatusCode = functionResult.StatusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(functionResult.ResponseJson());
            }
            catch (NotImplementedException nie)
            {
                context.Response.StatusCode = 501;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { nie.Message }));
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(new {
                        ex.Message,
                        ex.StackTrace,
                        InnerException = ex.InnerException != null ? new { ex.InnerException.Message, ex.InnerException.StackTrace } : null
                }));
            }
        });
    }
}