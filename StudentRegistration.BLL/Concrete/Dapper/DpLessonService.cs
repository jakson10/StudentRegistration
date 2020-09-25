using Dapper;
using Microsoft.Extensions.Configuration;
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
    public class DpLessonService : ILessonService
    {
        private readonly IConfiguration _configration;
        public DpLessonService(IConfiguration configuration)
        {
            _configration = configuration;
        }

        public async Task<bool> AddAsync(Lesson lesson)
        {
            var sql = "INSERT INTO [dbo].[Lesson] ([FkGradeId] , [LessonName]) VALUES (@FkGradeId , @LessonName)";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, lesson);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Eklenirken hata oluştu" + ex.Message.ToString());
            }

        }

        public async Task<bool> UpdateAsync(Lesson lesson)
        {
            var sql = "UPDATE [dbo].[Lesson] SET [FkGradeId] = @FkGradeId, [LessonName] = @LessonName WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, lesson);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Güncellenirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM [dbo].[Lesson] WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, new { Id = id });
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Ders silinirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<Lesson> GetAsync(int id)
        {
            var sql = "SELECT * FROM [dbo].[Lesson] WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var lesson = await connection.QueryAsync<Lesson>(sql, new { Id = id });
                return lesson.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Ders getirilirken hata oluştu" + ex.Message.ToString());
            }
        }


        public async Task<bool> AreThereLesson(string lessonName, int gradeId)
        {
            var sql = "SELECT * FROM Lesson WHERE LOWER(LessonName) = @LessonName AND FkGradeId = @FkGradeId ";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var lesson = await connection.QueryAsync<Lesson>(sql, new { LessonName = lessonName, FkGradeId = gradeId });
                if (lesson.FirstOrDefault() != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Ders getirilirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<IEnumerable<LessonJoinGradeModel>> GetAllAsync()
        {
            var sql = @"SELECT Lesson.Id, Lesson.LessonName, Lesson.FkGradeId, Grade.GradeName FROM Lesson INNER JOIN Grade on Lesson.FkGradeId = Grade.Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var list = await connection.QueryAsync<LessonJoinGradeModel>(sql);
                return list;
            }
            catch (Exception ex)
            {
                throw new Exception("Liste oluşturken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<IEnumerable<Lesson>> GetAllByGradeIdAsync(int gradeId)
        {
            var sql = @"SELECT * FROM Lesson Where FkGradeId = @FkGradeId";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                return await connection.QueryAsync<Lesson>(sql, new { FkGradeId = gradeId });

            }
            catch (Exception ex)
            {
                throw new Exception("Liste oluşturken hata oluştu" + ex.Message.ToString());
            }
        }
    }
}
