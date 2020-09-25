using AutoMapper;
using StudentRegistration.Core.Entities;
using StudentRegistration.Core.QueryModel;
using StudentRegistration.WebUI.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.WebUI.AutoMapper
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Student, StudenAddDto>();
            CreateMap<StudenAddDto, Student>();

            CreateMap<Student, StudentUpdateDto>();
            CreateMap<StudentUpdateDto, Student>();

            CreateMap<Student, StudentDto>();
            CreateMap<StudentDto, Student>();

            CreateMap<Grade, GradeDto>();
            CreateMap<GradeDto, Grade>();

            CreateMap<Lesson, LessonDto>();
            CreateMap<LessonDto, Lesson>();

            CreateMap<Lesson, LessonJoinGradeModel>();
            CreateMap<LessonJoinGradeModel, Lesson>();

            CreateMap<Student, StudentJoinGradeModel>();
            CreateMap<StudentJoinGradeModel, Student>();

        }
    }
}
