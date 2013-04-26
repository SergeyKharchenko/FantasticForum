using System.ComponentModel.DataAnnotations;

namespace Models.Abstract
{
    public abstract class SqlEntityWithImage : SqlEntity
    {
        [ScaffoldColumn(false)]
        public string ImageId { get; set; }
    }
}