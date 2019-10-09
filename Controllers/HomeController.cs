using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PayMeApp.Models;

namespace PayMeApp.Controllers
{
    public class HomeController : Controller
    {
        private PayMeContext DbContext;
        public HomeController(PayMeContext context)
        {
            DbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet("Sign-Up")]
        public IActionResult SignUp()
        {
            return View();
        } 
        [HttpGet("Success")]
        public IActionResult Success()
        {
            return View();
        } 

        [HttpPost("Register")]
        public IActionResult Register(User user)
        {
            System.Console.WriteLine(user.Email);
            System.Console.WriteLine(user.Password);
            System.Console.WriteLine(ModelState.IsValid);
            if(ModelState.IsValid)
            {


                if(DbContext.Users.Any(u=> u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("SignUP");
                }
                else
                {

                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    user.Password = Hasher.HashPassword(user,user.Password);

                    DbContext.Add(user);
                    HttpContext.Session.SetString("Username", user.FirstName);
                    user.CreatedAt = DateTime.Now;
                    user.UpdatedAt = DateTime.Now;
                    user.Balance = 0;
                    DbContext.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
            }

            else
            {
                return View("SignUp");
            }
        }


        [HttpPost("Log-In")]
        public IActionResult LogIn(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                User userInDb = DbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
                if(userInDb == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }

                else
                {
                PasswordHasher<LoginUser>  hasher = new PasswordHasher<LoginUser>();

                PasswordVerificationResult result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);

                if(result == 0)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                    HttpContext.Session.SetString("Username", userInDb.FirstName);
                    HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Login");
            }
        }








        [HttpGet("DashBoard")]
        public IActionResult DashBoard()
        {
            string currentUserName = HttpContext.Session.GetString("Username");
            int? currentUserId= HttpContext.Session.GetInt32("UserId");

            if(currentUserName == null)
            {
                return RedirectToAction ("Index");
            }
            DashboardViewModel model = new DashboardViewModel()
            {
                Transactions = DbContext.Transactions.ToList(),
                User = DbContext.Users.Where(u=>u.UserId == (int)currentUserId).FirstOrDefault(),
            };
            return View(model);
        }

        
        [HttpPost("PayUp")]
        public IActionResult PayUp(DashboardViewModel PayingUser)
        {
            System.Console.WriteLine("Made it to get paid**********************************************************************************************************************************************************************************************************************************************************");
            int? currentUserId= HttpContext.Session.GetInt32("UserId");
         
            PayingUser.User = DbContext.Users.Where(u=>u.UserId == (int)currentUserId).FirstOrDefault();
            
            PayingUser.Transaction.Amount = PayingUser.Transaction.Amount;
            PayingUser.User.Balance -= PayingUser.Transaction.Amount;
            PayingUser.User.UsersTransactions.Add(PayingUser.Transaction.Amount);
            DbContext.SaveChanges();
         
            return RedirectToAction("DashBoard",PayingUser);
        }

        [HttpPost("GetPayd")]
        public IActionResult GetPayd(DashboardViewModel gettingPaydUser)
        {
            System.Console.WriteLine("Made it to get paid**********************************************************************************************************************************************************************************************************************************************************");
            int? currentUserId= HttpContext.Session.GetInt32("UserId");
         
            gettingPaydUser.User = DbContext.Users.Where(u=>u.UserId == (int)currentUserId).FirstOrDefault();

            gettingPaydUser.Transaction.Amount = gettingPaydUser.Transaction.Amount;
            gettingPaydUser.User.Balance += gettingPaydUser.Transaction.Amount;
            DbContext.Add(gettingPaydUser);
            DbContext.SaveChanges();
            return RedirectToAction("DashBoard",gettingPaydUser);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Login");
        }

















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
