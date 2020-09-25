using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistration.BLL.Abstract;
using StudentRegistration.Core.Entities;
using StudentRegistration.Core.QueryModel;
using StudentRegistration.WebUI.Dtos;

namespace StudentRegistration.WebUI.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;
        private readonly IMapper _mapper;
        public LessonController(ILessonService lessonService, IMapper mapper)
        {
            _lessonService = lessonService;
            _mapper = mapper;
        }


        public IActionResult Create()
        {
            return View(new LessonDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(LessonDto lessonDto)
        {
            if (await _lessonService.AreThereLesson(lessonDto.LessonName.ToLower(), lessonDto.FkGradeId) == false)
            {
                await _lessonService.AddAsync(_mapper.Map<Lesson>(lessonDto));
                ViewBag.ErrorMessage = "";
                return RedirectToAction("GetAll", "Lesson");
            }
            ViewBag.ErrorMessage = "Sistemde aynı isme ait ders bulunmaktadır...!";
            return View(lessonDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _lessonService.DeleteAsync(id);
            return RedirectToAction("GetAll", "Lesson");
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(_mapper.Map<LessonDto>(await _lessonService.GetAsync(id)));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(LessonDto lessonDto)
        {
            if (await _lessonService.AreThereLesson(lessonDto.LessonName.ToLower(), lessonDto.FkGradeId) == false)
            {
                await _lessonService.UpdateAsync(_mapper.Map<Lesson>(lessonDto));
                ViewBag.ErrorMessage = "";
                return RedirectToAction("GetAll", "Lesson");
            }
            ViewBag.ErrorMessage = "Sistemde aynı isme ait ders bulunmaktadır...!";
            return View(lessonDto);
        }

        public async Task<IActionResult> GetAll(int activePage = 1)
        {
            ViewBag.ActivePage = activePage;
            var lessons = _mapper.Map<IEnumerable<LessonJoinGradeModel>>(await _lessonService.GetAllAsync());
            int totalPage = (int)Math.Ceiling((double)lessons.Count() / 8);
            ViewBag.TotalPage = totalPage;
            lessons = lessons.Skip((activePage - 1) * 8).Take(8);
            return View(lessons);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllLessons(int id)
        {
            var lessons = await _lessonService.GetAllByGradeIdAsync(id);
            var lessonsList = new SelectList(lessons, "Id", "LessonName");
            return Json(lessonsList);
        }
    }
}
