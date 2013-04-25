using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Models
{
    public class ForumContext : DbContext
    {
        public ForumContext() : base("ForumConnection")
        {            
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Record> Records { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Topic>()
                        .HasRequired(topic => topic.Section)
                        .WithMany(section => section.Topics)
                        .HasForeignKey(topic => topic.SectionId);

            modelBuilder.Entity<Record>()
                        .HasRequired(record => record.Topic)
                        .WithMany(section => section.Records)
                        .HasForeignKey(record => record.TopicId);
        }
    }
}