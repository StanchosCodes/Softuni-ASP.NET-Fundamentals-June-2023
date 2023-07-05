using Library.Data;
using Library.Data.Models;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Library.Controllers
{
	[Authorize]
	public class BookController : Controller
	{
		private readonly LibraryDbContext context;

        public BookController(LibraryDbContext context)
        {
			this.context = context;
        }

        public async Task<IActionResult> All()
		{
			IEnumerable<BookViewModel> books = await this.context.Books
				.Select(b => new BookViewModel()
				{
					Id = b.Id,
					Title = b.Title,
					Author = b.Author,
					Rating = b.Rating,
					ImageUrl = b.ImageUrl,
					Category = b.Category.Name
				})
				.ToListAsync();

			return View(books);
		}

		public async Task<IActionResult> Mine()
		{
			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			List<BookViewModel> usersBooks = await this.context.IdentityUsersBooks
				.Where(ub => ub.CollectorId == userId)
				.Select(ub => new BookViewModel()
				{
					Id = ub.Book!.Id,
					Title = ub.Book.Title,
					Author = ub.Book.Author,
					ImageUrl = ub.Book.ImageUrl,
					Description = ub.Book.Description,
					Category = ub.Book.Category.Name
				})
				.ToListAsync();

			MineBookViewModel mineBooks = new MineBookViewModel()
			{
				Books = usersBooks
			};

			return View(mineBooks);
		}

		[HttpGet]
		public async Task<IActionResult> Add()
		{
			List<CategoryViewModel> categories = await this.context.Categories
				.Select(c => new CategoryViewModel()
				{
					Id = c.Id,
					Name = c.Name
				})
				.ToListAsync();

			AddViewModel bookModel = new AddViewModel()
			{
				Categories = categories
			};

			return View(bookModel);
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddViewModel bookModel)
		{
			if (!ModelState.IsValid)
			{
				return View(bookModel);
			}

			try
			{
				Book newBook = new Book()
				{
					Title = bookModel.Title,
					Author = bookModel.Author,
					Description = bookModel.Description,
					ImageUrl = bookModel.Url,
					Rating = bookModel.Rating,
					CategoryId = bookModel.CategoryId
				};

				await this.context.Books.AddAsync(newBook);
				await this.context.SaveChangesAsync();

				return RedirectToAction("All", "Book");
			}
			catch (Exception)
			{
				ModelState.AddModelError(string.Empty, "Failed to add book!");

				return View(bookModel);
			}
		}

		public async Task<IActionResult> AddToCollection(int id)
		{
			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			BookViewModel? book = await this.context.Books
				.Where(b => b.Id == id)
				.Select(b => new BookViewModel()
				{
					Id = b.Id,
					Title = b.Title,
					Author = b.Author,
					ImageUrl = b.ImageUrl,
					Description = b.Description,
					Rating = b.Rating,
					Category = b.Category.Name
				})
				.FirstOrDefaultAsync();

			if (book == null)
			{
				return RedirectToAction("All", "Book");
			}

			bool isAdded = await this.context.IdentityUsersBooks.AnyAsync(ub => ub.BookId == id && ub.CollectorId == userId);

			if (!isAdded)
			{
				IdentityUserBook userBook = new IdentityUserBook()
				{
					CollectorId = userId!,
					BookId = book.Id
				};

				await this.context.IdentityUsersBooks.AddAsync(userBook);
				await this.context.SaveChangesAsync();
			}

			return RedirectToAction("All", "Book");
		}

		public async Task<IActionResult> RemoveFromCollection(int id)
		{
			BookViewModel? book = await this.context.Books
				.Where(b => b.Id == id)
				.Select(b => new BookViewModel()
				{
					Id = b.Id,
					Title = b.Title,
					Author = b.Author,
					ImageUrl = b.ImageUrl,
					Description = b.Description,
					Rating = b.Rating,
					Category = b.Category.Name
				})
				.FirstOrDefaultAsync();

			if (book == null)
			{
				return RedirectToAction("All", "Book");
			}

			var userId = this.User!.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			var userBook = await this.context.IdentityUsersBooks
				.FirstOrDefaultAsync(ub => ub.BookId == id && ub.CollectorId == userId);

			if (userBook != null)
			{
				this.context.IdentityUsersBooks.Remove(userBook);
				await this.context.SaveChangesAsync();
			}

			return RedirectToAction("Mine", "Book");
		}
	}
}
