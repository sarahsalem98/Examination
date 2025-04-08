
using Examination.PL.General;
using Examination.PL.IBL;
using Examination.PL.ModelViews;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Examination.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;

        }
        public IActionResult Login()
        {
            ViewData["Title"] = "Login";
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginMV model)
        {
            UserMV userMV = _userService.GetUserByEmail(model.Email);
            ResponseMV responseMV=new ResponseMV();
            if (userMV != null)
            {
                if (PasswordHelper.VerifyPassword(model.Password, userMV.Password))
                {

                    var userType = userMV.UserTypes.FirstOrDefault();
                    if (userType != null) {
                        var claims = new List<Claim>
                         {
                            new Claim(ClaimTypes.NameIdentifier, userMV.Id.ToString()),
                            new Claim(ClaimTypes.Email, userMV.Email),
                            new Claim(ClaimTypes.Name, userMV.FirstName),
                            new Claim("UserType", userType.TypeName)
                         };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principle= new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle).Wait();

                        string redirectUrl= userType.TypeName switch
                        {
                            Constants.UserTypes.Student => "/Student/Exam/Previous",
                            
                        };

                        responseMV =new ResponseMV
                        {
                            Success = true,
                            Message = "Login successful",
                            RedirectUrl = redirectUrl

                        };
                    }
                }
                else
                {

                    responseMV = new ResponseMV
                    {
                        Success = false,
                        Message = "Invalid password or email",
                        RedirectUrl = null

                    };
                }



            }
            else
            {

                responseMV = new ResponseMV
                {
                    Success = false,
                    Message = "user is not Exist",
                    RedirectUrl = null

                };
            }
           

            return Json(responseMV);

        }
    }
}
