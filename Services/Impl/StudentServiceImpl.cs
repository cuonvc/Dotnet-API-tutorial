using Demo.Converter;
using Demo.DTOs;

namespace Demo.Services.Impl {
    public class StudentServiceImpl : StudentService {

        private readonly DataContext dataContext;

        private readonly StudentConverter studentConverter;

        public StudentServiceImpl (DataContext dataContext, StudentConverter studentConverter) {
            this.dataContext = dataContext;
            this.studentConverter = studentConverter;
        }

        public StudentDTO create (StudentDTO studentDTO) {
            Student student = studentConverter.ToEntity(studentDTO);
            dataContext.Students.Add(student);
            dataContext.SaveChanges();

            return studentConverter.ToDTO(student);
        }
    }
}
