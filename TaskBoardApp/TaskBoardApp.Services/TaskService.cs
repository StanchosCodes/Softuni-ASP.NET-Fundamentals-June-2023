using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Services.Contracts;
using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Services
{
	public class TaskService : ITaskService
	{
		private readonly TaskBoardAppDbContext context;

        public TaskService(TaskBoardAppDbContext context)
        {
			this.context = context;
        }

        public async Task AddAsync(string ownerId, TaskFormViewModel taskModel)
		{
			Data.Models.Task newTask = new Data.Models.Task()
			{
				Title = taskModel.Title,
				Description = taskModel.Description,
				BoardId = taskModel.BoardId,
				CreatedOn = DateTime.UtcNow,
				OwnerId = ownerId
			};

			await this.context.Tasks.AddAsync(newTask);
			await this.context.SaveChangesAsync();
		}

		public async Task<TaskDetailsViewModel> GetDetailsAsync(string taskId)
		{
			TaskDetailsViewModel? task = await this.context.Tasks
				.Select(t => new TaskDetailsViewModel()
				{
					Id = t.Id.ToString(),
					Title = t.Title,
					Description = t.Description,
					CreatedOn = t.CreatedOn.ToString("f"),
					Board = t.Board!.Name,
					Owner = t.Owner.UserName
				})
				.FirstOrDefaultAsync(t => t.Id == taskId);

			return task;
		}

		public async Task<Data.Models.Task> GetTaskAsync(string id)
		{
			Data.Models.Task? taskToEdit = await this.context.Tasks.FirstOrDefaultAsync(t => t.Id.ToString() == id);

			return taskToEdit;
		}

		public async Task EditAsync(TaskFormViewModel taskModel, Data.Models.Task taskToEdit)
		{
			taskToEdit.Title = taskModel.Title;
			taskToEdit.Description = taskModel.Description;
			taskToEdit.BoardId = taskModel.BoardId;

			await this.context.SaveChangesAsync();
		}

		public async Task<TaskViewModel> CreateTaskViewModelAsync(string id)
		{
			TaskViewModel? task = await this.context.Tasks
				.Select(t => new TaskViewModel()
				{
					Id = t.Id.ToString(),
					Title = t.Title,
					Description = t.Description
				})
				.FirstOrDefaultAsync(t => t.Id == id);

			return task;
		}

		public async Task DeleteTaskAsync(Data.Models.Task taskToDelete)
		{
			this.context.Tasks.Remove(taskToDelete);
			await this.context.SaveChangesAsync();
		}
	}
}
