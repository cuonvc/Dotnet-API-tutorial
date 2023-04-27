using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller {

        private readonly DataContext context;

        public StudentController(DataContext context) {
            this.context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student) {
            await context.Students.AddAsync(student);
            await context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllStudents () {
             return Ok(await context.Students.ToListAsync());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> getById([FromRoute] int id) {
            Student student = await context.Students.FindAsync(id);
            if (student == null) {
                return BadRequest("Student not exist with id: " + id);
            }
            return Ok(student);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> updateById ([FromRoute] int id, Student student) {
            Student oldStudent = await context.Students.FindAsync(id);
            if (oldStudent == null) {
                return BadRequest("Student not found with id: " + id);
            }

            oldStudent.Name = student.Name;
            oldStudent.Age = student.Age;
            oldStudent.Address = student.Address;

            await context.SaveChangesAsync();
            return Ok(oldStudent);
        }

    }
}
