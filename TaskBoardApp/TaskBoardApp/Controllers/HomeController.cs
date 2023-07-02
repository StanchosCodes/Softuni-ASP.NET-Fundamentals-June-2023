using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoardApp.Services.Contracts;
using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly IHomeService homeService;

        public HomeController(IHomeService homeService)
        {
			this.homeService = homeService;
        }

        public IActionResult Index()
		{
			List<HomeBoardModel> tasksCounts = homeService.GetTasksCount().ToList();

			int userTasksCount = -1;

			if (User?.Identity?.IsAuthenticated ?? false)
			{
				var currentUserId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
				userTasksCount = this.homeService.GetUserTasksCount(currentUserId!);
			}

			HomeViewModel homeModel = new HomeViewModel()
			{
				AllTasksCount = this.homeService.GetAllTasksCount(),
				BoardsWithTasksCount = tasksCounts,
				UserTasksCount = userTasksCount
			};

			return View(homeModel);
		}
	}
}