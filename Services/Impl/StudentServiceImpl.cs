using System.Security.Claims;
using Demo.Converter;
using Demo.DTOs;
using Demo.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Services.Impl {
    public class StudentServiceImpl : StudentService {

        private readonly DataContext dataContext;

        private readonly StudentConverter studentConverter;
        
        private readonly IHttpContextAccessor httpContextAccessor;

        public StudentServiceImpl (DataContext dataContext, StudentConverter studentConverter,
            IHttpContextAccessor httpContextAccessor) {
            this.dataContext = dataContext;
            this.studentConverter = studentConverter;
            this.httpContextAccessor = httpContextAccessor;
        }

        //break
        // public ResponseObject<StudentDTO> create (StudentDTO studentDTO) {
        //
        //     Student student = studentConverter.ToEntity(studentDTO);
        //     dataContext.Students.Add(student);
        //     dataContext.SaveChanges();
        //
        //     ResponseObject<StudentDTO> responseObject = new ResponseObject<StudentDTO>();
        //
        //     return responseObject.responseSuccess("Created student successfully", studentConverter.ToDTO(student));
        // }

        public ResponseObject<StudentDTO> getProfile() {

            ResponseObject<StudentDTO> responseObject = new ResponseObject<StudentDTO>();

            string username = httpContextAccessor.HttpContext.User.FindFirst("Username").Value;

            Student student = dataContext.Students
                .FromSql($"SELECT * FROM Students WHERE Username = {username}")
                .FirstOrDefault();
            
            if (student == null) {
                return responseObject.responseError("Student not found with username: " + username, StatusCodes.Status400BadRequest.ToString(), null);
            }

            return responseObject.responseSuccess("Success", studentConverter.ToDTO(student));
        }

        public ResponseObject<List<StudentDTO>> getAllStudents(int pageSize, int pageNo, string sortBy) {

            List<StudentDTO> datas = dataContext.Set<Student>()
                //tạm thời để mặc định sort by name
                .OrderBy(student => student.FirstName)
                .Skip((pageNo - 1) * pageSize)
                .Take(pageSize)
                .Select(student => studentConverter.ToDTO(student))
                .ToList();

            ResponseObject<List<StudentDTO>> responseObjectList = new ResponseObject<List<StudentDTO>>();
            return responseObjectList.responseSuccess("Success", datas);
        }

        public ResponseObject<StudentDTO> update(StudentDTO studentDTO) {

            ResponseObject<StudentDTO> responseObject = new ResponseObject<StudentDTO>();

            Student student = dataContext.Students.Find(studentDTO.Id);
            if (student == null) {
                return responseObject.responseError("Student not found with idL " + studentDTO.Id,
                    StatusCodes.Status404NotFound.ToString(), null);
            }

            studentConverter.ToEntity(studentDTO, student);
            dataContext.SaveChanges();
            return responseObject.responseSuccess("Success", studentConverter.ToDTO(student));

        }
    }
}
