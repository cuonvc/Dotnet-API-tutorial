using Microsoft.EntityFrameworkCore;

namespace Demo.Data {
    public class DataContext : DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {

        }

        public DataContext() {
            
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Major> Majors { get; set; }
    }
}
