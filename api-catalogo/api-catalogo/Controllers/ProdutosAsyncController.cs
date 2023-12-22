using api_catalogo.Context;
using api_catalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosAsyncController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosAsyncController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        // /api/primeiro
        [HttpGet("{valor:alpha:length(5)}")]
        public ActionResult<Produto> GetRestricaoPorCaracteres(string valor)
        {
            var valorAlpha = valor;
            var produto = _context.Produtos.FirstOrDefault();

            if (produto is null)
                return NotFound("Produto não encontrado");

            return produto;
        }

        // /api/produtos/
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Produto>>> Get2() 
        { 
            return await _context.Produtos.AsNoTracking().ToListAsync(); 
        }

        // /api/produtos/id
        [HttpGet("{id}", Name = "GetProdutoById")] //Restrição de Id no minimo 1 ou mior que 1
        public async Task<ActionResult<Produto>>  Get(int id)
        {
            var produto = await _context.Produtos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            return produto;
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Dados Invalidos.");

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            return CreatedAtAction("GetProdutoById", new { id = produto.ProdutoId }, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }
    }
}
