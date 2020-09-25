using Microsoft.Extensions.DependencyInjection;
using StudentRegistration.BLL.Abstract;
using StudentRegistration.BLL.Concrete.Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudentRegistration.BLL.DiContainer
{
    public  static class CustomExtensions
    {
        public static void AddContainerWithDependencies(this IServiceCollection services)
        {
            services.AddTransient<IGradeService, DpGradeService>();
            services.AddTransient<ILessonService, DpLessonService>();
            services.AddTransient<IStudenService, DpStudentService>();
        }
    }
}
