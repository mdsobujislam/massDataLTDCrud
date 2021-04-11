using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Authentication.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Authentication.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole( string userrole)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(userrole))
            {
                if(await _roleManager.RoleExistsAsync(userrole))
                {
                    msg = "Role ["+userrole+" already exist]";
                }
                else
                {
                    IdentityRole r = new IdentityRole(userrole);
                    await _roleManager.CreateAsync(r);
                    msg = "Role ["+userrole+ "] has been created successfull !!";
                }
            }
            else
            {
                msg = "Please Enter a Valid Role Name";
            }
            ViewBag.msg = msg;
            return View("Index");
        }
        public IActionResult AssignRole()
        {
            ViewBag.users = _userManager.Users;
            ViewBag.roles = _roleManager.Roles;
            ViewBag.msg = TempData["msg"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userdata, string roledata)
        {
            string msg = "";
            if (!string.IsNullOrEmpty(userdata) && !string.IsNullOrEmpty(roledata))
            {
                ApplicationUser u = await _userManager.FindByEmailAsync(userdata);
                if (u!=null)
                {
                    if(await _roleManager.RoleExistsAsync(roledata))
                    {
                        await _userManager.AddToRoleAsync(u, roledata);
                        msg = "Role has been Assign to User";
                    }
                    else
                    {
                        msg = "Role is not found";
                    }
                }
                else
                {
                    msg = "User is not found";
                }

            }
            else
            {
                msg = "Please select valid user and role";
            }
            TempData["msg"]= msg;
            return RedirectToAction("AssignRole");
        }
    }
}