using System.Collections.Generic;
using Models;
using Mvc.ViewModels;
using NUnit.Framework;

namespace Tests.ViewModel.Section
{
    [TestFixture]
    public class SectionListVMTests
    {
        [Test]
        public void ConvertFromSectionToSectionListTest()
        {
            var sections = new List<Models.Section>
                {
                    new Models.Section {ImageId = "1", Title = "Sport"},
                    new Models.Section {ImageId = "12", Title = "Life"},
                    new Models.Section {ImageId = "13", Title = "Programming"}
                };

            var sectionListVMs = sections.ToVMList();

            for (var i = 0; i < sections.Count; i++)
            {
                Assert.That(sections[i].Id, Is.EqualTo(sectionListVMs[i].Id));
                Assert.That(sections[i].Title, Is.EqualTo(sectionListVMs[i].Title));
                Assert.That(sections[i].Title, Is.EqualTo(sectionListVMs[i].Title));
            }            
        }

    }
}