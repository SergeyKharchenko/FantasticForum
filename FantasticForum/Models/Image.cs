using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Image")]
    public class Image : Entity
    {
        [ScaffoldColumn(false)]
        public string ImageMimeType { get; set; }

        [ScaffoldColumn(false)]
        public string FileName { get; set; }
    }
}