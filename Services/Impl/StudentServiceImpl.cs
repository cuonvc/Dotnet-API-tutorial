﻿using Demo.Converter;
using Demo.DTOs;
using Demo.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Services.Impl {
    public class StudentServiceImpl : StudentService {

        private readonly DataContext dataContext;

        private readonly StudentConverter studentConverter;

        // private readonly ResponseObject<StudentDTO> responseObject;
        //
        // private readonly ResponseObject<List<StudentDTO>> responseObjectList;

        public StudentServiceImpl (DataContext dataContext, StudentConverter studentConverter) {
            this.dataContext = dataContext;
            this.studentConverter = studentConverter;
        }

        public ResponseObject<StudentDTO> create (StudentDTO studentDTO) {

            Student student = studentConverter.ToEntity(studentDTO);
            dataContext.Students.Add(student);
            dataContext.SaveChanges();

            ResponseObject<StudentDTO> responseObject = new ResponseObject<StudentDTO>();

            return responseObject.responseSuccess("Created student successfully", studentConverter.ToDTO(student));
        }

        public ResponseObject<StudentDTO> getById(int id) {

            ResponseObject<StudentDTO> responseObject = new ResponseObject<StudentDTO>();

            Student student = dataContext.Students.Find(id);
            if (student == null) {
                return responseObject.responseError("Student not found with id: " + id, StatusCodes.Status400BadRequest.ToString(), null);
            }

            return responseObject.responseSuccess("Success", studentConverter.ToDTO(student));
        }

        public ResponseObject<List<StudentDTO>> getAllStudents(int pageSize, int pageNo, string sortBy) {

            List<StudentDTO> datas = dataContext.Set<Student>()
                //tạm thời để mặc định sort by name
                .OrderBy(student => student.Name)
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
