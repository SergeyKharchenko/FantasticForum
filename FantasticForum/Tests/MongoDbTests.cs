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

            var entity = new EntityDummy { Name = "Tom" };
            collection.Insert(entity);
        }

        [Test]
        public void AppVarTest()
        {
            Console.WriteLine(ConfigurationManager.AppSettings.Get("Environment"));
            Console.WriteLine(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));

            foreach (var key in ConfigurationManager.AppSettings.AllKeys)
            {
                Console.WriteLine(key);
            }
        }


        private string GetMongoDbConnectionString()
        {
            Console.WriteLine(@"ConfigurationManager" + ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            Console.WriteLine(@"Environment" + Environment.GetEnvironmentVariable("MONGOLAB_URI"));

            return Environment.GetEnvironmentVariable("MONGOLAB_URI") ??
                   "mongodb://localhost/Things";
        }

        public class EntityDummy
        {
            public ObjectId Id { get; set; }

            public string Name { get; set; }
        }
    }
}