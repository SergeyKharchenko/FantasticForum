using Models.Abstract;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Models
{
    [Table("Section")]
    public class Section : SqlEntityWithImage, ICloneable
    {
        [Required]
        [StringLength(30, MinimumLength = 2)]
        public string Title { get; set; }

        public virtual Collection<Topic> Topics { get; set; }

        protected bool Equals(Section other)
        {
            return string.Equals(Title, other.Title) &&
                   string.Equals(ImageId, other.ImageId) &&
                   Topics.SequenceEqual(other.Topics);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Section) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ImageId != null ? ImageId.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Topics != null ? Topics.GetHashCode() : 0);
                return hashCode;
            }
        }

        public object Clone()
        {
            return new Section {Id = Id, Timestamp = Timestamp, ImageId = ImageId, Title = Title, Topics = Topics};
        }
    }
}