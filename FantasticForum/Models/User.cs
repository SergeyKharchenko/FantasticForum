using Models.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("User")]
    public class User : SqlEntityWithImage
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 2)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual Collection<Record> Records { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(36, MinimumLength = 36)]
        public string Guid { get; set; }

        [ScaffoldColumn(false)]
        public bool IsConfirmed { get; set; }
    }
}