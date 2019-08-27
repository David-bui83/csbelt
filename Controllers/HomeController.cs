using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using csbelt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace csbelt.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;
        public HomeController(MyContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("registor")]
        public IActionResult Register(User user)
        {
            if(ModelState.IsValid)
            {
                if(dbContext.Users.Any(u => u.UserName == user.UserName))
                {
                    ModelState.AddModelError("UserName","Username is already taken");
                    return View("Index");
                }
                else
                {
                    PasswordHasher<User> hasher = new PasswordHasher<User>();
                    user.Password = hasher.HashPassword(user,user.Password);
                    dbContext.Add(user);
                    dbContext.SaveChanges();

                    var userInDb = dbContext.Users.FirstOrDefault(u => u.UserName == user.UserName);

                    HttpContext.Session.GetInt32("id");
                    HttpContext.Session.SetInt32("id",userInDb.UserId);


                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LoginUser userSubmission)
        {
            if(ModelState.IsValid)
            {
                var userInDb = dbContext.Users.FirstOrDefault(u => u.UserName == userSubmission.LoginUserName);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginUserName","Invalid Username/Password");
                    return View("Index");
                }
                
                var hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.LoginPassword);
                if(result==0)
                {
                    ModelState.AddModelError("LoginUserName","Invalid Username/Password");
                    return View("Index");
                }
                else
                {
                    HttpContext.Session.GetInt32("id");
                    HttpContext.Session.SetInt32("id",userInDb.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            int? uid = HttpContext.Session.GetInt32("id");
            if(uid == null)
            {
                return RedirectToAction("Index");
            }

            List<User> userList = dbContext.Users.ToList();
            ViewBag.users = userList;

            List<Hobby> hobbyList = dbContext.Hobbies.Include(c => c.creator).Include(u => u.UserJoined).ThenInclude(u => u.User).ToList();

            return View("Dashboard",hobbyList);
        }

        [HttpGet("new/hobby")]
        public IActionResult NewHobby()
        {
            int? uid = HttpContext.Session.GetInt32("id");
            if(uid == null)
            {
                return RedirectToAction("Index");
            }
            return View("NewHobby");
        }

        [HttpPost("create/hobby")]
        public IActionResult CreateHobby(Hobby hobby)
        {
            int? uid = HttpContext.Session.GetInt32("id");
            if(ModelState.IsValid)
            {
                if(dbContext.Hobbies.Any(h => h.Name == hobby.Name))
                {
                    ModelState.AddModelError("Name","Hobby has already been created");
                    return View("NewHobby");
                }
                else
                {
                    hobby.UserId = (int)uid;
                    dbContext.Add(hobby);
                    dbContext.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
            }
            return View("NewHobby");
        }

        [HttpGet("display/{id}")]
        public IActionResult Display(int id)
        {
            int? uid = HttpContext.Session.GetInt32("id");
            if(uid == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.uid = (int)uid;

            List<Association> assList = dbContext.Associations.ToList();
            ViewBag.a = assList;

            Hobby hobby = dbContext.Hobbies.Include(c => c.creator).Include(u => u.UserJoined).ThenInclude(u => u.User).FirstOrDefault(h => h.HobbyId == id);
            ViewBag.h = hobby;
            return View("Display",hobby);
        }

        [HttpPost("join/{id}")]
        public IActionResult Join(Association addHobby, int id)
        {
            int? uid = HttpContext.Session.GetInt32("id");

            addHobby.HobbyId = id;
            addHobby.UserId = (int)uid;
            dbContext.Add(addHobby);

            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("edit/{id}")]
        public IActionResult EditHobby(int id)
        {
             int? uid = HttpContext.Session.GetInt32("id");
            if(uid == null)
            {
                return RedirectToAction("Index");
            }
            Hobby editHobby = dbContext.Hobbies.FirstOrDefault(h => h.HobbyId == id);
            return View("EditHobby", editHobby);
        }

        [HttpPost("edit/{id}")]
        public IActionResult UpdateHobby(Hobby editHob,int id)
        {
            Hobby hob = dbContext.Hobbies.FirstOrDefault(h => h.HobbyId == id);
            if(ModelState.IsValid)
            {
                hob.Name = editHob.Name;
                hob.Description = editHob.Description;
                dbContext.SaveChanges();
                return RedirectToAction("Display", new {id=hob.HobbyId});
            }
            return View("editHobby", editHob);
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
