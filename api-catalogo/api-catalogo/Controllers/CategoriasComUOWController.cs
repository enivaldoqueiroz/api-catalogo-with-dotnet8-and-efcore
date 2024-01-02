using api_catalogo.DTOs;
using api_catalogo.Models;
using api_catalogo.Pagination;
using api_catalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace api_catalogo.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]//DataAnnotation para autenticar com o Bearer
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
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasProdutos()
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasProdutos();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            
            return categoriasDto;
        }

        [HttpGet]
        [Route("GetCategoriasSemPaginacao")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            try
            {
                var categorias = await _unitOfWork.CategoriaRepository.Get().ToListAsync();

                if (categorias is null)
                    return NotFound("Categorias não encontrados");

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
        }

        [HttpGet]
        [Route("GetCategoriasComPaginacao")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias([FromQuery] CategoriasParameters categoriasParameters)
        {
            try
            {
                var categorias = await _unitOfWork.CategoriaRepository.GetCategoriasParameter(categoriasParameters);

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

                Response.Headers.Append("X-Panigation", JsonSerializer.Serialize(metadata));

                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);

                return categoriasDto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação. Entre em contato com o suporte.");
            }
        }

        [HttpGet("{id:int}")]
        [ActionName(nameof(GetCategoriasById))]
        public async Task<ActionResult<CategoriaDTO>> GetCategoriasById(int id)
        {
            try
            {
                var categoria = await _unitOfWork.CategoriaRepository.GetById(p => p.CategoriaId == id);

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
        public async Task<ActionResult> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
                return BadRequest("Dados Invalidos.");

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _unitOfWork.CategoriaRepository.Add(categoria);
            await _unitOfWork.Commit();

            return CreatedAtAction("GetCategoriasById", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
                return BadRequest("Dados Invalidos.");

            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.Commit();

            var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.GetById(p => p.CategoriaId == id);

            if (categoria is null)
                return NotFound($"Categoria com id = {id} não encontrado...");

            _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.Commit();

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }
    }
}
