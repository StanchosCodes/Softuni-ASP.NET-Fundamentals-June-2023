using ForumApp.Data;
using ForumApp.Data.Models;
using ForumApp.Models.Post;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Controllers
{
	public class PostController : Controller
	{
		private readonly ForumAppDbContext context;

        public PostController(ForumAppDbContext context)
        {
			this.context = context;
        }

        public async Task<IActionResult> All()
		{
			List<PostViewModel> posts = await context.Posts
				.Select(p => new PostViewModel()
				{
					Id = p.Id,
					Title = p.Title,
					Content = p.Content
				})
				.ToListAsync();

			return View(posts);
		}

		[HttpGet]
		public IActionResult Add()
		{
			PostFormViewModel formModel = new PostFormViewModel();

			return View(formModel);
		}

		[HttpPost]
		public async Task<IActionResult> Add(PostFormViewModel formModel)
		{
			Post newPost = new Post()
			{
				Title = formModel.Title,
				Content = formModel.Content
			};

			await context.Posts.AddAsync(newPost);
			await context.SaveChangesAsync();

			return RedirectToAction("All", "Post");
		}

		[HttpGet]
		public async Task<IActionResult> Edit(int id)
		{
			Post? postToEdit = await this.context.Posts
				.FindAsync(id);

			if (postToEdit == null)
			{
				throw new ArgumentException("Invalid post id!");
			}

			return View(new PostFormViewModel()
			{
				Title = postToEdit.Title,
				Content = postToEdit.Content
			});
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, PostFormViewModel editedPost)
		{
			Post? postToEdit = await this.context.Posts
				.FindAsync(id);

			if (postToEdit == null)
			{
				throw new ArgumentException("Invalid post id!");
			}

			postToEdit.Title = editedPost.Title;
			postToEdit.Content = editedPost.Content;

			await this.context.SaveChangesAsync();

			return RedirectToAction("All", "Post");
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			Post? postToDelete = await this.context.Posts
				.FindAsync(id);

			if (postToDelete == null)
			{
				throw new ArgumentException("Invalid post id!");
			}

			this.context.Remove(postToDelete);

			await this.context.SaveChangesAsync();

			return RedirectToAction("All", "Post");
		}
	}
}
