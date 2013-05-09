using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Models
{
    public class ForumContext : DbContext
    {
        public ForumContext()
            : base("ForumConnection")
        {
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Topic>()
                        .HasRequired(topic => topic.Section)
                        .WithMany(section => section.Topics)
                        .HasForeignKey(topic => topic.SectionId);

            modelBuilder.Entity<Record>()
                        .HasRequired(record => record.Topic)
                        .WithMany(section => section.Records)
                        .HasForeignKey(record => record.TopicId);

            modelBuilder.Entity<Record>()
                        .HasRequired(record => record.User)
                        .WithMany(section => section.Records)
                        .HasForeignKey(record => record.UserId);
        }
    }
}