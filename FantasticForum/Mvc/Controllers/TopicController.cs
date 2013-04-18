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
        private readonly ISqlCrudUnitOfWork<Topic> unitOfWork;

        public TopicController(ISqlCrudUnitOfWork<Topic> unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        //
        // GET: /Topic/List

        public ActionResult List(int id)
        {
            return View(id);
        }

    }
}
