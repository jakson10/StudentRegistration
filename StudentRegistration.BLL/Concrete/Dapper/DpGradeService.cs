using Dapper;
using Microsoft.Extensions.Configuration;
using StudentRegistration.BLL.Abstract;
using StudentRegistration.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegistration.BLL.Concrete.Dapper
{
    public class DpGradeService : IGradeService
    {
        private readonly IConfiguration _configration;

        public DpGradeService(IConfiguration configuration)
        {
            _configration = configuration;
        }

        public async Task<bool> AddAsync(Grade grade)
        {
            var sql = "INSERT INTO [dbo].[Grade] ([GradeName]) VALUES (@GradeName)";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, grade);
                return true;
            }
            catch (Exception ex)
            {
                //sqlClose(connection);
                throw new Exception("Eklenirken hata oluştu" + ex.Message.ToString());
            }

        }

        public async Task<bool> UpdateAsync(Grade grade)
        {
            var sql = "UPDATE [dbo].[Grade] SET [GradeName] = @GradeName WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, grade);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Güncellenirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sql = "DELETE FROM [dbo].[Grade] WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                await connection.ExecuteAsync(sql, new { Id = id });
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Sınıf silinirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<Grade> GetAsync(int id)
        {
            var sql = "SELECT * FROM [dbo].[Grade] WHERE Id = @Id";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var grade = await connection.QueryAsync<Grade>(sql, new { Id = id });
                return grade.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Sınıf getirilirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<bool> GetGradeByNameAsync(string gradeName)
        {
            var sql = "SELECT * FROM [dbo].[Grade] WHERE LOWER(GradeName) = @GradeName";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                var grade = await connection.QueryAsync<Grade>(sql, new { GradeName = gradeName });
                if (grade.FirstOrDefault() != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Sınıf getirilirken hata oluştu" + ex.Message.ToString());
            }
        }

        public async Task<IEnumerable<Grade>> GetAllAsync()
        {
            var sql = "SELECT * FROM [dbo].[Grade]";
            using IDbConnection connection = new SqlConnection(_configration.GetConnectionString("DefaultConnection"));
            try
            {
                return await connection.QueryAsync<Grade>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception("Liste oluşturken hata oluştu" + ex.Message.ToString());
            }
        }


    }
}
