using System;
using System.Collections.Generic;
using System.IO;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests.Repository
{
    [TestFixture]
    public class RepositoryTests
    {
        private IRepository<Section> repository;
        private ForumContext context;
        private List<Section> sections;
            
        [TestFixtureSetUp]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Directory.GetCurrentDirectory());

            context = new ForumContext();
            if (context.Database.Exists())
                context.Database.Delete();
            context.Database.Initialize(true);

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

            Assert.That(actualSections, Is.EquivalentTo(sections));
        }

        [Test]
        public void GetByIdTest()
        {
            var section = GetSectionById(2);

            Assert.That(section.Id, Is.EqualTo(2));
            Assert.That(section.Title, Is.EqualTo("Life"));
        }

        [Test]
        public void CreateTest()
        {
            var section = new Section {Title = "Love"};
            repository.Create(section);
            var id = section.Id;

            section = GetSectionById(id);

            Assert.That(section.Id, Is.EqualTo(id));
            Assert.That(section.Title, Is.EqualTo("Love"));
        }

        [Test]
        public void RemoveTest()
        {
            var section = repository.GetById(1);
            Assert.That(section.Id, Is.EqualTo(1));
            Assert.That(section.Title, Is.EqualTo("Sport"));
            repository.Remove(section);
            repository.SaveChanges();

            section = repository.GetById(1);

            Assert.That(section, Is.Null);
        }

        [Test]
        public void RemoveByIdTest()
        {
            var section = repository.GetById(3);
            Assert.That(section.Id, Is.EqualTo(3));
            Assert.That(section.Title, Is.EqualTo("News"));
            repository.Remove(3);
            repository.SaveChanges();

            section = repository.GetById(3);

            Assert.That(section, Is.Null);
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            context.Database.Delete();
            context.Dispose();
        }

        public Section GetSectionById(int id)
        {
            return repository.GetById(id);
        }
    }
}