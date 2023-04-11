using Application.Context;
using Application.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProvaContext _context;

        public ProdutoService(IProvaContext context)
        {
            _context = context;
        }

        public async Task<List<Produtos>> GetAll(string term, int page = 1, int pageSize = 10)
        {
            var produtos = _context.Produtos.Include(p => p.Categoria).AsQueryable();

            if (!string.IsNullOrEmpty(term))
            {
                produtos = produtos.Where(p => p.Nome.Contains(term) || p.Categoria.Nome.Contains(term));
            }

            return await produtos.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public Produtos GetById(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.Id == id);
            return produto;
        }

        public async Task<Produtos> Add(Produtos produto)
        {
            if (string.IsNullOrEmpty(produto.Nome))
            {
                throw new ArgumentException("O campo Nome é obrigatório.");
            }

            if (_context.Produtos.Any(p => p.Nome == produto.Nome))
            {
                throw new ArgumentException("Já existe um produto com o mesmo nome cadastrado.");
            }

            if (produto.PrecoUnitario <= 0)
            {
                throw new ArgumentException("O preço unitário deve ser maior que zero.");
            }

            if (produto.QuantidadeEstoque <= 0)
            {
                throw new ArgumentException("A quantidade deve ser maior que zero.");
            }

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return produto;
        }

        public async Task<string> Update(Produtos produto)
        {
            var produtoExistente = await _context.Produtos.FindAsync(produto.Id);

            if (produtoExistente == null)
            {
                return "Produto não encontrado.";
            }

            if (string.IsNullOrEmpty(produto.Nome))
            {
                return "O campo Nome é obrigatório.";
            }

            if (await _context.Produtos.AnyAsync(p => p.Nome == produto.Nome && p.Id != produto.Id))
            {
                return "Já existe um produto com o mesmo nome cadastrado.";
            }

            if (produto.PrecoUnitario <= 0)
            {
                return "O preço unitário deve ser maior que zero.";
            }

            if (produto.QuantidadeEstoque <= 0)
            {
                return "A quantidade deve ser maior que zero.";
            }

            produtoExistente.Nome = produto.Nome;
            produtoExistente.PrecoUnitario = produto.PrecoUnitario;
            produtoExistente.QuantidadeEstoque = produto.QuantidadeEstoque;
            produtoExistente.CategoriaId = produto.CategoriaId;

            await _context.SaveChangesAsync();

            return null;
        }

        public async Task<string> Delete(int id)
        {
            var produto = await _context.Produtos.FindAsync((long)id);

            if (produto == null)
            {
                return "Produto não encontrado.";
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return null;
        }
    }

}

