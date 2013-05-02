using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Abstract;
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

        public ActionResult Register(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
            return View();
        }

        //
        // Post: /Account/Register
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel registeredUser, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View(registeredUser);
            var user = (User) mapper.Map(registeredUser, typeof (RegisterViewModel), typeof (User));
            var createdUser = userUnitOfWork.RegisterUser(user, avatar);
            authorizationAssistant.WriteAuthInfoInSession(Session, createdUser.Id);
            return Redirect(TempData["returnUrl"] as string);
        }

        //
        // Get: /Account/Login

        public ActionResult Login(string returnUrl)
        {
            TempData["returnUrl"] = returnUrl;
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
            return Redirect(TempData["returnUrl"] as string);
        }
    }
}
