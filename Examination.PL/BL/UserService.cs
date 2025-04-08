
using AutoMapper;
using Examination.DAL.Repos.IRepos;
using Examination.PL.IBL;
using Examination.PL.ModelViews;

namespace Examination.PL.BL
{
    public class UserService:IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

        }
        public UserMV GetUserByEmail(string Email)
        {

            try
            {
                var data = _unitOfWork.UserRepo.FirstOrDefault(u => u.Email == Email, "UserTypes");
                if (data == null)
                {
                    return null;
                }
                return _mapper.Map<UserMV>(data);

            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "error occured whilr retreiving user email :{Email}", Email);
                return null;

            }
        }
    }
}
