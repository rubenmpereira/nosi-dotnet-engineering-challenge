using Microsoft.EntityFrameworkCore;
using NOS.Engineering.Challenge.Models;
using System.ComponentModel;

namespace NOS.Engineering.Challenge.Database
{
    public class ContentDbContext : DbContext
    {
        public ContentDbContext()
        {

        }
        public ContentDbContext(DbContextOptions<ContentDbContext> options) : base(options)
        {

        }

        public DbSet<Content> Contents { get; set; }

        public virtual void SetModified(object entity,object newEntity)
        {
            Entry(entity).CurrentValues.SetValues(newEntity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Content>()
            .Property(e => e.GenreList)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));

            base.OnModelCreating(modelBuilder);
        }

    }
}
