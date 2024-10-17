using AutoMapper;
using LMS_LibraryManagementSystem_.Data;
using LMS_LibraryManagementSystem_.Models;
using LMS_LibraryManagementSystem_.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LMS_LibraryManagementSystem_.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;

        public UsersController(ApplicationDbContext context,RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.mapper = mapper;
        }


        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();

            /* by using Select AutoMapper:*/
            //var userViewModels = mapper.Map<List<ApplicationUserVM>>(users);

            var userViewModels = new List<ApplicationUserVM>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                var userViewModel = new ApplicationUserVM
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Address = user.Address,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles.ToList(),
                };
                userViewModels.Add(userViewModel);
            }
            return View(userViewModels);
        }



          [HttpGet]
          [Authorize]
          public async Task<IActionResult> Create()
          {
              var roles = await roleManager.Roles.ToListAsync();
              var viewModel = new ApplicationUserCreateVM
              {
                  Roles = roles.Select(role => new SelectListItem
                  {
                      Value = role.Name,
                      Text = role.Name
                  }).ToList()
              };
              return View(viewModel);
          }



          [HttpPost]
          public async Task<IActionResult> Create(ApplicationUserCreateVM uservm)
          {
              if (!ModelState.IsValid)
              {
                  return View(uservm);
              } 
              var user = new ApplicationUser
              {
                  UserName = uservm.UserName,
                  Email = uservm.Email,
                  Address = uservm.Address,
                  PhoneNumber = uservm.PhoneNumber
              };
              var result = await userManager.CreateAsync(user,uservm.PasswordHash);
              if (!result.Succeeded)
              {
                 return View(uservm);
              }
            foreach (var role in uservm.SelectedRole)
            {
                await userManager.AddToRoleAsync(user, role);
            }
            //await userManager.AddToRoleAsync(user, uservm.SelectedRole);
            user.EmailConfirmed = true;
            var result1 = await userManager.UpdateAsync(user);
            await context.SaveChangesAsync();
            return RedirectToAction("Index");

          }

    }

}
