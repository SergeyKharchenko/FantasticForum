using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Abstract;

namespace Models
{
    [Table("Topic")]
    public class Topic : SqlEntity
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Title { get; set; }

        public virtual int SectionId { get; set; }
        public virtual Section Section { get; set; }

        public virtual Collection<Record> Records { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Topic) obj);
        }

        protected bool Equals(Topic other)
        {
            return string.Equals(Title, other.Title);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Title != null ? Title.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}