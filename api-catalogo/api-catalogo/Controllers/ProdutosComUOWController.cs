using api_catalogo.DTOs;
using api_catalogo.Models;
using api_catalogo.Pagination;
using api_catalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]//Rotas produtos
    [ApiController]
    public class ProdutosComUOWController : ControllerBase
    {
        //Usando o padrão UnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProdutosComUOWController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutoPrecos()
        {
            List<Produto> produto =  _unitOfWork.ProdutoRepository.GetProdutosPorPreco().ToList();
            List<ProdutoDTO> produtoDTO = _mapper.Map<List<ProdutoDTO>>(produto);

            return produtoDTO;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutos([FromQuery] ProdutosParameters produtosParameters) 
        { 
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosParameter(produtosParameters);
            if (produtos is null)
                return NotFound("Produtos não encontrados");

            var matadata = new 
            { 
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            Response.Headers.Append("X-Panigation", JsonSerializer.Serialize(matadata));

            var ProdutosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            return ProdutosDto;
        }
       
        // /api/produtos/id
        [HttpGet("{id}")]
        [ActionName(nameof(GetProdutoComUOWById))]
        public ActionResult<ProdutoDTO> GetProdutoComUOWById(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null)
                return NotFound("Produto não encontrado!");

            //Mapeamento de um DTO de forma manual
            ProdutoDTO produtoDTO = new ProdutoDTO
            {
                ProdutoId = produto.ProdutoId,
                Nome = produto.Nome,
                Preco = produto.Preco,
                ImagemUrl = produto.ImagemUrl,
                Descricao = produto.Descricao,
                CategoriaId = produto.CategoriaId
            };

            return produtoDTO;
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest("Dados Invalidos.");

            Produto produto = _mapper.Map<Produto>(produtoDto);

            _unitOfWork.ProdutoRepository.Add(produto);
            _unitOfWork.Commit();

            ProdutoDTO produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            var rota = new { id = produtoDto.ProdutoId };

            // Use CreatedAtAction para criar a resposta 201 Created
            return CreatedAtAction(nameof(GetProdutoComUOWById), rota, produtoDTO);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest($"Produto com Id : {id} errado");

            var produto = _mapper.Map<Produto>(produtoDto);

            _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            var ProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(ProdutoDTO);
        }
    }
}
