using System;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using Models;
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
            kernel.Bind(typeof(IRepository<>)).To(typeof(Repository<>));  
            kernel.Bind(typeof(DbContext)).To(typeof(ForumContext)).InSingletonScope();  
        }

    }
}