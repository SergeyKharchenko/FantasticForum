using System;
using System.Collections.Generic;
using System.Linq;
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

    }
}
