using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.BLL.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Examination.DAL.Entities.Student, Examination.BLL.ModelViews.StudentMV>().ReverseMap();
          

        }
    }
}
