using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Examination.BLL.ModelViews;
using Examination.DAL.Entities;

namespace Examination.BLL.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Student, StudentMV>().ReverseMap();
            CreateMap<User, UserMV>().ReverseMap();
            CreateMap<UserType, UserTypeMV>().ReverseMap();

        }
    }
}
