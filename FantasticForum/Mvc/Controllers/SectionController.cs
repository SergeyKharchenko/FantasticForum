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

        public SectionController(ISectionUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
        // GET: /Section/Create
        [HttpPost]
        public ActionResult Create(Section section, HttpPostedFileBase avatar)
        {
            if (!ModelState.IsValid)
                return View();
            const string virtualPath = "~/Images/Section";
            var path = Server.MapPath(virtualPath);
            unitOfWork.Create(section, avatar, path, virtualPath.Substring(1));
            return RedirectToAction("List");
        }

    }
}
