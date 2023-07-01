using System.ComponentModel.DataAnnotations;
using static ForumApp.Data.DataConstants.Post;

namespace ForumApp.Models.Post
{
	public class PostViewModel
	{
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;
    }
}
