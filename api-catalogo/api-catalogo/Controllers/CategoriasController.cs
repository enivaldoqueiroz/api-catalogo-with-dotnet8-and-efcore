using api_catalogo.Context;
using api_catalogo.Models;
using api_catalogo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public CategoriasController(AppDbContext appDbContext, IConfiguration configuration)
        {
            _context = appDbContext;
            _configuration = configuration;
        }

        [HttpGet("autor")]
        public string GetAutor()
        {
            var autor = _configuration["autor"];
            var conexao = _configuration["ConnectionStrings:DefaultConnection"];

            return $"Autor : {autor} Conexao: {conexao}";
        }

        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices] IMeuServico meuServico, string nome)
        {
            return meuServico.Saudacao(nome);
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
            try
            {
                //throw new DataMisalignedException();//Forçando a exceção para testar o StatusCode

                var categorias = _context.Categorias
                                .AsNoTracking() //Otimizando o desempenho: Método AsNoTracking() ajuda na otimização das consultas Get()
                                .Take(10)       //Otimizando o desempenho: Nunca retornar todos os registros em uma consulta
                                .ToList();

                if (categorias is null)
                    return NotFound("Categorias não encontrados");

                return categorias;
            }
            catch (Exception)
            {
                //link para outros StatusCodes https://learn.microsoft.com/pt-br/dotnet/api/microsoft.aspnetcore.http.statuscodes?view=aspnetcore-8.0
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
        }

        [HttpGet("{id:int}", Name = "GetCategoriaById")]
        public ActionResult<Categoria> GetCategoriaById(int id)
        {
            try
            {
                var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

                if (categoria == null)
                    return NotFound($"Categoria com id = {id} não encontrado...");

                return categoria;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                        "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
            
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
                return BadRequest("Dados Invalidos.");

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return CreatedAtAction("GetCategoriaById", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest("Dados Invalidos.");

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //var produto = _context.Produtos.Find(id);//Outra forma de localizar o id
            var categoria = _context.Categorias.FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Categoria com id = {id} não encontrado...");

            _context.Categorias.Remove(categoria);
            _context.SaveChanges();

            return Ok(categoria);
        }
    }
}
