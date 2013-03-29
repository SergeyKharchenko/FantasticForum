using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Controllers
{
    public class TopicController : Controller
    {
        private readonly IEntityUnitOfWork unitOfWork;

        public TopicController(IEntityUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        

        //
        // GET: /Topic/List

        public ViewResult List()
        {
            return View(unitOfWork.Topics);
        }

    }
}
