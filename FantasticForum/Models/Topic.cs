using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Abstract;

namespace Models
{
    [Table("Topic")]
    public class Topic : Entity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        public virtual int SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}