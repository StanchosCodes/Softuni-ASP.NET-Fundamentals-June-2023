using System.ComponentModel.DataAnnotations;

using static Homies.Data.ValidationConstants.EntityValidationConstants.Event;

namespace Homies.Models
{
	public class AddEditViewModel
	{
		[Required]
		[StringLength(EventNameMaxLength, MinimumLength = EventNameMinLength)]
		public string Name { get; set; } = null!;

		[Required]
		[StringLength(EventDescriptionMaxLength, MinimumLength = EventDescriptionMinLength)]
		public string Description { get; set; } = null!;

		[Required]
		public DateTime Start { get; set; }

		[Required]
		public DateTime End { get; set; }

		[Required]
		public int TypeId { get; set; }

        public IEnumerable<TypeViewModel> Types { get; set; } = new List<TypeViewModel>();
    }
}
