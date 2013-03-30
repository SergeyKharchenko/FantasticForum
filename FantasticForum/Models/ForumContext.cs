using System.Data.Entity;

namespace Models
{
    public class ForumContext : DbContext
    {
        public ForumContext() : base("ForumConnection")
        {            
        }

        public DbSet<Section> Sections { get; set; }
    }
}