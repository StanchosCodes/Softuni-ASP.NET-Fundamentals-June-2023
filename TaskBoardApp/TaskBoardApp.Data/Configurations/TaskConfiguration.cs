using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task = TaskBoardApp.Data.Models.Task;

namespace TaskBoardApp.Data.Configurations
{
	internal class TaskConfiguration : IEntityTypeConfiguration<Task>
	{
		public void Configure(EntityTypeBuilder<Task> builder)
		{
			ICollection<Task> tasks = GenerateTasks();

			builder
				.HasData(tasks);
		}

		private ICollection<Task> GenerateTasks()
		{
			ICollection<Task> tasks = new HashSet<Task>();

			Task currentTask;

			currentTask = new Task()
			{
				Title = "Implement CSS styles",
				Description = "Implement better styling for all public pages",
				CreatedOn = DateTime.UtcNow.AddDays(-200),
				OwnerId = "9881bc74-6d04-4a05-b2d8-9b083654488c",
				BoardId = 1
			};

			tasks.Add(currentTask);

			currentTask = new Task()
			{
				Title = "Android Client App",
				Description = "Create Android client App for the RESTful TaskBoard service",
				CreatedOn = DateTime.UtcNow.AddMonths(-5),
				OwnerId = "992c51bd-f5ec-4a3d-9ec1-8152eaeff526",
				BoardId = 1
			};

			tasks.Add(currentTask);

			currentTask = new Task()
			{
				Title = "Desktop Client App",
				Description = "Create Desktop client App for the RESTful TaskBoard service",
				CreatedOn = DateTime.UtcNow.AddMonths(-1),
				OwnerId = "9881bc74-6d04-4a05-b2d8-9b083654488c",
				BoardId = 2
			};

			tasks.Add(currentTask);

			currentTask = new Task()
			{
				Title = "Create Tasks",
				Description = "Implement [Create Task] page for adding tasks",
				CreatedOn = DateTime.UtcNow.AddYears(-1),
				OwnerId = "9881bc74-6d04-4a05-b2d8-9b083654488c",
				BoardId = 3
			};

			tasks.Add(currentTask);

			return tasks;
		}
	}
}
