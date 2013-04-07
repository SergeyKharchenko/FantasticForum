using System;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.ViewModels;

namespace Mvc.Controllers
{
    public class SectionController : Controller
    {
        private readonly ISectionUnitOfWork unitOfWork;
        private readonly IFileHelper fileHelper;

        public SectionController(ISectionUnitOfWork unitOfWork, IFileHelper fileHelper)
        {
            this.unitOfWork = unitOfWork;
            this.fileHelper = fileHelper;
        }
        

        //
        // GET: /Section/List

        public ViewResult List()
        {
            return View(unitOfWork.Section.ToVMList());
        }        

        //
        // GET: /Section/Create

        public ViewResult Create()
        {
            return View("Edit", new Section());
        }       

        //
        // GET: /Section/Edit

        public ViewResult Edit(int id)
        {
            var section = unitOfWork.GetSectionById(id);
            return View(section);
        }      

        //
        // POST: /Section/Edit
        [HttpPost]
        public ActionResult Edit(Section section, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View("Edit", section);
            try
            {
                unitOfWork.CreateOrUpdateSection(section, avatar);
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "This section had been changed by another user");
                return View("Edit", section);
            }
            return RedirectToAction("List");
        }

        //
        // POST: /Section/GetAvatar

        public FileContentResult GetAvatar(int id)
        {
            var getAvatarSM = unitOfWork.GetAvatar(id);
            if (getAvatarSM.HasAvatar)
                return File(getAvatarSM.AvatarData, getAvatarSM.ImageMimeType);

            var imageData = fileHelper.FileToByteArray(Server.MapPath("~/Images/Section/section-without-avatar.png"));
            return File(imageData, "image/png");
        }

        //
        // Get: /Section/Remove
        
        public ViewResult Remove(int id, bool? concurrencyError)
        {            
            var section = unitOfWork.GetSectionById(id);
            if (concurrencyError != null && concurrencyError.Value)
                ViewBag.ConcurrencyError = "This section had been changed by another user";
            return View(section);
        }

        //
        // POST: /Section/Remove
        [HttpPost][ValidateAntiForgeryToken]
        public RedirectToRouteResult Remove(Section section)
        {
            try
            {
                unitOfWork.RemoveSection(section);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Remove", new { id = section.Id, concurrencyError = true });
            }
            return RedirectToAction("List");
        }
    }
}
