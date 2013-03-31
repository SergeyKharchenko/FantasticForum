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
                    new Section
                        {
                            Title = "Sport",
                            Image = new Image
                                {
                                    FileName = "/Images/Section/section-sport.jpg",
                                    ImageMimeType = "image/jpg"
                                }
                        },
                    new Section
                        {
                            Title = "Life",
                            Image = new Image
                                {
                                    FileName = "/Images/Section/section-life.jpg",
                                    ImageMimeType = "image/jpg"
                                }
                        },
                    new Section
                        {
                            Title = "News",
                            Image = new Image
                                {
                                    FileName = "/Images/Section/section-news.png",
                                    ImageMimeType = "image/png"
                                }
                        }
                };
            sections.ForEach(section => context.Sections.Add(section));
            context.SaveChanges();
        }   
    }
}