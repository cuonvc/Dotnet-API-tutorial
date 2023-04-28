using Demo.Converter;
using Demo.DTOs;
using Microsoft.AspNetCore.Mvc;

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

        public StudentDTO getById(int id) {
            Student student = dataContext.Students.Find(id);
            if (student == null) {
                return null;
            }
            return studentConverter.ToDTO(student);
            
        }

        public List<StudentDTO> getAllStudents() {
            return dataContext.Students.ToList()
                .Select(student => studentConverter.ToDTO(student))
                .ToList();
        }

        public StudentDTO update(StudentDTO studentDTO) {

            Student student = dataContext.Students.Find(studentDTO.Id);
            if (student == null) {
                return null;
            }

            studentConverter.ToEntity(studentDTO, student);
            dataContext.SaveChanges();
            return studentConverter.ToDTO(student);

        }
    }
}
