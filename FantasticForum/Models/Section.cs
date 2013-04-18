using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Models.Abstract;

namespace Models
{
    [Table("Section")]
    public class Section : SqlEntity
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }

        [ScaffoldColumn(false)]     
        public string ImageId { get; set; }

        public virtual Collection<Topic> Topics { get; set; }
    }
}