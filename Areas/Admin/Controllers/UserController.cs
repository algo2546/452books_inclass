using BooksSpring2024_sec02.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BooksSpring2024_sec02.Areas.Admin.Controllers
{


    [Area("admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private BooksDBContext _dbContext;

        private UserManager<IdentityUser> _userManager;

        public UserController(BooksDBContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;

            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<ApplicationUser> userList = _dbContext.ApplicationUsers.ToList();

            var allRoles = _dbContext.Roles.ToList();

            var userRoles = _dbContext.UserRoles.ToList();

            foreach (var user in userList)
            {
                var userRole = userRoles.Find(ur => ur.UserId == user.Id);
                if (userRole != null)
                {
                    var roleId = userRole.RoleId;
                    var role = allRoles.Find(r => r.Id == roleId);
                    if (role != null)
                    {
                        user.RoleName = role.Name;
                    }
                    else
                    {
                        user.RoleName = "Role not found"; 
                    }
                }
                else
                {
                    user.RoleName = "No role assigned";
                }
            }



            return View(userList);
        }


        public IActionResult LockUnlock(string id)
        {

            var userFromDb = _dbContext.ApplicationUsers.Find(id);

            if(userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
            {
                userFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                userFromDb.LockoutEnd = DateTime.Now.AddYears(10);
            }

            _dbContext.SaveChanges();

            return RedirectToAction("Index");

        }

        public IActionResult EditUserRole(string id)
        {
            var currentUserRole = _dbContext.UserRoles.FirstOrDefault(ur => ur.UserId == id);

            IEnumerable<SelectListItem> listOfRoles = _dbContext.Roles.ToList().Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });

            ViewBag.ListOfRoles = listOfRoles;

            ViewBag.UserInfo = _dbContext.ApplicationUsers.Find(id);

            return View(currentUserRole);

        }

        [HttpPost]
        public IActionResult EditUserRole(Microsoft.AspNetCore.Identity.IdentityUserRole<string> updatedRole)
        {

            ApplicationUser applicationUser = _dbContext.ApplicationUsers.Find(updatedRole.UserId);

            string newRoleName = _dbContext.Roles.Find(updatedRole.RoleId).Name;

            string oldRoleId = _dbContext.UserRoles.FirstOrDefault(u => u.UserId == applicationUser.Id).RoleId;

            string oldRoleName = _dbContext.Roles.Find(oldRoleId).Name;

            _userManager.RemoveFromRoleAsync(applicationUser, oldRoleName).GetAwaiter().GetResult();

            _userManager.AddToRoleAsync(applicationUser, newRoleName).GetAwaiter().GetResult();

            return RedirectToAction("Index");

        }



    }
}
