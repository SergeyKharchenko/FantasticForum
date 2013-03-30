using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Section")]
    public class Section : Entity
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }
    }
}