using AutoMapper;
using Examination.DAL.Entities;
using Examination.PL.ModelViews;

namespace Examination.PL.Mapper
{
    public class GeneralMapperProfile:Profile
    {
        public GeneralMapperProfile()
        {
            CreateMap<UserMV,User>().ReverseMap();
            CreateMap<UserTypeMV, UserType>().ReverseMap();
        }

    }
}
