using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Services.Contracts
{
	public interface IHomeService
	{
		ICollection<HomeBoardModel> GetTasksCount();

		int GetUserTasksCount(string userId);

		int GetAllTasksCount();
	}
}
