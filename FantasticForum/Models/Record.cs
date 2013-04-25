using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Abstract;

namespace Models
{
    [Table("Record")]
    public class Record : SqlEntity
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }

        public string CreationDate { get; set; }

        public int TopicId { get; set; }
        public Topic Topic { get; set; }
    }
}