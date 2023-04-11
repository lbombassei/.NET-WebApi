using Application.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IProdutoService
    {
        Task <List<Produtos>> GetAll(string term, int page, int pageSize);
        Task<Produtos> Add(Produtos produto);
        Task<string> Update(Produtos produto);
        Task<string> Delete(int id);
        Produtos GetById(int id);
    }
}