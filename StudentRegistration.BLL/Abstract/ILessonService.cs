using StudentRegistration.Core.Entities;
using StudentRegistration.Core.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.BLL.Abstract
{
    public interface ILessonService
    {
        Task<bool> AddAsync(Lesson lesson);
        Task<bool> UpdateAsync(Lesson lesson);
        Task<bool> DeleteAsync(int id);
        Task<Lesson> GetAsync(int id);
        Task<bool> AreThereLesson(string lessonName, int gradeId);
        Task<IEnumerable<LessonJoinGradeModel>> GetAllAsync();

        Task<IEnumerable<Lesson>> GetAllByGradeIdAsync(int gradeId);
    }
}
