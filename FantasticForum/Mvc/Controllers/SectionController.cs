using System;
using System.Data.Entity.Infrastructure;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.ViewModels;
using System.Linq;

namespace Mvc.Controllers
{
    public class SectionController : Controller
    {
        private readonly AbstractSectionUnitOfWork unitOfWork;
        private readonly IFileHelper fileHelper;
        private readonly IMapper mapper;

        public SectionController(AbstractSectionUnitOfWork unitOfWork, IFileHelper fileHelper, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.fileHelper = fileHelper;
            this.mapper = mapper;
        }
        

        //
        // GET: /Section/List

        public ViewResult List()
        {
            var sections = unitOfWork.Entities
                                     .Select(section => mapper.Map(section, typeof (Section), typeof (SectionViewModel)))
                                     .Cast<SectionViewModel>().AsEnumerable();            
            return View(sections);
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
            var section = unitOfWork.Read(id);
            return View(section);
        }      

        //
        // POST: /Section/Edit
        [HttpPost]
        public ActionResult Edit(Section section, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View("Edit", section);

            var crudTesult = unitOfWork.CreateOrUpdateSection(section, avatar);

            if (!crudTesult.Success)
            {
                ModelState.AddModelError("", "This section had been changed by another user");
                return View("Edit", crudTesult.Entity);
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
            var section = unitOfWork.Read(id);
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
                unitOfWork.Delete(section);
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Remove", new { id = section.Id, concurrencyError = true });
            }
            return RedirectToAction("List");
        }
    }
}
