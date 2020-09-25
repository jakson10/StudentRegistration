using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.WebUI.Dtos
{
    public class GradeDto
    {
        public int Id { get; set; }
        public string GradeName { get; set; }

        public virtual ICollection<StudentDto> Students { get; set; }

        public virtual ICollection<LessonDto> Lessons { get; set; }
    }
}
