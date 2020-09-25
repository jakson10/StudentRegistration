using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistration.Core.Entities
{
    public class Grade
    {
        public int Id { get; set; }
        public string GradeName { get; set; }

        public virtual ICollection<Student> Students { get; set; }

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
