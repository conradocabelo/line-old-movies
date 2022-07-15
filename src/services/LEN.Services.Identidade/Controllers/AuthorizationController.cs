using LEN.Core.Api.Controllers;
using LEN.Services.Identidade.Extensions;
using LEN.Services.Identidade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LEN.Services.Identidade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : BaseController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenFactory _tokenFactory;

        public AuthorizationController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ITokenFactory tokenFactory)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(usuarioLogin.Email);
                return Ok(_tokenFactory.Create(user));
            }

            if (result.IsLockedOut)
            {
                AdicionarErroProcessamento("As tentativas de login mal sucedidas bloquearam o usuario");
                return CustomResponse();
            }

            AdicionarErroProcessamento("Usuario ou Senha incorretos");
            return CustomResponse();
        }
    }
}
