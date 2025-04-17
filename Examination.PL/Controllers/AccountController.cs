
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
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["Title"] = "Login";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public IActionResult Login(AccountLoginMV model)
        {
            UserMV userMV = _userService.GetUserByEmail(model.Email);
            ResponseMV responseMV=new ResponseMV();
            string redirectUrl = "";
            if (userMV != null)
            {
                if (PasswordHelper.VerifyPassword(model.Password, userMV.Password))
                {

                    var userType = userMV.UserTypes.FirstOrDefault();
                    if (userType != null) {
                        var claims = new List<Claim>
                         {
                            new Claim("UserId", userMV.Id.ToString()),
                            new Claim(ClaimTypes.Email, userMV.Email),
                            new Claim(ClaimTypes.Name,  userMV.FirstName+" "+ userMV.LastName),
                            new Claim("UserType", userType.TypeName)
                         };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principle= new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle).Wait();

                        if(model.ReturnUrl != null)
                        {
                            redirectUrl = model.ReturnUrl;  
                        }
                        else
                        {

                             redirectUrl = userType.TypeName switch
                            {
                                Constants.UserTypes.Student => "/Student/Exam/Previous",
                                Constants.UserTypes.Admin => "/Admin/Student/Index",
                                Constants.UserTypes.Instructor => "/Instructor/Student/Index",

                            };

                        }
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

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).Wait();
            return RedirectToAction("Login", "Account");
        }
    }
}
