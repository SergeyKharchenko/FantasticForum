using System.Collections.Generic;
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
                    new Models.Section {Title = "Sport"},
                    new Models.Section {Title = "Life"},
                    new Models.Section {Title = "Programming"}
                };

            var sectionListVMs = sections.ToVMList();

            for (var i = 0; i < sections.Count; i++)
            {
                Assert.That(sections[i].Id == sectionListVMs[i].Id);
                Assert.That(sections[i].Title == sectionListVMs[i].Title);
            }            
        }

    }
}