using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBoardApp.Services.Contracts;
using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Controllers
{
	[Authorize]
	public class BoardController : Controller
	{
		private readonly IBoardService boardService;

        public BoardController(IBoardService boardService)
        {
			this.boardService = boardService;
        }

        public async Task<IActionResult> All()
		{
			IEnumerable<BoardViewModel> boards = await this.boardService.GetAllAsync();

			return View(boards);
		}
	}
}
