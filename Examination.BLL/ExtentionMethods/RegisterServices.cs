using Examination.BLL.BL;
using Examination.BLL.IBL;
using Examination.BLL.Mapper;
using Examination.DAL.Repos;
using Examination.DAL.Repos.IRepos;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.BLL.ExtentionMethods
{
   public static class RegisterServicesExtention
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IStudentService, StudentService>();
           // services.AddScoped<IUserService,UserService>();  
            services.AddAutoMapper(typeof(MapperProfile));
            return services;
        }

    }
}
