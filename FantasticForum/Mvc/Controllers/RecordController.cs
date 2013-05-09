using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Filters;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.Infrastructure.UnitsOfWork.Concrete;
using PagedList;

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

        public ViewResult List(int sectionId, int topicId, int? page)
        {
            var records = unitOfWork.Read(record => record.TopicId == topicId);
            var pageNumber = page ?? 1;
            var pagedRecords = records.OrderBy(r => r.CreationDate).ToPagedList(pageNumber, 10);
            ViewBag.SectionId = sectionId;
            ViewBag.TopicId = topicId;
            return View(pagedRecords);
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

        //
        // GET: /Record/Edit
        [Authorize]
        public ViewResult Edit(int sectionId, int topicId, int id)
        {
            var userId = (User.Identity as UserIndentity).User.Id;
            var record = unitOfWork.Read(r => r.Id == id && r.UserId == userId)
                                   .FirstOrDefault();
            if (record == null)
                throw new ArgumentException("Such record was not found or you are not the creator of it");
            return View(record);
        }

        //
        // Post: /Record/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(int sectionId, int topicId, Record record)
        {
            if (!ModelState.IsValid)
                return View(record);
            unitOfWork.Update(record);
            return RedirectToAction("List");
        }

        //
        // GET: /Delete/Edit
        [Authorize]
        public ViewResult Delete(int sectionId, int topicId, int id)
        {
            var record = unitOfWork.Read(id);
            if (record == null)
                throw new ArgumentException("Such record was not found or you are not the creator of it");
            return View(record);
        }

        //
        // Post: /Delete/Edit
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(int sectionId, int topicId, Record record)
        {
            unitOfWork.Delete(record);
            return RedirectToAction("List");
        }
    }
}
