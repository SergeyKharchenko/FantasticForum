using System.ComponentModel;
using System.Data;
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
                var c = e.Element as Topic;
                var records = Records.Where(record => record.TopicId == record.Id);
                foreach (var record in records)
                    Records.Remove(record);
            }
        }
    }
}