using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistration.Core.QueryModel
{
    public class LessonJoinGradeModel
    {
        public int Id { get; set; }
        public int FkGradeId { get; set; }
        public string LessonName { get; set; }
        public string GradeName { get; set; }
    }

}
