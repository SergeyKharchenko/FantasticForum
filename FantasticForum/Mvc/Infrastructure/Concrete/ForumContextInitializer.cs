using System.Collections.Generic;
using System.Data.Entity;
using Models;

namespace Mvc.Infrastructure.Concrete
{
    public class ForumContextInitializer : DropCreateDatabaseAlways<ForumContext>
    {
        protected override void Seed(ForumContext context)
        {
            var sections = new List<Section>
                {
                    new Section {Title = "Sport"},
                    new Section {Title = "Life"},
                    new Section {Title = "News"}
                };
            sections.ForEach(section => context.Sections.Add(section));
            context.SaveChanges();
        }   
    }
}