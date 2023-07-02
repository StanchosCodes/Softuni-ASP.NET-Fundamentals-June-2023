using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskBoardApp.Data.Models;
using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Data
{
	public class TaskBoardAppDbContext : IdentityDbContext<IdentityUser>
	{
		public TaskBoardAppDbContext(DbContextOptions<TaskBoardAppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Task> Tasks { get; set; } = null!;

		public DbSet<Board> Boards { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder builder)
		{
			// getting the configuration file with reflection to apply it on the builder

			builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(TaskBoardAppDbContext)) ?? Assembly.GetExecutingAssembly());

			base.OnModelCreating(builder);
		}
	}
}