using ForumApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Data
{
	public class ForumAppDbContext : DbContext
	{
		private Post FirstPost { get; set; } = null!;
        private Post SecondPost { get; set; } = null!;
        private Post ThirdPost { get; set; } = null!;

        public ForumAppDbContext(DbContextOptions<ForumAppDbContext> options)
            : base(options)
        {
            Database.Migrate();
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			SeedPosts();

			modelBuilder.Entity<Post>()
				.HasData(this.FirstPost, this.SecondPost, this.ThirdPost);

			base.OnModelCreating(modelBuilder);
		}

		public DbSet<Post> Posts { get; init; }

        private void SeedPosts()
        {
            this.FirstPost = new Post()
            {
                Id = 1,
                Title = "My first post",
                Content = "My first posts content"
            };

			this.SecondPost = new Post()
			{
				Id = 2,
				Title = "My second post",
				Content = "My second posts content"
			};

			this.ThirdPost = new Post()
			{
				Id = 3,
				Title = "My third post",
				Content = "My third posts content"
			};
		}
    }
}
