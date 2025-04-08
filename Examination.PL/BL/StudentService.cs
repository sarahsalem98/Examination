using AutoMapper;
using Examination.DAL.Entities;
using Examination.DAL.Repos.IRepos;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class StudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public int Add(StudentMV student)
        {
            int result = 0;
            try
            {
                var std = _mapper.Map<Student>(student);
                _unitOfWork.StudentRepo.Insert(std);
                result = _unitOfWork.Save();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
