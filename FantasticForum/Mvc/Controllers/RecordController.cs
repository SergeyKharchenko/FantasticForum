using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.UnitsOfWork.Concrete;

namespace Mvc.Controllers
{
    public class RecordController : BaseController<Record>
    {
        public RecordController(SqlCrudUnitOfWork<Record> unitOfWork)
            : base(unitOfWork)
        {
        }

        //
        // GET: /Record/

        public ActionResult List(int sectionId, int topicId)
        {
            var records = unitOfWork.Read(record => record.TopicId == topicId);
            return View(records);
        }

    }
}
