using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    new Section
                        {
                            Title = "Sport",
                            Topics = new Collection<Topic>
                                {
                                    new Topic {Title = "Football"},
                                    new Topic {Title = "Chess"},
                                }
                        },
                    new Section
                        {
                            Title = "Life",
                            Topics = new Collection<Topic>
                                {
                                    new Topic {Title = "Love"},
                                    new Topic {Title = "House"},
                                }
                        },
                    new Section
                        {
                            Title = "News",
                            Topics = new Collection<Topic>
                                {
                                    new Topic {Title = "Science news"}
                                }
                        }
                };
            sections.ForEach(section => context.Sections.Add(section));
            context.SaveChanges();
        }   
    }
}