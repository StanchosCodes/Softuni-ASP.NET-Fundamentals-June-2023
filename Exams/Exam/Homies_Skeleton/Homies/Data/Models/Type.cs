using System.ComponentModel.DataAnnotations;

using static Homies.Data.ValidationConstants.EntityValidationConstants.Type;

namespace Homies.Data.Models
{
	public class Type
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(TypeNameMaxLength)]
        public string Name { get; set; } = null!;

		public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
