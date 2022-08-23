using Identity_IndvidualAccount.Data;
using Identity_IndvidualAccount.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Identity_IndvidualAccount.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        UserManager<IdentityUser> userManger;
        RoleManager<IdentityRole> roleManger;
        ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger,UserManager<IdentityUser>user, RoleManager<IdentityRole> role, ApplicationDbContext identity )
        {
            _logger = logger;
            userManger = user;
            roleManger = role;
            db = identity;

        }

        public async Task<IActionResult> Index()
        {
            
            await roleManger.CreateAsync(new IdentityRole { Name = "Admin" });
            await roleManger.CreateAsync(new IdentityRole { Name = "SuperAdmin" });
            await roleManger.CreateAsync(new IdentityRole { Name = "reporter" });
            await roleManger.CreateAsync(new IdentityRole { Name = "sales manager" });


            return View();
        }
        public async Task<IActionResult> addRoll(RoelVm model)
        {
            await roleManger.CreateAsync(new IdentityRole { Name = model.roleName });
            return RedirectToAction("index");
        }
        public async Task <IActionResult> UserRole()
        {
            var users = userManger.Users.ToList();
            List<UserRolesVM> res = new List<UserRolesVM>();
            foreach (var item in users)
            {
                
               var roles = await userManger.GetRolesAsync(item);
                res.Add(new UserRolesVM { user = item, userRoles = (List<string>)roles });
            }
            ViewBag.allRoles = roleManger.Roles.ToList();
            return View(res);
        }
        public IActionResult users()
        {
            var users = userManger.Users.ToList();
          
            return View(users);
        }
        public IActionResult roles()
        {
            var role = roleManger.Roles.ToList();
            return View(role);
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles= "Supermadmin")]
        public IActionResult Sales()
        {
            return View();
        }
        public async Task <IActionResult> addroletouser(string userid, string rolename)
        {
            var user = await userManger.FindByIdAsync(userid);
            var res = await userManger.AddToRoleAsync(user, rolename);
            if (!res.Succeeded)
            {
                await userManger.RemoveFromRoleAsync(user, rolename); 
            }
            return RedirectToAction("UserRole");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
