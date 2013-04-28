using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public class AccountController : BaseController<User>
    {
        private readonly AbstractUserUnitOfWork userUnitOfWork;
        private readonly IMapper mapper;
        private readonly IAuthorizationAssistant authorizationAssistant;

        public AccountController(AbstractUserUnitOfWork userUnitOfWork,
                                 IAuthorizationAssistant authorizationAssistant,
                                 IMapper mapper)
            : base(userUnitOfWork)
        {
            this.userUnitOfWork = userUnitOfWork;
            this.mapper = mapper;
            this.authorizationAssistant = authorizationAssistant;
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
            var createdUser = userUnitOfWork.RegisterUser(user, avatar);
            authorizationAssistant.WriteAuthInfoInCookie(Response, createdUser.Id);
            return RedirectToAction("Register");
        }
    }
}
