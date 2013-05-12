using System;
using System.Collections.Generic;

namespace Mvc.ViewModels
{
    public class RecordsViewModel
    {
        public int SectionId { get; set; }    
        public int TopicId { get; set; }
        public List<RecordViewModel> Records { get; set; }
    }

    public class RecordViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserEmail { get; set; }
    }
}   