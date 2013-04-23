using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;
using System.Linq;
using Assert = NUnit.Framework.Assert;

namespace Tests.Repository
{
    [TestFixture]
    public class SqlRepositoryTests
    {
        private IRepository<Section> repository;
        private ForumContext context;
        private static List<Section> sections;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());

            context = new ForumContext();
            if (context.Database.Exists())
                context.Database.Delete();
            context.Database.Initialize(true);
        }

        [SetUp]
        public void SetUp()
        {
            sections = new List<Section>
                {
                    new Section
                        {
                            Title = "Sport",
                            Topics = new Collection<Topic>
                                {
                                    new Topic {Title = "Topic 1"},
                                    new Topic {Title = "Topic 2"}
                                }
                        },
                    new Section
                        {
                            Title = "Life",
                            Topics = new Collection<Topic>
                                {
                                    new Topic {Title = "Topic 3"},
                                }
                        },
                    new Section
                        {
                            Title = "News",
                            Topics = new Collection<Topic>
                                {
                                    new Topic {Title = "Topic 4"},
                                    new Topic {Title = "Topic 5"},
                                    new Topic {Title = "Topic 6"},
                                }
                        }
                };
            sections.ForEach(section => context.Sections.Add(section));
            context.SaveChanges();

            repository = new SqlRepository<Section>(context);
        }

        [Test]
        public void GetAllEntitiesTest()
        {
            var actualSections = repository.Entities;

            Assert.That(actualSections, Is.EquivalentTo(sections));
        }

        [Test]
        public void GetByIdTest()
        {
            var id = context.Sections.First().Id;
            var section = GetEntityById(id);

            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Sport"));
        }

        private readonly object[] getData = new object[]
            {
                new object[]
                    {
                        (Expression<Func<Section, bool>>) (section => section.Title == "Life"),
                        new List<Section>
                            {
                                new Section
                                    {
                                        Title = "Life",
                                        Topics = new Collection<Topic>
                                            {
                                                new Topic {Title = "Topic 3", SectionId = 2}
                                            }
                                    }
                            }
                    },
                new object[]
                    {
                        null,
                        new List<Section>
                            {
                                new Section
                                    {
                                        Title = "Sport",
                                        Topics = new Collection<Topic>
                                            {
                                                new Topic {Title = "Topic 1"},
                                                new Topic {Title = "Topic 2"}
                                            }
                                    },
                                new Section
                                    {
                                        Title = "Life",
                                        Topics = new Collection<Topic>
                                            {
                                                new Topic {Title = "Topic 3"}
                                            }
                                    },
                                new Section
                                    {
                                        Title = "News",
                                        Topics = new Collection<Topic>
                                            {
                                                new Topic {Title = "Topic 4"},
                                                new Topic {Title = "Topic 5"},
                                                new Topic {Title = "Topic 6"},
                                            }
                                    }
                            }
                    }
            };

        [Test, TestCaseSource("getData")]
        public void GetTest(Expression<Func<Section, bool>> filter, List<Section> expectedResult)
        {
            var actualResult = repository.Get(filter);

            var actualList = actualResult.ToList();
            Assert.That(actualList, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public void CreateTest()
        {            
            var section = new Section {Title = "Study"};

            repository.Create(section);
            var id = section.Id;

            section = GetEntityById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Study"));
        }

        [Test]
        public void UpdateTest()
        {
            var id = context.Sections.First(s => s.Title == "Sport").Id;
            var section = GetEntityById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Sport"));

            section.Title = "Games";

            repository.Update(section);

            section = GetEntityById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Games"));
        }

        [Test]
        public void UpdateWithConcurrencyExceptionTest()
        {            
            var id = context.Sections.First(s => s.Title == "Sport").Id;
            var section1 = GetEntityById(id);
            Assert.That(section1.Id, Is.EqualTo(id));
            Assert.That(section1.Title, Is.EqualTo("Sport"));
            var section2 = section1.Clone() as Section;

            section1.Title = "Games";

            repository.Update(section1);

            section1 = GetEntityById(id);
            Assert.That(section1.Id, Is.EqualTo(id));
            Assert.That(section1.Title, Is.EqualTo("Games"));

            section2.Title = "War";
            
            Assert.Throws<DbUpdateConcurrencyException>(() => repository.Update(section2));       
        }

        [Test]
        public void RemoveTest()
        {
            var id = context.Sections.First(s => s.Title == "Sport").Id;
            var section = repository.GetById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Sport"));

            for (var i = section.Topics.Count - 1; i >= 0; i--)
                context.Topics.Remove(section.Topics[i]);
            context.SaveChanges();
            repository.Remove(section);

            section = repository.GetById(id);

            Assert.That(section, Is.Null);
        }

        [Test]
        public void RemoveWithConcurrencyExceptionTest()
        {
            var id = context.Sections.First(s => s.Title == "Sport").Id;
            var section1 = GetEntityById(id);
            Assert.That(section1.Id, Is.EqualTo(id));
            Assert.That(section1.Title, Is.EqualTo("Sport"));
            var section2 = section1.Clone() as Section;

            section1.Title = "Games";

            repository.Update(section1);

            section1 = GetEntityById(id);
            Assert.That(section1.Id, Is.EqualTo(id));
            Assert.That(section1.Title, Is.EqualTo("Games"));

            for (var i = section2.Topics.Count - 1; i >= 0; i--)
                context.Topics.Remove(section2.Topics[i]);
            context.SaveChanges();
            Assert.Throws<DbUpdateConcurrencyException>(() => repository.Remove(section2));
        }

        [Test]
        public void RemoveByIdTest()
        {
            var id = context.Sections.First(s => s.Title == "Sport").Id;
            var section = repository.GetById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Sport"));

            for (var i = section.Topics.Count - 1; i >= 0; i--)
                context.Topics.Remove(section.Topics[i]);
            context.SaveChanges();
            repository.Remove(id);
            context.SaveChanges();

            section = repository.GetById(id);

            Assert.That(section, Is.Null);
        }

        public Section GetEntityById(int id)
        {
            return repository.GetById(id);
        }

        [TearDown]
        public void TearDown()
        {
            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            objectContext.Refresh(RefreshMode.StoreWins,
                                  objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Modified | EntityState.Deleted)
                                               .Select(x => x.Entity));

            context.Sections.ToList().ForEach(section =>
                {
                    if (section.Topics != null)
                    {
                        for (var i = section.Topics.Count - 1; i >= 0; i--)
                            context.Topics.Remove(section.Topics[i]);
                        context.SaveChanges();
                    }

                    context.Sections.Remove(section);
                });
            context.SaveChanges();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            context.Database.Delete();
            context.Dispose();
        }
    }
}