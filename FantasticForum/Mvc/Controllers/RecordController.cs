using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;

namespace Mvc.Controllers
{
    public class RecordController : BaseController<Record>
    {
        public RecordController(ISqlCrudUnitOfWork<Record> unitOfWork)
            : base(unitOfWork)
        {
        }

        //
        // GET: /Record/

        public ViewResult List(int sectionId, int topicId)
        {
            var records = unitOfWork.Read(record => record.TopicId == topicId);
            ViewBag.SectionId = sectionId;
            ViewBag.TopicId = topicId;
            return View(records);
        }

        //
        // POST: /Record/Add

        public ActionResult Add(int sectionId, int topicId, string text)
        {
            var records = unitOfWork.Read(record => record.TopicId == topicId);
            return View(records);
        }


    }
}
