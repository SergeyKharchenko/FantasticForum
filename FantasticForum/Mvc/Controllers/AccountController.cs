using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using System.Web;
using System.Web.Mvc;
using Mvc.ViewModels;
using System.Linq;

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
            authorizationAssistant.WriteAuthInfoInSession(Session, createdUser.Id);
            return RedirectToAction("List", "Section");
        }

        //
        // Get: /Account/Login

        public ActionResult Login()
        {
            return View();
        }

        //
        // Get: /Account/Login
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return View(loginViewModel);
            var user = unitOfWork.Read(u => u.Email == loginViewModel.Email && u.Password == loginViewModel.Password)
                .FirstOrDefault();
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid login or password");
                return View(loginViewModel);
            }
            authorizationAssistant.WriteAuthInfoInSession(Session, user.Id);
            return RedirectToAction("List", "Section");
        }
    }
}
