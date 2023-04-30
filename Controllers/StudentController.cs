using Demo.DTOs;
using Demo.DTOs.Response;
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
            ResponseObject<StudentDTO> response = studentService.create(studentDTO);
            return Ok(response);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetStudent([FromRoute] int id) {
            ResponseObject<StudentDTO> response = studentService.getById(id);
            if (response.Code.Equals(StatusCodes.Status400BadRequest.ToString())) {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult getAllStudents() {
            ResponseObject<List<StudentDTO>> response = studentService.getAllStudents();
            return Ok(response);
        }

        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateStudent([FromRoute] int id, StudentDTO studentDTO) {
            studentDTO.Id = id;
            ResponseObject<StudentDTO> student = studentService.update(studentDTO);
            if (student == null) {
                return NotFound("Student not found with id: " + id);
            }

            return Ok(student);
        }

    }
}
