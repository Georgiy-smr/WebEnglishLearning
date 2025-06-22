using System.Collections.Generic;
using System.Reflection.Emit;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace ContextDataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>().HasKey(log => log.Id);
            modelBuilder.Entity<Word>().HasIndex(log => log.Id);
            modelBuilder.Entity<Word>().HasQueryFilter(p => !p.SoftDeleted);

            modelBuilder.Entity<User>().HasKey(log => log.Id);
            modelBuilder.Entity<User>().HasIndex(log => log.Id);
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.SoftDeleted);
        }

        public DbSet<Word> Words { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
