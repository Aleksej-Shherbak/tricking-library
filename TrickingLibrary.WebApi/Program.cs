using System.Collections.Generic;
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
                    ctx.Add(new Difficulty() { Id = "easy", Name = "Easy", Description = "Very easy" });
                    ctx.Add(new Difficulty() { Id = "medium", Name = "Medium", Description = "Pretty medium" });
                    ctx.Add(new Difficulty() { Id = "hard", Name = "Hard", Description = "Very Hard" });
                    
                    ctx.Add(new Category() { Id = "kick", Name = "Kick", Description = "Kick test" });
                    ctx.Add(new Category() { Id = "flip", Name = "Flip", Description = "Flip test" });
                    ctx.Add(new Category() { Id = "transition", Name = "Transition", Description = "Transition test" });
                    
                    ctx.Add(new Trick
                    {
                        Id = "backwards-roll",
                        Name = "Backwards roll",
                        Description = "This is a test Backwards roll",
                        Difficulty = "easy",
                        TrickCategories = new List<TrickCategory>
                        {
                            new TrickCategory
                            {
                                CategoryId = "flip"
                            }
                        },
                    });
                    
                    ctx.Add(new Trick
                    {
                        Id = "back-flip",
                        Name = "Back flip",
                        Description = "This is a test back flip",
                        Difficulty = "medium",
                        TrickCategories = new List<TrickCategory>
                        {
                            new TrickCategory
                            {
                                CategoryId = "flip"
                            }
                        },
                        Prerequisites = new List<TrickRelationship>
                        {
                            new TrickRelationship
                            {
                                PrerequisiteId = "backwards-roll" 
                            }
                        }
                    });

                    ctx.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Test description. I've tried to go for max height",
                        Video = "slow_backfip.mp4",
                        IsVideoProcessed = true,
                    });
                    
                    ctx.Add(new Submission
                    {
                        TrickId = "back-flip",
                        Description = "Test description.",
                        Video = "backflip.mp4",
                        IsVideoProcessed = true,
                    });
                    
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