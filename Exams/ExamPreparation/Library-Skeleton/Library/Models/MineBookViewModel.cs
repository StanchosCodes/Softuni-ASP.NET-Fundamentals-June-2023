namespace Library.Models
{
	public class MineBookViewModel
	{
		public ICollection<BookViewModel> Books { get; set; } = new List<BookViewModel>();
	}
}
