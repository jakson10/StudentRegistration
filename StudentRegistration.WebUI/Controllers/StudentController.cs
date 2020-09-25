using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistration.BLL.Abstract;
using StudentRegistration.Core.Entities;
using StudentRegistration.Core.QueryModel;
using StudentRegistration.WebUI.Dtos;
using StudentRegistration.WebUI.Models;

namespace StudentRegistration.WebUI.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudenService _studenService;
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        public StudentController(IStudenService studenService, ILessonService lessonService, IMapper mapper)
        {
            _studenService = studenService;
            _mapper = mapper;
            _lessonService = lessonService;
        }
        public async Task<IActionResult> GetAll(string s, int activePage = 1)
        {
            int totalPage = 0;
            ViewBag.ActivePage = activePage;
            var getStudents = await _studenService.GetAllAsync();
            if (s == null)
            {
                var students = StudentPage(out totalPage, activePage, getStudents);
                ViewBag.TotalPage = totalPage;
                return View(students);
            }
            else
            {
                var studensId = SearchWord(out totalPage, activePage, s, getStudents.ToList());
                ViewBag.SearchedWord = s;
                ViewBag.TotalPage = totalPage;
                return View(studensId);
            }
        }

        public IActionResult Create()
        {
            return View(new StudenAddDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudenAddDto model)
        {
            if (await _studenService.AreThereStudent(model.StudentName.ToLower(), model.StudentLastName.ToLower()) == false)
            {
                var uploadModel = await UploadFileAsync(model.Image, "image/jpeg");
                if (uploadModel.UploadState == UploadState.Success)
                {
                    model.PhotoPath = uploadModel.NewName;
                    await _studenService.AddAsync(_mapper.Map<Student>(model));
                }
                else if (uploadModel.UploadState == UploadState.NotExist)
                {
                    model.PhotoPath = "default.jpg";
                    await _studenService.AddAsync(_mapper.Map<Student>(model));
                }
                else
                {
                    ViewBag.ErrorMessage = uploadModel.ErrorMessage;
                    return View(model);
                }
                ViewBag.ErrorMessage = "";
                return RedirectToAction("GetAll", "Student");
            }

            ViewBag.ErrorMessage = "Sistemde aynı sınıfa ait öğrenci bulunmaktadır...!";
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var student = await _studenService.GetAsync(id);
            return View(new StudentUpdateDto
            {
                Id = student.Id,
                StudentName = student.StudentName,
                StudentLastName = student.StudentLastName,
                FkGradeId = student.FkGradeId,
                PhotoPath = student.PhotoPath
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(StudentUpdateDto model)
        {
            if (await _studenService.AreThereStudent(model.StudentName.ToLower(), model.StudentLastName.ToLower()) == false)
            {
                var uploadModel = await UploadFileAsync(model.Image, "image/jpeg");
                if (uploadModel.UploadState == UploadState.Success)
                {
                    var updateStudent = await _studenService.GetAsync(model.Id);
                    updateStudent.StudentName = model.StudentName;
                    updateStudent.StudentLastName = model.StudentLastName;
                    updateStudent.FkGradeId = model.FkGradeId;
                    updateStudent.PhotoPath = uploadModel.NewName;

                    await _studenService.UpdateAsync(_mapper.Map<Student>(updateStudent));
                }
                else if (uploadModel.UploadState == UploadState.NotExist)
                {
                    var updateStudent = await _studenService.GetAsync(model.Id);
                    updateStudent.StudentName = model.StudentName;
                    updateStudent.StudentLastName = model.StudentLastName;
                    updateStudent.FkGradeId = model.FkGradeId;

                    await _studenService.UpdateAsync(_mapper.Map<Student>(updateStudent));
                }
                else
                {
                    ViewBag.ErrorMessage = uploadModel.ErrorMessage;
                    return View(model);
                }
                ViewBag.ErrorMessage = "";
                return RedirectToAction("GetAll", "Student");
            }
            ViewBag.ErrorMessage = "Sistemde aynı sınıfa ait öğrenci bulunmaktadır...!";
            return View(model);

        }

        public async Task<IActionResult> Delete(int id)
        {
            await _studenService.DeleteAsync(id);
            return RedirectToAction("GetAll", "Student");
        }

        [HttpGet]
        public async Task<JsonResult> ViewLessons(int gradeId)
        {
            var lessons = _mapper.Map<IEnumerable<LessonDto>>(await _lessonService.GetAllByGradeIdAsync(gradeId));
            var gradesList = new SelectList(lessons, "Id", "LessonName");
            return Json(gradesList);
        }

        private IEnumerable<StudentJoinGradeModel> StudentPage(out int totalPage, int activePage, IEnumerable<StudentJoinGradeModel> models)
        {
            totalPage = (int)Math.Ceiling((double)models.Count() / 8);
            models = models.Skip((activePage - 1) * 8).Take(8);
            return models;
        }
        private IEnumerable<StudentJoinGradeModel> SearchWord(out int totalPage, int activePage, string s, List<StudentJoinGradeModel> models)
        {
            var queryList = _studenService.GetSearchModelAsync(s).Result;
            var studentsId = new List<int>();
            foreach (var item in queryList)
            {
                studentsId.Add(item.Id);
            }
            studentsId = studentsId.Distinct().ToList();
            var studentsBySearchResult = new List<StudentJoinGradeModel>();
            for (int i = 0; i < studentsId.Count(); i++)
            {
                var result = (from item in models
                              where item.Id == studentsId[i]
                              select item).FirstOrDefault();
                studentsBySearchResult.Add(result);
            }

            var studentsBySearchResultConvert = studentsBySearchResult.AsEnumerable();
            totalPage = (int)Math.Ceiling((double)studentsBySearchResultConvert.Count() / 8);
            studentsBySearchResultConvert = studentsBySearchResultConvert.Skip((activePage - 1) * 8).Take(8);
            return studentsBySearchResultConvert;
        }

    }
}
