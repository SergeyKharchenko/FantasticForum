using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;

namespace Mvc.Controllers
{
    public class SectionController : Controller
    {
        private readonly IEntityUnitOfWork unitOfWork;

        public SectionController(IEntityUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        

        //
        // GET: /Section/List

        public ViewResult List()
        {
            return View(unitOfWork.Sections);
        }

    }
}
