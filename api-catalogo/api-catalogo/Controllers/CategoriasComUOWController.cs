using api_catalogo.Models;
using api_catalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasComUOWController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriasComUOWController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<Categoria>> GetCategoriasProdutos()
        {
            return _unitOfWork.CategoriaRepository.GetCategoriasProdutos().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> Get()
        {
            try
            {
                var categorias = _unitOfWork.CategoriaRepository.Get().ToList();

                if (categorias is null)
                    return NotFound("Categorias não encontrados");

                return categorias;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
        }

        [HttpGet("{id:int}")]
        [ActionName(nameof(GetCategoriasProdutosById))]
        public ActionResult<Categoria> GetCategoriasProdutosById(int id)
        {
            try
            {
                var categoria = _unitOfWork.CategoriaRepository.GetCategoriasProdutos().FirstOrDefault(p => p.CategoriaId == id);

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

            _unitOfWork.CategoriaRepository.Add(categoria);
            _unitOfWork.Commit();

            return CreatedAtAction("GetCategoriasProdutosById", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
                return BadRequest("Dados Invalidos.");

            _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.GetCategoriasProdutos().FirstOrDefault(p => p.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Categoria com id = {id} não encontrado...");

            _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();

            return Ok(categoria);
        }
    }
}
