using Application.Entities;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public IActionResult GetAll([FromQuery] string term, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var produtos = _produtoService.GetAll(term, page, pageSize);
                return Ok(produtos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] Produtos produto)
        {
            try
            {
                var novoProduto = await _produtoService.Add(produto);
                return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Produtos produto)
        {
            try
            {
                produto.Id = id;
                var erro = await _produtoService.Update(produto);

                if (erro != null)
                {
                    return BadRequest(erro);
                }

                return Ok("O produto foi Atualizado!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var erro = await _produtoService.Delete(id);

                if (erro != null)
                {
                    return BadRequest(erro);
                }

                return Ok("Produto deletado com sucesso!");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var produto = _produtoService.GetById(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }
    }
}
