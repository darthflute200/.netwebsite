using Microsoft.EntityFrameworkCore;

namespace EmreSarıyer.Models{
    public class DataContext : DbContext{
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<UserModel> User => Set<UserModel>();
        public DbSet<BlogModel> Blog => Set<BlogModel>();
        public DbSet<CommentModel> Comment => Set<CommentModel>();
    }
}