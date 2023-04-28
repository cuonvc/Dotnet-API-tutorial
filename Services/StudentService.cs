using Demo.DTOs;

namespace Demo.Services {
    public interface StudentService {
        StudentDTO create(StudentDTO student);

        StudentDTO getById (int id);

        List<StudentDTO> getAllStudents();

        StudentDTO update(StudentDTO student);
    }
}
