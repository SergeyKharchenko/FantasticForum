using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Controllers
{
    public class AccountController : BaseController<User>
    {
        private readonly AbstractUserUnitOfWork userUnitOfWork;
        private readonly IMapper mapper;

        public AccountController(AbstractUserUnitOfWork userUnitOfWork, IMapper mapper)
            : base(userUnitOfWork)
        {
            this.userUnitOfWork = userUnitOfWork;
            this.mapper = mapper;
        }


        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // Post: /Account/Register
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(User user, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View(user);
            var authTicket = userUnitOfWork.RegisterUser(user, avatar);
            var authCookie = new HttpCookie(ConfigurationManager.AppSettings.Get("Auth")) { Value = authTicket};
            Response.Cookies.Set(authCookie);
            return RedirectToAction("Register");
        }

    }
}
