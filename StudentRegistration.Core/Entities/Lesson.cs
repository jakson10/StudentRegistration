using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistration.Core.Entities
{
    public class Lesson
    {
        public int Id { get; set; }
        public int FkGradeId { get; set; }
        public string LessonName { get; set; }

        public virtual Grade Grade { get; set; }
    }
}
