using Models.Abstract;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Record")]
    public class Record : SqlEntity
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public DateTime CreationDate { get; set; }

        public int TopicId { get; set; }
        public virtual Topic Topic { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}