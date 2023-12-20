using api_catalogo.Context;
using api_catalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api_catalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        /*
         * Endpoint não assincrono que retorna uma lista de Produtos em caso de sucesso 
         * e em caso de produto nulo retorna uma Status 404.
         * 
         * Para retornarmos a lista de produtos ou a NotFound utiliozamos o tipo ActionResult conforme 
         * Exemplo: ActionResult<IEnumerable<Produto>>
         */
        [HttpGet]
        public ActionResult<IEnumerable<Produto>> Get() 
        { 
            var produtos = _context.Produtos.ToList();

            if (produtos is null)
                return NotFound("Produtos não encontrados");

            return produtos;
        }


        /*
         Method: 
         First() - Se não encontrar retorna uma exception
         FirstOrDefault() - Se não encontrar retorna um null
         */
        [HttpGet("{id:int}")]
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            return produto;
        }
    }
}
