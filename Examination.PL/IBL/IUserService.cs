

using Examination.PL.ModelViews;

namespace Examination.PL.IBL
{
    public interface IUserService
    {
       public UserMV GetUserByEmail(string Email);
    }
}
