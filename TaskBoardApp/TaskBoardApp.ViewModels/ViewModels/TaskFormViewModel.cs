using System.ComponentModel.DataAnnotations;
using static TaskBoardApp.Common.ValidationConstants.Task;

namespace TaskBoardApp.ViewModels
{
	public class TaskFormViewModel
	{
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "Title should be between {2} and {1} characters long!")]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "Description should be between {2} and {1} characters long!")]
        public string Description { get; set; } = null!;

        [Display(Name = "Board")]
        public int BoardId { get; set; }

        public IEnumerable<TaskBoardViewModel> Boards { get; set; } = new HashSet<TaskBoardViewModel>();
    }
}
