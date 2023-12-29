using api_catalogo.DTOs;
using api_catalogo.Models;
using api_catalogo.Pagination;
using api_catalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasComUOWController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriasComUOWController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos()
        {
            var categorias = _unitOfWork.CategoriaRepository.GetCategoriasProdutos().ToList();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            
            return categoriasDto;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = _unitOfWork.CategoriaRepository.GetCategoriasParameter(categoriasParameters);

                if (categorias is null)
                    return NotFound("Categorias não encontrados");

                var metadata = new
                {
                    categorias.TotalCount,
                    categorias.PageSize,
                    categorias.CurrentPage,
                    categorias.TotalPages,
                    categorias.HasNext,
                    categorias.HasPrevious
                };

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
        }

        [HttpGet("{id:int}")]
        [ActionName(nameof(GetCategoriasProdutosById))]
        public ActionResult<CategoriaDTO> GetCategoriasProdutosById(int id)
        {
            try
            {
                var categoria = _unitOfWork.CategoriaRepository.GetCategoriasProdutos().FirstOrDefault(p => p.CategoriaId == id);

                if (categoria == null)
                    return NotFound($"Categoria com id = {id} não encontrado...");

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return categoriaDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, 
                        "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
        }

        [HttpPost]
        public ActionResult Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest("Dados Invalidos.");

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _unitOfWork.CategoriaRepository.Add(categoria);
            _unitOfWork.Commit();

            return CreatedAtAction("GetCategoriasProdutosById", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
                return BadRequest("Dados Invalidos.");

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

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

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }
    }
}
