using System;
using System.Collections.Generic;
using System.Web.Routing;
using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.Mailers;
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
        private readonly IUserMailer userMailer;
        private readonly IUrlAssistant urlAssistant;

        public AccountController(AbstractUserUnitOfWork userUnitOfWork,
                                 IAuthorizationAssistant authorizationAssistant,
                                 IFileAssistant fileAssistant,
                                 IUserMailer userMailer,
                                 IUrlAssistant urlAssistant,
                                 IMapper mapper)
            : base(userUnitOfWork)
        {
            this.userUnitOfWork = userUnitOfWork;
            this.mapper = mapper;
            this.authorizationAssistant = authorizationAssistant;
            this.fileAssistant = fileAssistant;
            this.userMailer = userMailer;
            this.urlAssistant = urlAssistant;
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
        public ViewResult Register(RegisterViewModel registeredUser, HttpPostedFileBase avatar)
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
            var url = urlAssistant.GenerateAbsoluteUrl("RegistrationConfirmation", "Account", new { guid  = createdUser.Guid}, Url);
            var message = userMailer.Register(createdUser.Email, url);
            message.Send();

            return View("RegistrationNotification");
        }

        //
        // Get: /Account/RegistrationConfirmation

        public ViewResult RegistrationConfirmation(string guid)
        {
            var user = userUnitOfWork.Read(u => u.Guid == guid && !u.IsConfirmed).FirstOrDefault();
            if (user == null)
                return View(false);
            user.IsConfirmed = true;
            userUnitOfWork.Update(user);
            authorizationAssistant.WriteAuthInfoInSession(Session, user);
            return View(true);
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
