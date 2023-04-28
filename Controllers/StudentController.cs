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

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetStudent([FromRoute] int id) {
            StudentDTO student = studentService.getById(id);
            if (student == null) {
                return NotFound("Student not found with id: " + id);
            }
            return Ok(student);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult getAllStudents() {
            List<StudentDTO> students = studentService.getAllStudents();
            return Ok(students);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateStudent([FromRoute] int id, StudentDTO studentDTO) {
            studentDTO.Id = id;
            StudentDTO student = studentService.update(studentDTO);
            if (student == null) {
                return NotFound("Student not found with id: " + id);
            }

            return Ok(student);
        }

    }
}
