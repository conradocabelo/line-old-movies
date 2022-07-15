using LEN.Services.Identidade.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LEN.Services.Identidade.Extensions
{
    public interface ITokenFactory
    {
        Task<UsuarioRespostaLogin> Create(IdentityUser identityUser);
    }

    public class TokenFactory : ITokenFactory
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthSettings _authSettings;

        public TokenFactory(UserManager<IdentityUser> userManager, IOptions<AuthSettings> authSettings)
        {
            _userManager = userManager;
            _authSettings = authSettings.Value;
        }

        public async Task<UsuarioRespostaLogin> Create(IdentityUser identityUser)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(await GenerateClaims(identityUser));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _authSettings.Emissor,
                Audience = _authSettings.DominioValidade,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_authSettings.TempoExpiracao),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var usuarioRespostaLogin = new UsuarioRespostaLogin();
            usuarioRespostaLogin.AccessToken = tokenHandler.WriteToken(token);
            usuarioRespostaLogin.ExpiresIn = TimeSpan.FromHours(_authSettings.TempoExpiracao).TotalSeconds;

            return usuarioRespostaLogin;
        }

        private async Task<IList<Claim>> GenerateClaims(IdentityUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in roles)
            {
                claims.Add(new Claim("role", userRole));
            }

            return claims;
        }

        private static long ToUnixEpochDate(DateTime date)
           => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
