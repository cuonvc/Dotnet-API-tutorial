using Demo.DTOs;

namespace Demo.Converter {
    public class StudentConverter {

        public Student ToEntity(StudentDTO dto) {
            Student student = new Student();
            student.Name = dto.Name;
            student.Address = dto.Address;
            student.Age = dto.Age;

            return student;
        }

        public void ToEntity(StudentDTO dto, Student entity) {
            entity.Name = dto.Name;
            entity.Address = dto.Address;
            entity.Age = dto.Age;
        }

        public StudentDTO ToDTO(Student entity) {
            StudentDTO student = new StudentDTO();
            student.Id = entity.Id;
            student.Name = entity.Name;
            student.Address = entity.Address;
            student.Age = entity.Age;

            return student;
        }
    }
}
