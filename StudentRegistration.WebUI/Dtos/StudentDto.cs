﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.WebUI.Dtos
{
    public class StudentDto
    {
        public int Id { get; set; }
        public int FkGradeId { get; set; }
        public string StudentName { get; set; }
        public string StudentLastName { get; set; }
        public string PhotoPath { get; set; }

        public virtual GradeDto Grade { get; set; }
    }
}
