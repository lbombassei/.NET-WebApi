using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string senha)
        {
            var result = await _usuarioService.DoLogin(email, senha);

            if (result == null)
            {
                return BadRequest("Email ou senha incorretos.");
            }

            return Ok("Usuário logado com sucesso.");
        }
    }
}