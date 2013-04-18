using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq.Expressions;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;
using System.Linq;

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
                    new Section {Title = "Sport", Topics = new Collection<Topic>
                        {
                            new Topic {Title = "Topic 1"},
                            new Topic {Title = "Topic 2"},
                        }},
                    new Section {Title = "Life", Topics = new Collection<Topic>
                        {
                            new Topic {Title = "Topic 3"},
                        }},
                    new Section {Title = "News", Topics = new Collection<Topic>
                        {
                            new Topic {Title = "Topic 4"},
                            new Topic {Title = "Topic 5"},
                            new Topic {Title = "Topic 6"},
                        }}
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
                                                new Topic {Title = "Topic 3"}
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
                                                new Topic {Title = "Topic 2"},
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
            Assert.That(actualList.Count, Is.EqualTo(expectedResult.Count));

            for (var i = 0; i < actualList.Count; i++)
            {
                Assert.That(actualList[i].Title, Is.EqualTo(expectedResult[i].Title));

                for (var j = 0; j < actualList[i].Topics.Count; j++)
                    Assert.That(actualList[i].Topics[j].Title, Is.EqualTo(expectedResult[i].Topics[j].Title));
            }
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
            var id = context.Sections.First().Id;
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
        public void RemoveTest()
        {
            var id = context.Sections.First().Id;
            var section = repository.GetById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Sport"));
            repository.Remove(section);

            section = repository.GetById(id);

            Assert.That(section, Is.Null);
        }

        [Test]
        public void RemoveByIdTest()
        {
            var id = context.Sections.First().Id;
            var section = repository.GetById(id);
            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Sport"));
            repository.Remove(id);

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
            context.Sections.ToList().ForEach(section =>
                {
                    for (var i = section.Topics.Count - 1; i >= 0; i--)
                        context.Topics.Remove(section.Topics[i]);
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