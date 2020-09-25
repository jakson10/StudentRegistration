using Microsoft.AspNetCore.Razor.TagHelpers;
using StudentRegistration.BLL.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.WebUI.TagHelpers
{
    [HtmlTargetElement("getstudentimage")]
    public class StudentImageTagHelper : TagHelper
    {
        private readonly IStudenService _studenService;
        public StudentImageTagHelper(IStudenService studenService)
        {
            _studenService = studenService;
        }
        public int Id { get; set; }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var blob = await _studenService.GetStudentImageById(Id);
            string html = string.Empty;

            html = $"<img src='{blob}' class='card-img-top'/>";

            output.Content.SetHtmlContent(html);
        }
    }
}
