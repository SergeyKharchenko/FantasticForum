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
                    new Models.Section {Title = "Sport", Image = new Image()},
                    new Models.Section {Title = "Life", Image = new Image()},
                    new Models.Section {Title = "Programming", Image = new Image()}
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