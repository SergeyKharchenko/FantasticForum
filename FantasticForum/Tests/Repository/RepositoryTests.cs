using System;
using System.Collections.Generic;
using System.IO;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;
using System.Linq;

namespace Tests.Repository
{
    [TestFixture]
    public class RepositoryTests
    {
        private IRepository<Section> repository;
        private ForumContext context;
        private List<Section> sections;

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
                    new Section {Title = "Sport"},
                    new Section {Title = "Life"},
                    new Section {Title = "News"}
                };
            sections.ForEach(section => context.Sections.Add(section));
            context.SaveChanges();

            repository = new Repository<Section>(context);
        }

        [Test]
        public void GetAllEntitiesTest()
        {
            var actualSections = repository.Entities;

            foreach (var section in sections)
            {
                Console.WriteLine(section.Id);
            }
            foreach (var section in actualSections)
            {
                Console.WriteLine(section.Id);
            }

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

        [Test]
        public void CreateOrUpdateNewEntityTest()
        {            
            var section = new Section {Title = "Study"};

            repository.CreateOrUpdate(section);
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

            repository.CreateOrUpdate(section);

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
            context.Sections.ToList().ForEach(section => context.Sections.Remove(section));
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