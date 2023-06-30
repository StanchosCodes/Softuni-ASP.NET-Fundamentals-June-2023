using MVCIntroDemo.Models;

namespace MVCIntroDemo.Data
{
	public static class Products
	{
		public static IEnumerable<ProductViewModel> products = new List<ProductViewModel>()
		{
			new ProductViewModel()
			{
				Id = 1,
				Name = "Cheese",
				Price = 7.00m
			},

			new ProductViewModel()
			{
				Id = 2,
				Name = "Ham",
				Price = 5.50m
			},

			new ProductViewModel()
			{
				Id = 3,
				Name = "Bread",
				Price = 1.50m
			}
		};
	}
}
