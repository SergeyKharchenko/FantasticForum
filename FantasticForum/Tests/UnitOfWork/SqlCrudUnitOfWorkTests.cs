using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using Models;
using Moq;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;
using System.Linq;

namespace Tests.UnitOfWork
{
    [TestFixture]
    public class SqlCrudUnitOfWorkTests
    {
        private Mock<IRepository<Section>> repositoryMock;
        private SqlCrudUnitOfWork<Section> unitOfWork;

        [SetUp]
        public void SetUp()
        {
            repositoryMock = new Mock<IRepository<Section>>();
            unitOfWork = new SqlCrudUnitOfWork<Section>(repositoryMock.Object);
        }

        [Test]
        public void CreateTest()
        {
            var section = new Section {Title = "1"};
            repositoryMock.Setup(repo => repo.Create(section))
                .Callback((Section s) => s.Id = 1)
                .Returns(section);

            var entity = unitOfWork.Create(section);

            repositoryMock.Verify(repo => repo.Create(section), Times.Once());
            Assert.That(entity, Is.EqualTo(section));
        }

        [Test]
        public void GetAllTest()
        {
            var sections = new List<Section>
                {
                    new Section {Title = "1"},
                    new Section {Title = "2"},
                    new Section {Title = "3"}
                };

            repositoryMock.Setup(repo => repo.Entities).Returns(sections);

            var entities = unitOfWork.Entities;

            repositoryMock.Verify(repo => repo.Entities, Times.Once());
            Assert.That(entities, Is.EquivalentTo(sections));
        }

        [Test]
        public void ReadTest()
        {
            var section = new Section { Title = "1" };
            repositoryMock.Setup(repo => repo.GetById(42)).Returns(section);

            var entity = unitOfWork.Read(42);

            repositoryMock.Verify(repo => repo.GetById(42), Times.Once());
            Assert.That(entity, Is.EqualTo(section));
        }

        [Test]
        public void ReadByConditionTest()
        {
            var section = new Section { Title = "1" };
            var expression = (Expression<Func<Section, bool>>) (s => s.Title == "1");
            repositoryMock.Setup(repo => repo.Get(expression, ""))
                .Returns(new List<Section> { section });

            var entity = unitOfWork.Read(expression).First();

            repositoryMock.Verify(repo => repo.Get(expression, ""), Times.Once());
            Assert.That(entity, Is.EqualTo(section));
        }

        [Test]
        public void UpdateTest()
        {
            var section = new Section { Title = "1" };
            repositoryMock.Setup(repo => repo.Update(section)).Returns(section);

            var crudResult = unitOfWork.Update(section);

            repositoryMock.Verify(repo => repo.Update(section), Times.Once());
            Assert.That(crudResult.Success, Is.True);
            Assert.That(crudResult.Entity, Is.EqualTo(section));
        }

        [Test]
        public void UpdateWithConcurrencyExceptionTest()
        {
            var section = new Section { Title = "1" };
            repositoryMock.Setup(repo => repo.Update(section)).Throws<DbUpdateConcurrencyException>();

            var crudResult = unitOfWork.Update(section);

            repositoryMock.Verify(repo => repo.Update(section), Times.Once());
            Assert.That(crudResult.Success, Is.False);
            Assert.That(crudResult.Entity, Is.Null);
        }

        [Test]
        public void DeleteTest()
        {
            var section = new Section { Title = "1" };
            repositoryMock.Setup(repo => repo.Remove(section)).Returns(section);

            var crudResult = unitOfWork.Delete(section);

            repositoryMock.Verify(repo => repo.Remove(section), Times.Once());
            Assert.That(crudResult.Success, Is.True);
            Assert.That(crudResult.Entity, Is.EqualTo(section));
        }

        [Test]
        public void DeleteWithConcurrencyExceptionTest()
        {
            var section = new Section { Title = "1" };
            repositoryMock.Setup(repo => repo.Remove(section)).Throws<DbUpdateConcurrencyException>();

            var crudResult = unitOfWork.Delete(section);

            repositoryMock.Verify(repo => repo.Remove(section), Times.Once());
            Assert.That(crudResult.Success, Is.False);
            Assert.That(crudResult.Entity, Is.Null);
        }

        [Test]
        public void DeleteByIdTest()
        {
            var section = new Section {Title = "1"};
            repositoryMock.Setup(repo => repo.Remove(1)).Returns(section);

            var crudResult = unitOfWork.Delete(1);

            repositoryMock.Verify(repo => repo.Remove(1), Times.Once());
            Assert.That(crudResult.Success, Is.True);
            Assert.That(crudResult.Entity, Is.EqualTo(section));
        }

        [Test]
        public void DeleteByIdWithConcurrencyExceptionTest()
        {
            repositoryMock.Setup(repo => repo.Remove(1)).Throws<DbUpdateConcurrencyException>();

            var crudResult = unitOfWork.Delete(1);

            repositoryMock.Verify(repo => repo.Remove(1), Times.Once());
            Assert.That(crudResult.Success, Is.False);
            Assert.That(crudResult.Entity, Is.Null);
        }
    }
}