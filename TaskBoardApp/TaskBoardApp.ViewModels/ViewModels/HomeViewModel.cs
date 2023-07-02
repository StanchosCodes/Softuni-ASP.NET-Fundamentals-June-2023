namespace TaskBoardApp.ViewModels
{
	public class HomeViewModel
	{
        public int AllTasksCount { get; set; }

        public ICollection<HomeBoardModel> BoardsWithTasksCount { get; set; } = new List<HomeBoardModel>();

        public int UserTasksCount { get; set; }
    }
}
