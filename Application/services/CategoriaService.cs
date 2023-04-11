using Application.Context;
using Application.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ProvaContext _context;

        public CategoriaService(ProvaContext context)
        {
            _context = context;
        }
        public async Task<List<Categorias>> GetAll()
        {
            return await _context.Categorias.ToListAsync();
        }
        public async Task Add(Categorias categoria)
        {
            // Verificar se o campo Nome foi preenchido
            if (string.IsNullOrEmpty(categoria.Nome))
            {
                throw new Exception("O campo Nome é obrigatório.");
            }

            // Verificar se a categoria já existe com o mesmo nome
            var categoriaExistente = await _context.Categorias.FirstOrDefaultAsync(c => c.Nome == categoria.Nome);
            if (categoriaExistente != null)
            {
                throw new Exception("Já existe uma categoria com o mesmo nome.");
            }

            // Gravar a nova categoria no banco de dados
            await _context.Categorias.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }

    }
}
