using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistration.Core.QueryModel
{
    public class SearchModel
    {
        public int Id { get; set; }
        public int FkGradeId { get; set; }
        public string LessonName { get; set; }
        public string GradeName { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
    }
}
