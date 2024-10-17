using AutoMapper;
using LMS_LibraryManagementSystem_.Data;
using LMS_LibraryManagementSystem_.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS_LibraryManagementSystem_.Controllers
{
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public RolesController(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();

            /* by using Select AutoMapper:
               List<RoleVM> => we want to convert the source to a list of RoleVM.
               roles => is the list we are converting from (the source).*/
            // var rolevm = mapper.Map<List<RoleVM>>(roles);

            /* by using Select: */
            var rolevm = roles.Select(role => new RoleVM
            {
                Id = role.Id,
                Name = role.Name  
             /* When you say Name = role.Name, you are copying the role name value 
                from the database (role.Name) to the Name property of the RoleVM 
                object that will be passed to the view page.*/
            }).ToList();
            return View(rolevm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RoleVM rolevm)
        {
            if (!ModelState.IsValid)
            {
                return View(rolevm);
            }
            var existingRole = await roleManager.FindByNameAsync(rolevm.Name);
            if (existingRole != null)
            {
                ModelState.AddModelError(string.Empty, "This role already exists."); 
                return View(rolevm); 
            }
            var result = await roleManager.CreateAsync(new IdentityRole(rolevm.Name));
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Role has been created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(rolevm);
            }
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index"); 
                }
            }
            return NotFound();
        }

    }
}
