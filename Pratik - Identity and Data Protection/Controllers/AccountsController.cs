using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pratik___Identity_and_Data_Protection.ViewModel;

namespace Pratik___Identity_and_Data_Protection.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Ok(new { message = "Kayıt Başarılı" });
                }

                return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
            }
            return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Giriş Başarılı" });
                }
                else
                {
                    return Unauthorized(new { message = "Kullanıcı Adı veya şifre Hatalı" });
                }
            }
            return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        }
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    return Ok(new { message = "Rol Oluşturuldu" });
                }
                else
                {
                    return BadRequest(new { errors = result.Errors.Select(e => e.Description) });

                }

            }
            return BadRequest(new { message = "Rol Adı Boş Olamaz" });
        }
        [HttpGet("roles")]
        public IActionResult GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole(AddRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            
                
                if (user == null) 
                {
                    return NotFound (new { message = "Kullanıcı Bulunamadi" });
                }


            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
              return NotFound( new { message = "Rol Bulunamadi" });
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            if (result.Succeeded)
            { 
                return Ok(new { message = "Rol Eklendi" });
            }
            else
                { return BadRequest(new { result = result.Errors.Select(x => x.Description) }); 
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);


            if (user == null)
            {
                return NotFound(new { message = "Kullanıcı Bulunamadi" });
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);

        }
    }
}
