using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TrickingLibrary.Data;
using TrickingLibrary.Entities;

namespace TrickingLibrary.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

                if (env.IsDevelopment())
                {
                    ctx.Difficulties.Add(new Difficulty() {Id = "easy", Name = "Easy", Description = "Very easy"});
                    ctx.Difficulties.Add(new Difficulty() {Id = "hard", Name = "Hard", Description = "Very Hard"});
                    
                    ctx.Categories.Add(new Category() {Id = "kick", Name = "Kick", Description = "Kick test"});
                    ctx.Categories.Add(new Category() {Id = "flip", Name = "Flip", Description = "Flip test"});
                    ctx.Categories.Add(new Category() {Id = "transition", Name = "Transition", Description = "Transition test"});
                    ctx.SaveChanges();
                }
            }
            
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}