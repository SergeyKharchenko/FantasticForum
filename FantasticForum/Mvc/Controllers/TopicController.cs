using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public class TopicController : Controller
    {
        //
        // GET: /Topic/List

        public ActionResult List(int id)
        {
            return View(id);
        }

    }
}
