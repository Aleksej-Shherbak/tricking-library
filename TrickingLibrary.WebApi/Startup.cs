using System.Threading.Channels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TrickingLibrary.Data;
using TrickingLibrary.WebApi.BackgroundServices;
using TrickingLibrary.WebApi.Middlewares;
using TrickingLibrary.WebApi.Services;

namespace TrickingLibrary.WebApi
{
    public class Startup
    {
        private const string AllCorsPolicyName = "All";
            
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Tricking library API", Version = "v1"}); });

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Dev"));

            services.AddHostedService<VideoProcessingBackgroundService>();
            services.AddSingleton(_ => Channel.CreateUnbounded<ProcessVideoMessage>());

            services.AddSingleton<VideoManager>();
            services.AddScoped<SubmissionService>();

            services.AddCors(opt =>
                opt.AddPolicy(AllCorsPolicyName, build =>
                    build.AllowAnyHeader()
                    .AllowAnyOrigin()
                    .AllowAnyMethod()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseCors(AllCorsPolicyName);
            
            app.UseExceptionHandlerMiddleware();
            
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}