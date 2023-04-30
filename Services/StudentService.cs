using Demo.DTOs;
using Demo.DTOs.Response;

namespace Demo.Services {
    public interface StudentService {
        ResponseObject<StudentDTO> create(StudentDTO student);

        ResponseObject<StudentDTO> getById (int id);

        ResponseObject<List<StudentDTO>> getAllStudents();

        ResponseObject<StudentDTO> update(StudentDTO student);
    }
}
