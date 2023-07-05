﻿using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class AddViewModel
	{
		[Required]
		[StringLength(50, MinimumLength = 10)]
		public string Title { get; set; } = null!;

		[Required]
		[StringLength(50, MinimumLength = 5)]
		public string Author { get; set; } = null!;

		[Required]
		[StringLength(5000, MinimumLength = 5)]
		public string Description { get; set; } = null!;

		[Required(AllowEmptyStrings = false)]
		public string Url { get; set; } = null!;

		[Required]
		[Range(typeof(decimal), "0.00", "10.00")]
		public decimal Rating { get; set; }

		[Required]
		[Range(1, 5)]
		public int CategoryId { get; set; }

		public IEnumerable<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
