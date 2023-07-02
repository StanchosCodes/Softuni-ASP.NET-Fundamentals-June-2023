using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskBoardApp.Services.Contracts;
using TaskBoardApp.ViewModels;

namespace TaskBoardApp.Controllers
{
	[Authorize]
	public class TaskController : Controller
	{
		private readonly IBoardService boardService;
		private readonly ITaskService taskService;

        public TaskController(IBoardService boardService, ITaskService taskService)
        {
			this.boardService = boardService;
			this.taskService = taskService;
        }

        [HttpGet]
		public async Task<IActionResult> Create()
		{
			TaskFormViewModel taskModel = new TaskFormViewModel()
			{
				Boards = await this.boardService.GetBoardsAsync()
			};
			return View(taskModel);
		}

		[HttpPost]
		public async Task<IActionResult> Create(TaskFormViewModel taskModel)
		{
			if (!ModelState.IsValid)
			{
				taskModel.Boards = await this.boardService.GetBoardsAsync();

				return View(taskModel);
			}

			var currentUserId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			await this.taskService.AddAsync(currentUserId!, taskModel);

			return RedirectToAction("All", "Board");
		}

		public async Task<IActionResult> Details(string id)
		{
			TaskDetailsViewModel task = await this.taskService.GetDetailsAsync(id);

			if (task == null)
			{
				return BadRequest();
			}

			return View(task);
		}

		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{
			Data.Models.Task taskToEdit = await this.taskService.GetTaskAsync(id);

			if (taskToEdit == null)
			{
				return BadRequest();
			}

			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != taskToEdit.OwnerId)
			{
				return Unauthorized();
			}

			TaskFormViewModel taskModel = new TaskFormViewModel()
			{
				Title = taskToEdit.Title,
				Description = taskToEdit.Description,
				BoardId = taskToEdit.BoardId,
				Boards = await this.boardService.GetBoardsAsync()
			};

			return View(taskModel);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, TaskFormViewModel taskModel)
		{
			Data.Models.Task taskToEdit = await this.taskService.GetTaskAsync(id);

			if (taskToEdit == null)
			{
				return BadRequest();
			}

			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != taskToEdit.OwnerId)
			{
				return Unauthorized();
			}

			if (!ModelState.IsValid)
			{
				taskModel.Boards = await this.boardService.GetBoardsAsync();

				return View(taskModel);
			}

			await this.taskService.EditAsync(taskModel, taskToEdit);

			return RedirectToAction("All", "Board");
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			Data.Models.Task taskToDelete = await this.taskService.GetTaskAsync(id);

			if (taskToDelete == null)
			{
				return BadRequest();
			}

			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != taskToDelete.OwnerId)
			{
				return Unauthorized();
			}

			TaskViewModel task = await this.taskService.CreateTaskViewModelAsync(id);

			return View(task);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(TaskViewModel taskModel)
		{
			Data.Models.Task taskToDelete = await this.taskService.GetTaskAsync(taskModel.Id);

			if (taskToDelete == null)
			{
				return BadRequest();
			}

			var userId = this.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId != taskToDelete.OwnerId)
			{
				return Unauthorized();
			}

			await this.taskService.DeleteTaskAsync(taskToDelete);

			return RedirectToAction("All", "Board");
		}
	}
}
