using Microsoft.EntityFrameworkCore;
using VINATECH.Models;

namespace VINATECH.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }
    }
}
