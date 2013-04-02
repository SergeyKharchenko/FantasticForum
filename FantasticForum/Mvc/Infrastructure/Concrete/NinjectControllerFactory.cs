﻿using System;
using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Models;
using MongoDB.Driver;
using Mvc.Infrastructure.Abstract;
using Ninject;

namespace Mvc.Infrastructure.Concrete
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private readonly IKernel kernel;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null
                       ? null
                       : (IController) kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            kernel.Bind(typeof(ISectionUnitOfWork)).To(typeof(SectionUnitOfWork));
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InThreadScope();

            var client = new MongoClient(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDB"));
            kernel.Bind(typeof(IMongoRepository<>)).To(typeof(MongoRepository<>))
                .InThreadScope()
                .WithConstructorArgument("database", database);  

            kernel.Bind(typeof(DbContext)).To(typeof(ForumContext)).InThreadScope();
            kernel.Bind(typeof(IFileHelper)).To(typeof(FileHelper));  
        }

    }
}