using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace Models
{
    public class ForumContext : DbContext
    {
        public ForumContext() : base("ForumConnection")
        {
            var contextAdapter = ((IObjectContextAdapter)this);
            contextAdapter.ObjectContext
                          .ObjectStateManager
                          .ObjectStateManagerChanged += ObjectStateManagerChanged;
        }

        public DbSet<Section> Sections { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Record> Records { get; set; }
        public DbSet<User> Users { get; set; }

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

            modelBuilder.Entity<Record>()
                        .HasRequired(record => record.User)
                        .WithMany(section => section.Records)
                        .HasForeignKey(record => record.UserId);
        }

        private void ObjectStateManagerChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action != CollectionChangeAction.Remove) 
                return;

            if (e.Element is Section)
            {
                var section = e.Element as Section;
                var topics = Topics.Where(topic => topic.SectionId == section.Id);
                foreach (var topic in topics)
                    Topics.Remove(topic);
            }
            if (e.Element is Topic)
            {
                var topic = e.Element as Topic;
                var records = Records.Where(record => record.TopicId == topic.Id);
                foreach (var record in records)
                    Records.Remove(record);
            }
            if (e.Element is User)
            {
                var user = e.Element as User;
                var records = Records.Where(record => record.UserId == user.Id);
                foreach (var record in records)
                    Records.Remove(record);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}