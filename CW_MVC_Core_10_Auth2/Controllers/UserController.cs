using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CW_MVC_Core_10_Auth2.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(
            UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Role name is important ...");
            }
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest("Role already exists ...");
            }
            if (User.Identity.IsAuthenticated)
            {
                var role = new IdentityRole { Name = roleName };
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return Ok($"The role: {role.Name} is created ...");
                }
                return BadRequest(Json(result.Errors));
            }
            return BadRequest("Role create error ...");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email or password are require");
            }
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            //logs errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Email or password are require");
            }
            var result = await _signInManager.PasswordSignInAsync(
                email, 
                password,
                isPersistent: false,
                lockoutOnFailure:false
                );
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Emaik or password are error ...");
        }
        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AssignRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(string id, string roleName)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(roleName))
            {
                return BadRequest("Id or Name Role are require");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                return NotFound($"The role {roleName} not found");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            return BadRequest(Json(result.Errors));
        }

    }
}
