using System;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace Mvc.Infrastructure.DAL
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

            context.Users.Add(new User {Email = "a@a.com", Password = "123", IsConfirmed = true});
            context.Users.Add(new User {Email = "a@b.com", Password = "123", IsConfirmed = true});

            context.SaveChanges();

            context.Records.Add(new Record{Text = "First post", TopicId = 1, UserId = 1, CreationDate = DateTime.Now - TimeSpan.FromMinutes(2)});
            context.Records.Add(new Record { Text = "Second post", TopicId = 1, UserId = 2, CreationDate = DateTime.Now - TimeSpan.FromMinutes(1) });
            context.Records.Add(new Record { Text = "Third post", TopicId = 1, UserId = 1, CreationDate = DateTime.Now});

            context.SaveChanges();
        }   
    }
}