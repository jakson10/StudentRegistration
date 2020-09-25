using StudentRegistration.Core.Entities;
using StudentRegistration.Core.QueryModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.BLL.Abstract
{
    public interface IStudenService
    {
        Task<bool> AddAsync(Student student);
        Task<bool> UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
        Task<StudentJoinGradeModel> GetAsync(int id);
        Task<StudentJoinGradeModel> GetStudentByNameAsync(string studentName);
        Task<IEnumerable<StudentJoinGradeModel>> GetAllAsync();
        Task<string> GetStudentImageById(int id);

        Task<bool> AreThereStudent(string studentName, string studentLastName);

        Task<IEnumerable<SearchModel>> GetSearchModelAsync(string s);
    }
}
