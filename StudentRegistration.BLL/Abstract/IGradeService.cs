using StudentRegistration.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.BLL.Abstract
{
    public interface IGradeService
    {
        Task<bool> AddAsync(Grade grade);
        Task<bool> UpdateAsync(Grade grade);
        Task<bool> DeleteAsync(int id);
        Task<Grade> GetAsync(int id);
        Task<bool> GetGradeByNameAsync(string gradeName);
        Task<IEnumerable<Grade>> GetAllAsync();
    }
}
