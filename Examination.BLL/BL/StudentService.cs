using AutoMapper;
using Examination.BLL.IBL;
using Examination.BLL.ModelViews;
using Examination.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Examination.BLL.BL
{
    public class StudentService : IStudentService
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
                var std = _mapper.Map<Examination.DAL.Entities.Student>(student);
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
