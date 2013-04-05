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
            return View();
        }      

        //
        // POST: /Section/Create
        [HttpPost]
        public ActionResult Create(Section section, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View();
            unitOfWork.CreateSection(section, avatar);
            return RedirectToAction("List");
        }

        //
        // POST: /Section/GetAvatar
        [HttpPost]
        public FileContentResult GetAvatar(int sectionId)
        {
            var getAvatarSM = unitOfWork.GetAvatar(sectionId);
            if (getAvatarSM.HasAvatar)
                return File(getAvatarSM.AvatarData, getAvatarSM.ImageMimeType);

            var imageData = fileHelper.FileToByteArray(Server.MapPath("~/Images/Section/section-without-avatar.png"));
            return File(imageData, "image/png");
        }

        //
        // Get: /Section/Remove
        
        public ViewResult Remove(int sectionId)
        {
            var section = unitOfWork.GetSectionById(sectionId);
            return View(section);
        }

        //
        // POST: /Section/Remove
        [HttpPost]
        public RedirectToRouteResult Remove(Section section)
        {
            unitOfWork.RemoveSection(section.Id);
            return RedirectToAction("List");
        }
    }
}
