using Entities.Entities;
using Entities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using WebApi.Models;
using WebApi.Token;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(UserManager<ApplicationUser> useManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = useManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/AdicionarUsuarioIdentity")]
        public async Task<IActionResult> AdicionarUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
                return Ok("Dados insuficientes");

            var user = new ApplicationUser
            {
                UserName = login.email,
                Email = login.email,
                CPF = login.cpf,
                Tipo = TipoUsuario.Comum,
            };

            var resultado = await _userManager.CreateAsync(user, login.senha);

            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);
            }

            //geracao de confirmacao caso precise
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // retorno email
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var ret = await _userManager.ConfirmEmailAsync(user, code);

            if (ret.Succeeded)
                return Ok("Usuário criado");
            else
                return Ok("Usuário não criado");

        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
            {
                return Unauthorized();
            }

            var result = await _signInManager.PasswordSignInAsync(login.email, login.senha, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                //var user = new ApplicationUser
                //{
                //    UserName = login.email,
                //    Email = login.email,
                //};

                var userCurrent = await _userManager.FindByEmailAsync(login.email);
                var idUsuario = userCurrent.Id;

                var token = new TokenJwtBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-0987654321"))
                    .AddSSubject("Empresa - None")
                    .AddIssuer("Dev.Security.Bearer")
                    .AddAudience("Dev.Security.Bearer")
                    .AddClaim("idUsuario", idUsuario)
                    .AddExpery(5)
                    .Builder();

                return Ok(token.value);
            }
            else
                return Unauthorized();
        }

    }
}
