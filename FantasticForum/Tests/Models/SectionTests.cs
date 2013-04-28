using Models;
using NUnit.Framework;
using System.Collections.ObjectModel;

namespace Tests.Models
{
    [TestFixture]
    public class SectionTests
    {
        [Test]
        public void EqualTest()
        {
            var section1 = new Section
            {
                Title = "a",
                Topics = new Collection<Topic>
                {
                    new Topic {Title = "b", SectionId = 1}
                }
            };
            var section2 = new Section
            {
                Title = "a",
                Topics = new Collection<Topic>
                {
                    new Topic {Title = "b", SectionId = 1}
                }
            };

            Assert.That(section1, Is.EqualTo(section2));
        }
    }
}