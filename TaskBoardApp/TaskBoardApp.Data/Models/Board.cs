using System.ComponentModel.DataAnnotations;
using static TaskBoardApp.Common.ValidationConstants.Board;

namespace TaskBoardApp.Data.Models
{
	public class Board
	{
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }
}
