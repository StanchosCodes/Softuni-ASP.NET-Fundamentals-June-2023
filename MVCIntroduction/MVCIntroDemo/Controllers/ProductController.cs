using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MVCIntroDemo.Models;
using System.Text;
using System.Text.Json;
using static MVCIntroDemo.Data.Products;

namespace MVCIntroDemo.Controllers
{
	public class ProductController : Controller
	{
		[ActionName("My-Products")]
		public IActionResult All(string keyword)
		{
			if (keyword != null)
			{
				IEnumerable<ProductViewModel> foundProducts = products
					.Where(p => p.Name.ToLower().Contains(keyword.ToLower()));

				return View(foundProducts);
			}

			return View(products);
		}

		[HttpGet]
		public IActionResult ById()
		{
			ProductViewModel model = new ProductViewModel();

			return View(model);
		}

		[HttpPost]
		public IActionResult ById(int id)
		{
			ProductViewModel? product = products.FirstOrDefault(p => p.Id == id);

			if (product == null)
			{
				return BadRequest();
			}

			return View(product);
		}

		public IActionResult AllAsJson()
		{
			var options = new JsonSerializerOptions()
			{
				WriteIndented = true
			};

			return Json(products, options);
		}

		public IActionResult AllAsText()
		{
			StringBuilder result = new StringBuilder();

			foreach (ProductViewModel product in products)
			{
				result.AppendLine($"{product.Id}: {product.Name} - {product.Price:f2} lv.");
			}

			return Content(result.ToString().TrimEnd());
		}

		public IActionResult AllAsTextFile()
		{
			StringBuilder result = new StringBuilder();

			foreach (ProductViewModel product in products)
			{
				result.AppendLine($"{product.Id}: {product.Name} - {product.Price:f2} lv.");
			}

			Response.Headers.Add(HeaderNames.ContentDisposition,
				@"attachment;filename=AllProducts.txt");

			return File(Encoding.UTF8.GetBytes(result.ToString().TrimEnd()), "text/plain");
		}
	}
}
