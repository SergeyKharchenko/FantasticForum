using System;
using System.Collections.Generic;
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
        private readonly IFileAssistant fileAssistant;

        public AccountController(AbstractUserUnitOfWork userUnitOfWork,
                                 IAuthorizationAssistant authorizationAssistant,
                                 IFileAssistant fileAssistant,
                                 IMapper mapper)
            : base(userUnitOfWork)
        {
            this.userUnitOfWork = userUnitOfWork;
            this.mapper = mapper;
            this.authorizationAssistant = authorizationAssistant;
            this.fileAssistant = fileAssistant;
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
        public ActionResult Register(RegisterViewModel registeredUser, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View(registeredUser);
            var user = (User) mapper.Map(registeredUser, typeof (RegisterViewModel), typeof (User));
            if (userUnitOfWork.IsUserExist(user))
            {
                ModelState.AddModelError("Email", "User with that email already exist");
                return View(registeredUser);
            }
            var createdUser = userUnitOfWork.RegisterUser(user, avatar);
            authorizationAssistant.WriteAuthInfoInSession(Session, createdUser);
            return RedirectToAction("List", "Section");
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
            authorizationAssistant.WriteAuthInfoInSession(Session, user);
            return Redirect(TempData["returnUrl"] as string);
        }

        //
        // POST: /Section/GetAvatar

        public FileContentResult GetAvatar(int id)
        {
            var imageUtilityModel = userUnitOfWork.GetAvatar(id);
            if (imageUtilityModel.HasImage)
                return File(imageUtilityModel.Data, imageUtilityModel.ImageMimeType);

            var imageData = fileAssistant.FileToByteArray(Server.MapPath("~/Images/Section/section-without-avatar.png"));
            return File(imageData, "image/png");
        }
        
        //
        // Get: /Account/Logout

        public RedirectToRouteResult Logout()
        {
            authorizationAssistant.RemoveAuthInfoFromSession(Session);
            return RedirectToAction("List", "Section");
        }
    }
}
