using api_catalogo.Context;
using api_catalogo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]//Rotas produtos
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProdutosController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        // /api/primeiro
        [HttpGet("primeiro")]   // Atendendo 3 endpoint distintos
        [HttpGet("teste")]      // Atendendo 3 endpoint distintos
        [HttpGet("/primeiro")]  // Atendendo 3 endpoint distintos
        public ActionResult<Produto> GetPrimeiro()
        {
            var produto = _context.Produtos.FirstOrDefault();

            if (produto is null)
                return NotFound("Produto não encontrado");

            return produto;
        }

        // /api/primeiro
        //Retrinção para reconhecer letras: valor:alpha
        //Retrinção para reconhecer letras e com tamanha de 5 caracteres: valor:alpha:length(5)
        [HttpGet("{valor:alpha:length(5)}")] //Restrição de valores somente de A à Z
        [HttpGet("RestricaoPorCaracteres")]
        public ActionResult<Produto> GetRestricaoPorCaracteres(string valor)
        {
            var valorAlpha = valor;
            var produto = _context.Produtos.FirstOrDefault();

            if (produto is null)
                return NotFound("Produto não encontrado");

            return produto;
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

        // /api/produtos/id
        //[HttpGet("{id:int}/{nome=Caderno}", Name="GetProdutoById")]
        //public ActionResult<Produto> GetProdutoById(int id, string nome)
        //{
        //    var parametro = nome;

        //    var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

        //    if (produto == null)
        //        return NotFound("Produto não encontrado!");

        //    return produto;
        //}

        /*
        Para inserir uma restrição em uma endpoint demoves inserir
        id:int:min(1) para fim de evitar consultas desnecessaria
         */

        // /api/produtos/id
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")] //Restrição de Id no minimo 1 ou mior que 1
        public ActionResult<Produto> Get(int id)
        {
            var produto = _context.Produtos
                .AsNoTracking()
                .FirstOrDefault(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            return produto;
        }

        /*
         Obs: Ao utilizarmos a DataAnnotation [ApiController] não precisamos
         inserir o [FromBody] Produto produto na assinatura do método e nem 
         colocar a validação ModelState.IsValid.

            [HttpPost]
            public ActionResult PostWithFromBody([FromBody]Produto produto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(produto);

                return Ok(produto);
            }
         */

        //[HttpPost]
        //public ActionResult Post(Produto produto)
        //{
        //    if (produto is null)
        //        return BadRequest();

        //    _context.Produtos.Add(produto); //Método Add() inclui o produto no contexto
        //    _context.SaveChanges(); //Método SaveChanges() Persisti os dados na tabela Produtos 

        //    /*
        //     A instancia CreatedAtActionResult vai ocionar o endpoint ObterProduto - public ActionResult<Produto> Get(int id) para retorbar o id
        //     */
        //    var var = new CreatedAtActionResult("GetProdutoById", "GetProdutoById", new { id = produto.ProdutoId }, produto);
        //    return Ok(var.Value);

        //    //return new CreatedAtActionResult(nameof(GetProdutoById), "GetProdutoById", new { id = produto.ProdutoId }, produto); ; //Retorna 201 Created

        // OBS: a Classe CreatedAtActionResult não está funcionando para esse caso
        //}

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Dados Invalidos.");

            _context.Produtos.Add(produto);
            _context.SaveChanges();

            // Use CreatedAtAction para criar a resposta 201 Created
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
            //var produto = _context.Produtos.Find(id);
            var produto = _context.Produtos.FirstOrDefault(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            _context.Produtos.Remove(produto);
            _context.SaveChanges();

            return Ok(produto);
        }

        //Tipos de Retorno

        /*
         #IActionResult
         Tipo de retorno IActionResult para retornos de StatusCode
         Obs: Podemos usar o IActionResult ou o ActionResult, mas o ideia é o usar a interface
         */
        [HttpGet]
        [Route("GetWithIActionResult")]
        public IActionResult Get3()
        {
            var produtos = _context.Produtos.ToList();

            if (produtos is null)
                return NotFound("Produtos não encontrados");

            return Ok();
        }

        /*
         ActionResult<T> : Permite o retorno de um tipo derivado de ActionResult ou o retorno de um 
         tipo especifico(T)
         */
        [HttpGet]
        [Route("GetWithActionResult")]
        public ActionResult<Produto> Get4()
        {
            var produtos = _context.Produtos.ToList();

            if (produtos is null)
                return NotFound("Produtos não encontrados");

            return Ok();
        }

        //Metodos Actions Assícronos


    }
}
