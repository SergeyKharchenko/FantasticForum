using System.Configuration;
using System.Data.Entity;
using Models;
using MongoDB.Driver;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.Assistants.Concrete;
using Mvc.Infrastructure.Concrete;
using Mvc.Infrastructure.DAL.Abstract;
using Mvc.Infrastructure.DAL.Cocnrete;
using Mvc.Infrastructure.Mailers;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Mvc.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Mvc.App_Start.NinjectWebCommon), "Stop")]

namespace Mvc.App_Start
{
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using System;
    using System.Web;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            #region Database

            kernel.Bind(typeof(DbContext)).To(typeof(ForumContext)).InRequestScope();

            var client = new MongoClient(ConfigurationManager.AppSettings.Get("MONGOLAB_URI"));
            var server = client.GetServer();
            var database = server.GetDatabase(ConfigurationManager.AppSettings.Get("MongoDB"));
            kernel.Bind(typeof(IRepository<Image>)).To(typeof(MongoRepository<Image>))
                  .InRequestScope()
                  .WithConstructorArgument("database", database);

            kernel.Bind(typeof(IRepository<>)).To(typeof(SqlRepository<>)).InRequestScope();

            #endregion

            #region Unit of work

            kernel.Bind(typeof(AbstractSectionUnitOfWork)).To(typeof(SectionUnitOfWork)).InRequestScope();
            kernel.Bind(typeof(AbstractTopicUnitOfWork)).To(typeof(TopicUnitOfWork)).InRequestScope();
            kernel.Bind(typeof(AbstractUserUnitOfWork)).To(typeof(UserUnitOfWork)).InRequestScope();
            kernel.Bind(typeof(ISqlCrudUnitOfWork<>)).To(typeof(SqlCrudUnitOfWork<>)).InRequestScope();

            #endregion

            #region Assistants

            kernel.Bind(typeof(IEntityWithImageAssistant<>)).To(typeof(EntityWithImageAssistant<>)).InRequestScope();
            kernel.Bind(typeof(IFileAssistant)).To(typeof(FileAssistant)).InSingletonScope();
            kernel.Bind(typeof(IAuthorizationAssistant)).To(typeof(AuthorizationAssistant)).InSingletonScope();
            kernel.Bind(typeof(IUrlAssistant)).To(typeof(UrlAssistant)).InSingletonScope();

            #endregion

            kernel.Bind(typeof(IMapper)).To(typeof(CommonMapper)).InSingletonScope();
            kernel.Bind(typeof(ILogger)).To(typeof(MyLogger)).InSingletonScope();
            kernel.Bind(typeof(IUserMailer)).To(typeof(UserMailer));
        }        
    }
}
