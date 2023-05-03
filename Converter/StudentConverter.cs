using Demo.Configuration;
using Demo.DTOs;
using Demo.DTOs.Request;

namespace Demo.Converter {
    public class StudentConverter {

        private readonly SecurityConfiguration securityConfiguration;

        public StudentConverter(SecurityConfiguration securityConfiguration) {
            this.securityConfiguration = securityConfiguration;
        }

        public Student ToEntity(StudentDTO dto) {
            Student student = new Student();
            student.FirstName = dto.FirstName;
            student.LastName = dto.LastName;
            student.Username = dto.Username;
            student.Address = dto.Address;
            student.Age = dto.Age;

            return student;
        }

        public void ToEntity(StudentDTO dto, Student entity) {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.Username = dto.Username;
            entity.Address = dto.Address;
            entity.Age = dto.Age;
        }

        public StudentDTO ToDTO(Student entity) {
            StudentDTO student = new StudentDTO();
            student.Id = entity.Id;
            student.FirstName = entity.FirstName;
            student.LastName = entity.LastName;
            student.Username = entity.Username;
            student.Address = entity.Address;
            student.Age = entity.Age;

            return student;
        }

        public Student RegRequestToEntity(RegisterRequest request) {
            Student student = securityConfiguration.encodePassword(request);
            
            student.Username = request.Username;
            student.FirstName = request.FirstName;
            student.LastName = request.LastName;
            return student;
        }
    }
}
