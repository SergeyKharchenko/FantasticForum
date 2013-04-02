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
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("appharbor_c717f8e9-4daf-4dfe-89c9-76933e1b68cd");
            var collection = database.GetCollection<EntityDummy>("entities");

            var entity = new EntityDummy { Data = new byte[] {1, 2, 3} };
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

            public byte[] Data { get; set; }
        }
    }
}