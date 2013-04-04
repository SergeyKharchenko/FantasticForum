﻿using System.Collections.Generic;
using System.Configuration;
using Models;
using MongoDB.Driver;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using NUnit.Framework;
using System.Linq;

namespace Tests.Repository
{
    [TestFixture]
    public class MongoRepositoryTests
    {
        private IMongoRepository<Image> repository;
        private List<Image> images;
            
        [TestFixtureSetUp]
        public void SetUp()
        {
            var client = new MongoClient(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDB"));
            repository = new MongoRepository<Image>(database, "testCollection");

            images = new List<Image>
                {
                    new Image {Data = new byte[] {11, 22, 33}, ImageMimeType = "png"},
                    new Image {Data = new byte[] {111, 222, 3}, ImageMimeType = "jpg"},
                    new Image {Data = new byte[] {222, 123, 1}, ImageMimeType = "gif"},
                };
            images.ForEach(i => repository.Create(i));
        }

        [Test]
        public void GetAllEntitiesTest()
        {
            var entities = repository.Entities.ToList();

            for (var i = 0; i < entities.Count(); i++)
            {
                Assert.That(entities[i].Id, Is.EqualTo(images[i].Id));
                Assert.That(entities[i].Data, Is.EquivalentTo(images[i].Data));
                Assert.That(entities[i].ImageMimeType, Is.EqualTo(images[i].ImageMimeType));
            }
        }

        [Test]
        public void InsertEntityTest()
        {
            var image = new Image {Data = new byte[] {1, 2, 3}, ImageMimeType = "Hello"};
            repository.Create(image);
            var id = image.Id.ToString();

            var newImage = GetEntityById(id);
            Assert.That(newImage.Data, Is.EquivalentTo(image.Data));
            Assert.That(newImage.ImageMimeType, Is.EqualTo(image.ImageMimeType));
        }

        [Test]
        public void GetEntityByIdTest()
        {
            var id = repository.Entities.First().Id.ToString();
            var image = GetEntityById(id);

            Assert.That(image.Id.ToString(), Is.EqualTo(id));
            Assert.That(image.Data, Is.EquivalentTo(new byte[] { 11, 22, 33 }));
            Assert.That(image.ImageMimeType, Is.EqualTo("png"));
        }

        [Test]
        public void RemoveTest()
        {
            var id = repository.Entities.Last().Id.ToString();
            var image = GetEntityById(id);
            Assert.That(image, Is.Not.Null);

            repository.Remove(id);

            image = GetEntityById(id);
            Assert.That(image, Is.Null);
        }

        public Image GetEntityById(string id)
        {
            var entity = repository.Get(id);
            return entity;
        }

        [TestFixtureTearDown]
        public void CleanUp()
        {
            repository.DropCollection();
        }
    }
}