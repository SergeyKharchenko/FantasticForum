using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Models.Abstract
{
    public abstract class SqlEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [Timestamp]
        [HiddenInput]
        public byte[] Timestamp { get; set; }
    }
}