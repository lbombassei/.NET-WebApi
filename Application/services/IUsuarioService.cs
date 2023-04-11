using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IUsuarioService
    {
        Task<IActionResult> DoLogin(string email, string senha);
    }
}
