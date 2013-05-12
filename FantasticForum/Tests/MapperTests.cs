using System;
using System.Collections.Generic;
using Models;
using Mvc.Infrastructure.Concrete;
using Mvc.ViewModels;
using NUnit.Framework;
using System.Linq;

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
                    new Record {Id = 1, Text = "Hello", CreationDate = DateTime.MinValue, User = new User {Email = "a"}},
                    new Record {Id = 2, Text = "World", CreationDate = DateTime.MaxValue, User = new User {Email = "b"}}
                };

            var recordsViewModel =
                mapper.Map<Tuple<int, int, IEnumerable<Record>>, RecordsViewModel>(
                    new Tuple<int, int, IEnumerable<Record>>(42, 123, records));

            Assert.That(recordsViewModel.SectionId, Is.EqualTo(42));
            Assert.That(recordsViewModel.TopicId, Is.EqualTo(123));
            Assert.That(recordsViewModel.Records.Count(), Is.EqualTo(2));

            for (var i = 0; i < 2; i++)
            {
                Assert.That(recordsViewModel.Records[i].Id, Is.EqualTo(records[i].Id));
                Assert.That(recordsViewModel.Records[i].Text, Is.EqualTo(records[i].Text));
                Assert.That(recordsViewModel.Records[i].CreationDate, Is.EqualTo(records[i].CreationDate));
                Assert.That(recordsViewModel.Records[i].UserEmail, Is.EqualTo(records[i].User.Email));
            }
        }
    }
}