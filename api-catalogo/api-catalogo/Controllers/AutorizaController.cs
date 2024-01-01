using api_catalogo.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorizaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AutorizaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public ActionResult<string> Get()
        {
            return "AutorizaController :: Acesso em : " + DateTime.Now.ToLongDateString();
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Post([FromBody] UsuarioDTO usuarioDTO)
        {
            if (usuarioDTO == null)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            var user = new IdentityUser
            {
                UserName = usuarioDTO.Email,
                Email = usuarioDTO.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, usuarioDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody]UsuarioDTO userInfo)
        {
            //verifica se o modelo é válido(Pode ser removido pois a DataAnnotation [ApiController] encapsula o anotação)
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));

            //verifica as credenciais do usuario e retorna um valor
            var result = await _signInManager.PasswordSignInAsync(userInfo.Email, 
                                                                  userInfo.Password,
                                                                  isPersistent: false,
                                                                  lockoutOnFailure: false);

            if (result.Succeeded)
                return Ok();
            else
            {
                ModelState.AddModelError(string.Empty, "Login Inválido....");
                return Ok(ModelState);
            }
        }
    }
}
