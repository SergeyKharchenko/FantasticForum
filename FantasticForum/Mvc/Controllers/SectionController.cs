using System;
using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Assistants.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.ViewModels;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Controllers
{
    public class SectionController : BaseController<Section>
    {
        private readonly AbstractSectionUnitOfWork sectionUnitOfWork;
        private readonly IFileAssistant fileAssistant;
        private readonly IMapper mapper;

        public SectionController(AbstractSectionUnitOfWork sectionUnitOfWork, IFileAssistant fileAssistant, IMapper mapper)
            : base(sectionUnitOfWork)
        {
            this.sectionUnitOfWork = sectionUnitOfWork;
            this.fileAssistant = fileAssistant;
            this.mapper = mapper;
        }
        

        //
        // GET: /Section/List

        public ViewResult List()
        {
            var sections = sectionUnitOfWork.Entities
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
            var section = sectionUnitOfWork.Read(id);
            return View(section);
        }      

        //
        // POST: /Section/Edit
        [HttpPost]
        public ActionResult Edit(Section section, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View("Edit", section);

            var crudTesult = sectionUnitOfWork.CreateOrUpdateSection(section, avatar);

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
            var imageUtilityModel = sectionUnitOfWork.GetAvatar(id);
            if (imageUtilityModel.HasImage)
                return File(imageUtilityModel.Data, imageUtilityModel.ImageMimeType);

            var imageData = fileAssistant.FileToByteArray(Server.MapPath("~/Images/Section/section-without-avatar.png"));
            return File(imageData, "image/png");
        }

        //
        // Get: /Section/Remove
        
        public ViewResult Remove(int id, bool? concurrencyError)
        {            
            var section = sectionUnitOfWork.Read(id);
            if (concurrencyError != null && concurrencyError.Value)
                ViewBag.ConcurrencyError = "This section had been changed by another user";
            return View(section);
        }

        //
        // POST: /Section/Remove
        [HttpPost][ValidateAntiForgeryToken]
        public RedirectToRouteResult Remove(Section section)
        {
            var crudTesult = sectionUnitOfWork.Delete(section);
            if (!crudTesult.Success)
                return RedirectToAction("Remove", new {id = section.Id, concurrencyError = true});

            return RedirectToAction("List");
        }


        public ViewResult Test()
        {
            return View();
        }
    }
}
