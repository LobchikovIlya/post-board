using Microsoft.EntityFrameworkCore;
using PostBoard.Api.Models;

namespace PostBoard.Api
{
    public class PostBoardContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Birthday> Birthdays { get; set; }

        public PostBoardContext(DbContextOptions<PostBoardContext> options) : base(options) { }

       
    }
}
