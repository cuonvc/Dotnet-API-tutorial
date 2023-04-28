using Demo.DTOs;
using Demo.Services;
using Demo.Services.Impl;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : Controller {

        private readonly StudentService studentService;

        public StudentController(StudentService studentService) {
            this.studentService = studentService;
        }

        [HttpPost]
        public IActionResult CreateStudent(StudentDTO studentDTO) {
            StudentDTO newStudent = studentService.create(studentDTO);
            return Ok(newStudent);
        }

        

    }
}
