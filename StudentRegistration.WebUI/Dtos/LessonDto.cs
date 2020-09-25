using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.WebUI.Dtos
{
    public class LessonDto
    {
        public int Id { get; set; }
        public int FkGradeId { get; set; }
        public string LessonName { get; set; }

        public virtual GradeDto Grade { get; set; }
    }
}
