using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Mvc;
using Models;
using MongoDB.Driver;
using Mvc.Infrastructure.Abstract;
using Ninject;
using Ninject.Web.Common;

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
            #region Database

            kernel.Bind(typeof (DbContext)).To(typeof (ForumContext)).InRequestScope();

            var client = new MongoClient(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDB"));
            kernel.Bind(typeof (IRepository<>)).To(typeof (MongoRepository<>))
                  .When(request => request.Target.Name.Contains("Mongo"))
                  .InRequestScope()
                  .WithConstructorArgument("database", database);

            kernel.Bind(typeof (IRepository<>)).To(typeof (SqlRepository<>)).InRequestScope();

            #endregion

            #region Unit of work

            kernel.Bind(typeof (AbstractSectionUnitOfWork)).To(typeof (SectionUnitOfWork)).InRequestScope();
            kernel.Bind(typeof (AbstractTopicUnitOfWork)).To(typeof (TopicUnitOfWork)).InRequestScope();
            kernel.Bind(typeof(AbstractUserUnitOfWork)).To(typeof(UserUnitOfWork)).InRequestScope();
            kernel.Bind(typeof (ISqlCrudUnitOfWork<>)).To(typeof (SqlCrudUnitOfWork<>)).InRequestScope();

            #endregion

            #region Assistants

            kernel.Bind(typeof(IEntityWithImageAssistant<>)).To(typeof(EntityWithImageAssistant<>)).InRequestScope();
            kernel.Bind(typeof (IFileAssistant)).To(typeof (FileAssistant)).InSingletonScope();
            kernel.Bind(typeof (IMapper)).To(typeof (CommonMapper)).InSingletonScope();

            #endregion
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