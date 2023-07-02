using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Services.Contracts
{
	public interface ITaskService
	{
		Task AddAsync(string ownerId, TaskFormViewModel taskModel);

		Task<TaskDetailsViewModel> GetDetailsAsync(string taskId);

		Task<Data.Models.Task> GetTaskAsync(string id);

		Task EditAsync(TaskFormViewModel taskModel, Data.Models.Task taskToEdit);

		Task<TaskViewModel> CreateTaskViewModelAsync(string id);

		Task DeleteTaskAsync(Data.Models.Task taskToDelete);
	}
}
