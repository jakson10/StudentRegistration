using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistration.Core.QueryModel
{
    public class StudentJoinGradeModel
    {
        public int Id { get; set; }
        public int FkGradeId { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string PhotoPath { get; set; }

        public string GradeName { get; set; }

    }
}
