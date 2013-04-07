using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Models.Abstract;

namespace Models
{
    [Table("Section")]
    public class Section : Entity
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }

        [ScaffoldColumn(false)]     
        public string ImageId { get; set; }

        [Timestamp]
        [HiddenInput]
        public byte[] Timestamp { get; set; }
    }
}