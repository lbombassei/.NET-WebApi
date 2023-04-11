using Application.Entities;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categorias>> GetAll()
        {
            var categorias = _categoriaService.GetAll();
            return Ok(categorias);
        }

        [HttpPost]
        public ActionResult<Categorias> Add([FromBody] Categorias categoria)
        {
            var novaCategoria = _categoriaService.Add(categoria);
            return CreatedAtAction(nameof(GetAll), new { id = novaCategoria.Id }, novaCategoria);
        }
    }
}
