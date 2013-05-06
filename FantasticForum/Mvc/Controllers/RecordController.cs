using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Filters;
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
        [HttpPost, ForumAuthorize]
        public RedirectToRouteResult Add(int sectionId, int topicId, string text)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("text", "Record cannot be empty");
            }
            else
            {
                var record = new Record
                {
                    Text = text,
                    CreationDate = DateTime.Now,
                    TopicId = topicId,
                    UserId = ((UserIndentity) User).User.Id
                };
                unitOfWork.Create(record);
            }

            return RedirectToAction("List", new {sectionId, topicId});
        }

    }
}
