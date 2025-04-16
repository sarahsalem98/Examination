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
            CreateMap<ExamMV, Exam>().ReverseMap();
            CreateMap<CourseMV,Course>().ReverseMap();
            CreateMap<InstructorMV, Instructor>().ReverseMap();
            CreateMap<InstructorCourseMV, InstructorCourse>().ReverseMap();
          //  CreateMap<CourseDepartmentMV,CourseDepartment>().ReverseMap();
          
            CreateMap<StudentMV, Student>().ReverseMap();
            CreateMap<ExamMV, Exam>().ReverseMap();
            CreateMap<CourseMV,Course>().ReverseMap();
            CreateMap<InstructorMV, Instructor>().ReverseMap();
            CreateMap<InstructorCourseMV, InstructorCourse>().ReverseMap();
           
            CreateMap<DepartmentMV, Department>().ReverseMap();



        }

    }
}
