using System.Configuration;
using Models;
using MongoDB.Driver;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;

namespace Tests.Repository
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        private IMongoRepository<Image> repository;
        private Image image;
            
        [TestFixtureSetUp]
        public void SetUp()
        {
            var client = new MongoClient(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDB"));
            repository = new MongoRepository<Image>(database);
        }

        [Test]
        public void InsertEntityTest()
        {
            image = new Image {Data = new byte[] {1, 2, 3}, ImageMimeType = "Hello"};
            repository.Create(image);
            var id = image.Id.ToString();

            var newImage = GetEntityById(id);
            Assert.That(newImage.Data, Is.EquivalentTo(image.Data));
            Assert.That(newImage.ImageMimeType, Is.EqualTo(image.ImageMimeType));
        }

        public Image GetEntityById(string id)
        {
            var entity = repository.Get(id);
            return entity;
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            repository.Remove(image.Id.ToString());
        }
    }
}