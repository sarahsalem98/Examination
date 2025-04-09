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
            CreateMap<BranchMV, Branch>().ReverseMap(); 
            CreateMap<DepartmentMV, Department>().ReverseMap(); 
            CreateMap<DepartmentBranchMV, DepartmentBranch>().ReverseMap();
            CreateMap<StudentMV, Student>().ReverseMap();   
        }

    }
}
