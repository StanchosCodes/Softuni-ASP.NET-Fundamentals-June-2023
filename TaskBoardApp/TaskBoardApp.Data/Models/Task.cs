using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static TaskBoardApp.Common.ValidationConstants.Task;

namespace TaskBoardApp.Data.Models
{
	public class Task
	{
        public Task()
        {
            this.Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

		public DateTime CreatedOn { get; set; }

        [ForeignKey("Board")]
        public int BoardId { get; set; }

        public Board? Board { get; set; }

        [Required]
        [ForeignKey("Owner")]
        public string OwnerId { get; set; } = null!;

		public IdentityUser Owner { get; set; } = null!;
	}
}
