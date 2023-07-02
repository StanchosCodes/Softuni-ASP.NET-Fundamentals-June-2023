using Microsoft.EntityFrameworkCore;
using TaskBoardApp.Data;
using TaskBoardApp.Services.Contracts;
using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Services
{
	public class BoardService : IBoardService
	{
		private readonly TaskBoardAppDbContext context;

        public BoardService(TaskBoardAppDbContext context)
        {
			this.context = context;
        }

        public async Task<IEnumerable<BoardViewModel>> GetAllAsync()
		{
			IEnumerable<BoardViewModel> allBoards = await this.context.Boards
				.Select(b => new BoardViewModel()
				{
					Id = b.Id,
					Name = b.Name,
					Tasks = b.Tasks
					.Select(t => new TaskViewModel()
					{
						Id = t.Id.ToString(),
						Title = t.Title,
						Description = t.Description,
						Owner = t.Owner.UserName
					})
					.ToArray()
				})
				.ToListAsync();

			return allBoards;
		}

		public async Task<IEnumerable<TaskBoardViewModel>> GetBoardsAsync()
		{
			IEnumerable<TaskBoardViewModel> boards = await this.context
				.Boards
				.Select(b => new TaskBoardViewModel()
				{
					Id = b.Id,
					Name = b.Name
				})
				.ToListAsync();

			return boards;
		}
	}
}