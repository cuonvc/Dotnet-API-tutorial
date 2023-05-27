using Demo.DTOs;
using Demo.DTOs.Response;

namespace Demo.Services {
    public interface StudentService {
        // ResponseObject<StudentDTO> create(StudentDTO student);

        ResponseObject<StudentDTO> getProfile();

        ResponseObject<List<StudentDTO>> getAllStudents(int pageSize, int pageNo, string sortBy);

        ResponseObject<StudentDTO> update(StudentDTO student);
    }
}
