using Application.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface ICategoriaService
    {
        Task<List<Categorias>> GetAll();
        Task Add(Categorias categoria);
    }
}
