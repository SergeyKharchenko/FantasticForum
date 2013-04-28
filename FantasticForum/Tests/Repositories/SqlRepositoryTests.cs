using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.IO;
using System.Linq.Expressions;
using Models;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.DAL.Cocnrete;
using NUnit.Framework;
using System.Linq;

namespace Tests.Repositories
{
    [TestFixture]
    public class SqlRepositoryTests
    {
        private IRepository<Section> repository;
        private ForumContext context;
        private static readonly List<Section> sections = new List<Section>
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
                        new List<Section> {sections.First(section => section.Title == "Life")}
                    },
                new object[]
                    {
                        null,
                        sections
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
            var section = context.Sections.First(s => s.Title == "Sport");
            var id = section.Id;
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
            var section1 = context.Sections.First();
            var section2 = section1.Clone() as Section;

            repository.Update(section1);

            Assert.Throws<DbUpdateConcurrencyException>(() => repository.Update(section2));       
        }

        [Test]
        public void RemoveTest()
        {
            var id = context.Sections.First().Id;
            var section = repository.GetById(id);            

            RemoveSection(section);

            var newSection = repository.GetById(id);
            Assert.That(section, Is.Not.Null);
            Assert.That(newSection, Is.Null);
        }

        [Test]
        public void RemoveWithConcurrencyExceptionTest()
        {
            var section1 = context.Sections.First();
            var section2 = section1.Clone() as Section;

            repository.Update(section1);

            Assert.Throws<DbUpdateConcurrencyException>(() => RemoveSection(section2));
        }

        [Test]
        public void RemoveByIdTest()
        {
            var section = context.Sections.First();
            var id = section.Id;

            RemoveTopicsFromSection(section);
            repository.Remove(id);

            var newSection = repository.GetById(id);
            Assert.That(section, Is.Not.Null);
            Assert.That(newSection, Is.Null);
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

            context.Sections.ToList().ForEach(RemoveSection);
            context.SaveChanges();
        }

        private void RemoveSection(Section section)
        {
            //RemoveTopicsFromSection(section);
            repository.Remove(section);
        }

        private void RemoveTopicsFromSection(Section section)
        {
            if (section.Topics == null)
                return;
            for (var i = section.Topics.Count - 1; i >= 0; i--)
                context.Topics.Remove(section.Topics[i]);
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