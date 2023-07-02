using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Services.Contracts
{
	public interface IBoardService
	{
		Task<IEnumerable<BoardViewModel>> GetAllAsync();

		Task<IEnumerable<TaskBoardViewModel>> GetBoardsAsync();
	}
}
