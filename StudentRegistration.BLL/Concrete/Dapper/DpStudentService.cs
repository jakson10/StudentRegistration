using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StudentRegistration.BLL.Abstract;
using StudentRegistration.Core.Entities;
using StudentRegistration.Core.QueryModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.BLL.Concrete.Dapper
{
    public class DpStudentService : IStudenService
    {
        private readonly IConfiguration _configration;
        private readonly ILogger<Student> _logger;
        public DpStudentService(IConfiguration configuration, ILogger<Student> logger)
        {
            _configration = configuration;
            _logger = logger;
        }

        public async Task<bool> AddAsync(Student student)
        {
            var sql = @"INSERT INTO [dbo].[Student] ([FkGradeId], [StudentName], [StudentLastName], [PhotoPath]) VALUES (@FkGradeId , @StudentName, @StudentLastName, @PhotoPath)";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, student);
                _logger.LogWarning($" '{DateTime.Now}' vakitte vaktinde nesnesi veritabanına başarıyla eklendi");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Eklenirken hata oluştu" + ex.Message.ToString());
            }

        }

        public async Task<bool> UpdateAsync(Student student)
        {
            var sql = "UPDATE [dbo].[Student] SET [FkGradeId] = @FkGradeId, [StudentName] = @StudentName, [StudentLastName] = @StudentLastName, [PhotoPath] = @PhotoPath WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, student);
                _logger.LogWarning($" '{DateTime.Now}' vaktinde öğrenci başarıyla güncellendi");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Güncellenirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM [dbo].[Student] WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, new { Id = id });
                _logger.LogWarning($" '{DateTime.Now}' vaktinde '{id}' numaralı öğrenci silindi");
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ders silinirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<StudentJoinGradeModel> GetAsync(int id)
        {
            var sql = @"SELECT Student.Id, Student.FkGradeId, Student.StudentName, Student.StudentLastName, Student.PhotoPath, Grade.GradeName" +
                " FROM Student INNER JOIN Grade on Student.FkGradeId = Grade.Id WHERE Student.Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var student = await connection.QueryAsync<StudentJoinGradeModel>(sql, new { Id = id });
                _logger.LogWarning($" '{DateTime.Now}' vaktinde '{id}' numaralı öğrenci getirme işlemi başarılı.");
                return student.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Ders getirilirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<StudentJoinGradeModel> GetStudentByNameAsync(string studentName)
        {
            var sql = @"SELECT Student.Id, Student.FkGradeId, Student.StudentName, Student.StudentLastName, Student.PhotoPath, Grade.GradeName" +
                   " FROM Student INNER JOIN Grade on Student.FkGradeId = Grade.Id WHERE Student.StudentName = @StudentName";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var student = await connection.QueryAsync<StudentJoinGradeModel>(sql, new { StudentName = studentName });
                _logger.LogWarning($" '{DateTime.Now}' vaktinde '{studentName}' isimlii öğrenci getirme işlemi başarılı.");
                return student.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Ders getirilirken hata oluştu" + ex.Message.ToString());
            }
        }
        public async Task<bool> AreThereStudent(string studentName, string studentLastName)
        {
            var sql = @"SELECT * FROM Student INNER JOIN Grade on Student.FkGradeId = Grade.Id WHERE LOWER(Student.StudentName) = @StudentName AND LOWER(Student.StudentLastName) = @StudentLastName";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var student = await connection.QueryAsync<StudentJoinGradeModel>(sql, new { StudentName = studentName, StudentLastName = studentLastName });
                if (student.FirstOrDefault() != null)
                {
                    _logger.LogWarning($" '{DateTime.Now}' vaktinde '{studentName} ' ' {studentLastName}' öğrencisi bulma işlemi başarılı.");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Ders getirilirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<IEnumerable<StudentJoinGradeModel>> GetAllAsync()
        {
            var sql = @"SELECT Student.Id, Student.FkGradeId, Student.StudentName, Student.StudentLastName, Student.PhotoPath, Grade.GradeName" +
                      " FROM Student INNER JOIN Grade on Student.FkGradeId = Grade.Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var list = await connection.QueryAsync<StudentJoinGradeModel>(sql);
                _logger.LogWarning($" '{DateTime.Now}' vaktinde öğrenci listesi getirme işlemi başarılı.");
                return (list);
            }
            catch (Exception ex)
            {
                throw new Exception("Liste oluşturken hata oluştu" + ex.Message.ToString());
            }
        }
        public async Task<string> GetStudentImageById(int id)
        {
            var student = await GetAsync(id);
            if (string.IsNullOrWhiteSpace(student.PhotoPath))
                return null;
            return string.Format($"/img/{student.PhotoPath}", "image/jpeg");
        }

        public async Task<IEnumerable<SearchModel>> GetSearchModelAsync(string s)
        {
            var sql = string.Format("SELECT c.Id, a.FkGradeId, a.LessonName, b.GradeName, c.StudentName, c.StudentLastName FROM Lesson as a inner join Grade as b on a.FkGradeId = b.Id " +
                        "inner join Student as c on b.Id = c.FkGradeId WHERE a.LessonName LIKE '%{0}%' OR b.GradeName LIKE '%{0}%' OR c.StudentName LIKE '%{0}%' " +
                        "OR c.StudentLastName LIKE '%{0}%'", s);
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var list = await connection.QueryAsync<SearchModel>(sql);
                _logger.LogWarning($" '{DateTime.Now}' vaktinde '{s}' kelimesine ait öğrenci listesi başarıyla getirildi.");
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Liste oluşturken hata oluştu" + ex.Message.ToString());
            }
        }

    }
}
