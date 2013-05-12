using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
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

        [Required]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string Text { get; set; }
        
        public string EncodedText { get; set; }

        public DateTime CreationDate { get; set; }
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public byte[] Timestamp { get; set; }
    }
}   