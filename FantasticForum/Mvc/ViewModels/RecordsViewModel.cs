using System;
using System.Collections.Generic;
using PagedList;

namespace Mvc.ViewModels
{
    public class RecordsViewModel
    {
        public int SectionId { get; set; }    
        public int TopicId { get; set; }
        public IPagedList<RecordViewModel> Records { get; set; }
    }

    public class RecordViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
    }
}   