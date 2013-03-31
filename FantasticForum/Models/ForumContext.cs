using System.Data.Entity;

namespace Models
{
    public class ForumContext : DbContext
    {
        public ForumContext() : base("ForumConnection")
        {            
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Section>()
                        .HasOptional(section => section.Image)
                        .WithOptionalDependent().Map(t => t.MapKey("ImageId"));
        }
    }
}