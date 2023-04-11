using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Context;
using Application.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ProvaContext _context;
        private readonly DbSet<Usuarios> _usuarioRepo;

        public UsuarioService(ProvaContext context)
        {
            _context = context;
            _usuarioRepo = _context.Set<Usuarios>();
        }

        public async Task<IActionResult> DoLogin(string email, string senha)
        {
            var usuario = await _usuarioRepo.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);

            if (usuario == null)
            {
                return new BadRequestObjectResult("Usuário não encontrado.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("MySecretKey");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("user", usuario.Nome)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return new OkObjectResult(new { Token = tokenString });
        }
    }
}
