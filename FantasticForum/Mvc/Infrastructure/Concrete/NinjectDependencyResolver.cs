using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using Models;
using MongoDB.Driver;
using Mvc.Infrastructure.Abstract;
using Ninject;

namespace Mvc.Infrastructure.Concrete
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependencyResolver()
        {
            kernel = new StandardKernel();

            AddBindigs();
        }

        private void AddBindigs()
        {
            /*
            kernel.Bind(typeof(AbstractSectionUnitOfWork)).To(typeof(SectionUnitOfWork)).InRequestScope();
            kernel.Bind(typeof(ISqlCrudUnitOfWork<>)).To(typeof(SqlCrudUnitOfWork<>)).InRequestScope();

            var client = new MongoClient(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDB"));
            kernel.Bind(typeof(IRepository<>)).To(typeof(MongoRepository<>))
                .When(request => request.Target.Name.Contains("Mongo"))
                .InRequestScope()
                .WithConstructorArgument("database", database);

            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>)).InRequestScope();

            kernel.Bind(typeof(DbContext)).To(typeof(ForumContext)).InRequestScope();
            kernel.Bind(typeof(IFileHelper)).To(typeof(FileHelper)).InSingletonScope();
            kernel.Bind(typeof(IMapper)).To(typeof(CommonMapper)).InSingletonScope();
             */
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}