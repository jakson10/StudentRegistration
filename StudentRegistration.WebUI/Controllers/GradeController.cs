using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentRegistration.BLL.Abstract;
using StudentRegistration.Core.Entities;
using StudentRegistration.WebUI.Dtos;

namespace StudentRegistration.WebUI.Controllers
{
    public class GradeController : Controller
    {
        private readonly IGradeService _gradeService;
        private readonly IMapper _mapper;
        public GradeController(IGradeService gradeService, IMapper mapper)
        {
            _gradeService = gradeService;
            _mapper = mapper;
        }

        public async Task<IActionResult> GetAll()
        {
            return View(_mapper.Map<IEnumerable<GradeDto>>(await _gradeService.GetAllAsync()));
        }

        public IActionResult Create()
        {
            return View(new GradeDto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(GradeDto gradeDto)
        {
            if (await _gradeService.GetGradeByNameAsync(gradeDto.GradeName.ToLower()) == false)
            {
                await _gradeService.AddAsync(_mapper.Map<Grade>(gradeDto));
                ViewBag.ErrorMessage = "";
                return RedirectToAction("GetAll", "Grade");
            }
            ViewBag.ErrorMessage = "Sistemde aynı isme ait sınıf bulunmaktadır...!";
            return View(gradeDto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _gradeService.DeleteAsync(id);
            return RedirectToAction("GetAll", "Grade");
        }

        public async Task<IActionResult> Edit(int id)
        {
            return View(_mapper.Map<GradeDto>(await _gradeService.GetAsync(id)));
        }


        [HttpPost]
        public async Task<IActionResult> Edit(GradeDto gradeDto)
        {
            if (await _gradeService.GetGradeByNameAsync(gradeDto.GradeName.ToLower()) == false)
            {
                await _gradeService.UpdateAsync(_mapper.Map<Grade>(gradeDto));
                ViewBag.ErrorMessage = "";
                return RedirectToAction("GetAll", "Grade");
            }
            ViewBag.ErrorMessage = "Sistemde aynı isme ait sınıf bulunmaktadır...!";
            return View(gradeDto);
        }

        [HttpGet]
        public async Task<JsonResult> GetAllGrades()
        {
            var grades = await _gradeService.GetAllAsync();
            var gradesList = new SelectList(grades, "Id", "GradeName");
            return Json(gradesList);
        }

    }
}
