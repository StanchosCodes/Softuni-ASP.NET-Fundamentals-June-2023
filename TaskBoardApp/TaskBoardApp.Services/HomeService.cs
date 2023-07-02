using TaskBoardApp.Data;
using TaskBoardApp.Services.Contracts;
using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Services
{
	public class HomeService : IHomeService
	{
		private readonly TaskBoardAppDbContext context;

        public HomeService(TaskBoardAppDbContext context)
        {
			this.context = context;
        }

		public ICollection<HomeBoardModel> GetTasksCount()
		{
			IEnumerable<string> boards = this.context
				.Boards
				.Select(b => b.Name)
				.Distinct()
				.ToArray();

			List<HomeBoardModel> tasksWithCount = new List<HomeBoardModel>();

			foreach (string boardName in boards)
			{
				int tasksInBoard = this.context.Tasks.Where(t => t.Board!.Name == boardName).Count();

				tasksWithCount.Add(new HomeBoardModel()
				{
					BoardName = boardName,
					TasksCount = tasksInBoard
				});
			}

			return tasksWithCount;
		}

		public int GetUserTasksCount(string userId)
		{
			int count = this.context.Tasks.Where(t => t.OwnerId == userId).Count();

			return count;
		}

		public int GetAllTasksCount()
		{
			int count = this.context.Tasks.Count();

			return count;
		}
	}
}
