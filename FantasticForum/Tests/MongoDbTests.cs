using System;
using System.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class MongoDbTests
    {
        [Test]
        public void ConnectTest()
        {
            var connectionString = GetMongoDbConnectionString();

            Console.WriteLine(connectionString);

            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("test");
            var collection = database.GetCollection<EntityDummy>("entities");

            var entity = new EntityDummy() { Name = "Tom" };
            collection.Insert(entity);
        }

        private string GetMongoDbConnectionString()
        {
            return ConfigurationManager.AppSettings.Get("MONGOLAB_URI") ??
                   "mongodb://localhost/Things";
        }

        public class EntityDummy
        {
            public ObjectId Id { get; set; }

            public string Name { get; set; }
        }
    }
}