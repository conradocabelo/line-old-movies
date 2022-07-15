using LEN.Core.Api.Controllers;
using LEN.Services.Identidade.Extensions;
using LEN.Services.Identidade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LEN.Services.Identidade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenFactory _tokenFactory;

        public RegisterController(UserManager<IdentityUser> userManager, ITokenFactory tokenFactory)
        {
            _userManager = userManager;
            _tokenFactory = tokenFactory;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult> Registar(UsuarioRegistro usuarioRegistro)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = usuarioRegistro.Email,
                Email = usuarioRegistro.Email,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, usuarioRegistro.Senha);

            if (result.Succeeded)
            {
                return CustomResponse(await _tokenFactory.Create(user));
            }

            foreach (var error in result.Errors)
                AdicionarErroProcessamento(error.Description);

            return CustomResponse();
        }
    }
}
