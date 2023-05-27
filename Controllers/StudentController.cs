using Demo.DTOs;
using Demo.DTOs.Response;
using Demo.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers {

    [ApiController]
    [Route("api/student/")]
    public class StudentController : Controller {

        private readonly StudentService studentService;

        public StudentController(StudentService studentService) {
            this.studentService = studentService;
        }

        // [HttpPost]
        // public IActionResult CreateStudent(StudentDTO studentDTO) {
        //     ResponseObject<StudentDTO> response = studentService.create(studentDTO);
        //     return Ok(response);
        // }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("profile")]
        public IActionResult GetStudent() {
            ResponseObject<StudentDTO> response = studentService.getProfile();
            if (response.Code.Equals(StatusCodes.Status400BadRequest.ToString())) {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("all")]
        public IActionResult getAllStudents(int pageSize, int pageNo, string sortBy) {
            ResponseObject<List<StudentDTO>> response = studentService.getAllStudents(pageSize, pageNo, sortBy);
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
