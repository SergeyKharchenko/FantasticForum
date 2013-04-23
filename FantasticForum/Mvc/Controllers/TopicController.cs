using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Mvc.Infrastructure.Abstract;
using Mvc.Infrastructure.Concrete;
using Mvc.ViewModels;

namespace Mvc.Controllers
{
    public class TopicController : Controller
    {
        private readonly ISqlCrudUnitOfWork<Topic> unitOfWork;
        private readonly IMapper mapper;

        public TopicController(ISqlCrudUnitOfWork<Topic> unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        //
        // GET: /Topic/List

        public ViewResult List(int id)
        {
            var topics = unitOfWork.Read(topic => topic.SectionId == id);
            return View(topics
                            .Select(topic => mapper.Map(topic, typeof (Topic), typeof (TopicViewModel)))
                            .Cast<TopicViewModel>().AsEnumerable());
        }
    }
}
