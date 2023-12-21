using api_catalogo.Context;
using api_catalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _context.Categorias
                    .Include(p => p.Produtos)
                    .Where(c => c.CategoriaId <= 5) //Otimizando o desempenho: Nunca retorne objetos relacionados sem aplicar um filtro
                    .ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            var categorias = _context.Categorias
                                .AsNoTracking() //Otimizando o desempenho: Método AsNoTracking() ajuda na otimização das consultas Get()
                                .Take(10)       //Otimizando o desempenho: Nunca retornar todos os registros em uma consulta
                                .ToList();      

            if (categorias is null)
                return NotFound("Categorias não encontrados");

            return categorias;
        }

        [HttpGet("{id:int}", Name = "GetCategoriaById")]
        public ActionResult<Categoria> GetCategoriaById(int id)
        {
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria == null)
                return NotFound("Categoria não encontrado!");

            return categoria;
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest();

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return CreatedAtAction("GetCategoriaById", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest();

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //var produto = _context.Produtos.Find(id);
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
                return NotFound();

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
