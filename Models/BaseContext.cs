using Microsoft.EntityFrameworkCore;

//Have to restructure the whole program to use this, but it may be worth it to avoid writing massive data access layers.

namespace tephraSystemEditor.Models
{
    public class BaseContext : DbContext
    {
        public DbSet<Character> Characters {get; set;}
        public DbSet<Specialty> Specialties {get; set;}
        public DbSet<Bonus> Bonuses {get; set;}
        public BaseContext(DbContextOptions<DbContext> options) : base(options)
        {       
        }
    }
}