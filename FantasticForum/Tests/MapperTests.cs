using System;
using System.Collections.Generic;
using Models;
using Mvc.Infrastructure.Concrete;
using Mvc.ViewModels;
using NUnit.Framework;
using System.Linq;
using PagedList;

namespace Tests
{
    [TestFixture]
    public class MapperTests
    {
        private CommonMapper mapper;

        [SetUp]
        public void SetUp()
        {
            mapper = new CommonMapper();
        }

        [Test]
        public void RecordsToRecordsViewModelTest()
        {
            var records = new List<Record>
                {
                    new Record {Id = 1, Text = "Hello", CreationDate = DateTime.MinValue, User = new User {Id = 1, Email = "a"}},
                    new Record {Id = 2, Text = "World", CreationDate = DateTime.MaxValue, User = new User {Id = 21, Email = "b"}},
                    new Record {Id = 3, Text = "!", CreationDate = DateTime.MaxValue, User = new User {Id = 2, Email = "c"}},
                    new Record {Id = 4, Text = "hoho!", CreationDate = DateTime.Today, User = new User {Id = 4, Email = "d"}},
                };

            var recordsViewModel =
                mapper.Map<Tuple<int, int, int, int, IEnumerable<Record>>, RecordsViewModel>(
                    new Tuple<int, int, int, int, IEnumerable<Record>>(42, 123, 2, 2, records));

            Assert.That(recordsViewModel.SectionId, Is.EqualTo(42));
            Assert.That(recordsViewModel.TopicId, Is.EqualTo(123));
            Assert.That(recordsViewModel.Records.Count(), Is.EqualTo(2));

            for (var i = 2; i < 4; i++)
            {
                Assert.That(recordsViewModel.Records[i - 2].Id, Is.EqualTo(records[i].Id));
                Assert.That(recordsViewModel.Records[i - 2].Text, Is.EqualTo(records[i].Text));
                Assert.That(recordsViewModel.Records[i - 2].CreationDate, Is.EqualTo(records[i].CreationDate));
                Assert.That(recordsViewModel.Records[i - 2].UserId, Is.EqualTo(records[i].User.Id));
                Assert.That(recordsViewModel.Records[i - 2].UserEmail, Is.EqualTo(records[i].User.Email));
            }
        }
    }
}