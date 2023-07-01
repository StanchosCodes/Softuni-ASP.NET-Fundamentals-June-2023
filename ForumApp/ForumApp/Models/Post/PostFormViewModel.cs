using System.ComponentModel.DataAnnotations;
using static ForumApp.Data.DataConstants.Post;

namespace ForumApp.Models.Post
{
	public class PostFormViewModel
	{
		[Required]
		[StringLength(TitleMaxLenght, MinimumLength = TitleMinLength)]
		public string Title { get; set; } = null!;

		[Required]
		[StringLength(ContentMaxLength, MinimumLength = ContentMinLength)]
		public string Content { get; set; } = null!;
	}
}
