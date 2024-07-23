using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SpiceCraft.Server.DB
{
    public class SpiceCraftDbContext : DbContext
    {
        public SpiceCraftDbContext(DbContextOptions<SpiceCraftDbContext> options) : base(options)
        {
        }

        // Define your DbSets here
       // public DbSet<YourEntity> YourEntities { get; set; }
    }
}
