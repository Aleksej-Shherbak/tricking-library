using Microsoft.EntityFrameworkCore;
using TrickingLibrary.Entities;

namespace TrickingLibrary.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Trick> Tricks { get; set; }
    }
}