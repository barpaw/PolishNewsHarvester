using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Polly;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PolishNewsHarvesterSdk.Consts;
using PolishNewsHarvesterWorker;
using PolishNewsHarvesterSdk.Http;
using PolishNewsHarvesterCommon.HarvesterMethods;
using PolishNewsHarvesterSdk.NewsSites;

namespace PolishNewsHarvesterApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Configuration["app:name"], Version = "v1" });
            });

            //HttpClient
            services
                .AddHttpClient(HttpClients.DefaultClient,
                    client => { client.DefaultRequestHeaders.UserAgent.TryParseAdd(UserAgents.GoogleBot); })
                .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(3,
                    retryCount => TimeSpan.FromMilliseconds(300), (result, timeSpan, retryCount, context) =>
                    {
                        if (result.Exception != null)
                        {
                            Log.Error(result.Exception, "An exception occurred on retry {RetryAttempt} for {PolicyKey}",
                                retryCount, context.PolicyKey);
                        }
                        else
                        {
                            Log.Error(
                                "A non success code {StatusCode} was received on retry {RetryAttempt} for {PolicyKey}",
                                (int)result.Result.StatusCode, retryCount, context.PolicyKey);
                        }
                    }));

            //SqlServer
            /*
            services.AddDbContext<PolishNewsHarvesterContext>(options =>
            {
                var connectionString = Configuration["SqlServer:ConnectionString"];

                options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                });

                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();


            });
            */


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    var corsOrigin = Configuration["WebApp:cors"];

                    builder
                        .WithOrigins(corsOrigin)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod();
                });
            });


            services.AddMemoryCache();

            services.AddTransient<IMethods, Methods>();
            services.AddTransient<IWirtualnaPolska, WirtualnaPolska>();
            services.AddTransient<IPolskaAgencjaPrasowa, PolskaAgencjaPrasowa>();
            services.AddTransient<ITvpInfo, TvpInfo>();
            services.AddTransient<IGazeta, Gazeta>();
            services.AddTransient<IHttpManager, HttpManager>();
            services.AddHostedService<Worker>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Configuration["app:name"]} v1"));
            }

            app.UseCors();

            // app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}