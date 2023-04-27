using Microsoft.EntityFrameworkCore;

namespace Demo.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        public DbSet<Student> Students { get; set; }
    }
}
