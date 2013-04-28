using System.Linq;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure;
using Mvc.Infrastructure.UnitsOfWork.Abstract;
using Mvc.ViewModels;

namespace Mvc.Controllers
{
    public class TopicController : BaseController<Topic>
    {
        private readonly AbstractTopicUnitOfWork topicUnitOfWork;
        private readonly IMapper mapper;

        public TopicController(AbstractTopicUnitOfWork topicUnitOfWork, IMapper mapper)
            : base(topicUnitOfWork)
        {
            this.topicUnitOfWork = topicUnitOfWork;
            this.mapper = mapper;
        }

        //
        // GET: /Topic/List

        public ViewResult List(int sectionId)
        {
            var topics = topicUnitOfWork.Read(topic => topic.SectionId == sectionId);
            ViewBag.SectionId = sectionId;
            return View(topics
                            .Select(topic => mapper.Map(topic, typeof (Topic), typeof (TopicViewModel)))
                            .Cast<TopicViewModel>().AsEnumerable());
        }

        //
        // GET: /Topic/Create

        public ViewResult Create(int sectionId)
        {
            return View(new Topic { SectionId = sectionId });
        } 

        //
        // Post: /Topic/Create
        [HttpPost]
        public ActionResult Create(Topic topic)
        {
            if (!ModelState.IsValid)
                return View("Create", topic);
            topicUnitOfWork.Create(topic);
            return RedirectToAction("List", new {sectionId = topic.SectionId});
        }

        //
        // GET: /Topic/Remove

        public ViewResult Remove(int id, bool? concurrencyError)
        {
            var topic = topicUnitOfWork.Read(id);
            ViewBag.SectionId = topic.SectionId;
            return View(mapper.Map(topic, typeof(Topic), typeof(TopicViewModel)));
        } 

        //
        // GET: /Topic/Remove
        [HttpPost][ValidateAntiForgeryToken]
        public RedirectToRouteResult Remove(Topic topic)
        {
            topicUnitOfWork.Delete(topic);
            return RedirectToAction("List", new { sectionId = topic.SectionId });
        } 
    }
}
