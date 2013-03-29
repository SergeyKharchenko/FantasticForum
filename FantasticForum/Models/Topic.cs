using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Topic")]
    public class Topic : Entity
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }
    }
}