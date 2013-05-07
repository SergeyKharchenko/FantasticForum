using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Mailers;

namespace Mvc.Controllers
{
    public class MailController : Controller
    {
        public IUserMailer Mailer { get; set; }

        public MailController()
        {
            Mailer = new UserMailer();
        }

        public ActionResult Index()
        {
            Mailer.Register().Send();
            return View();
        }
    }
}
