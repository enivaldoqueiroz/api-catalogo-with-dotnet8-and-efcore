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
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutoPrecos()
        {
            var produto = await _unitOfWork.ProdutoRepository.GetProdutosPorPreco();
            List<ProdutoDTO> produtoDTO = _mapper.Map<List<ProdutoDTO>>(produto);

            return produtoDTO;
        }

        /// <summary>
        /// Obtém uma lista paginada de produtos com base nos parâmetros fornecidos.
        /// </summary>
        /// <param name="produtosParameters">Parâmetros de paginação e filtro.</param>
        /// <returns>Uma ActionResult contendo uma lista paginada de produtos no formato ProdutoDTO.</returns>
        [HttpGet]
        [Route("GetProdutosComPaginacao")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> GetProdutos([FromQuery] ProdutosParameters produtosParameters)
        {
            // Chama o método do repositório para obter os produtos com base nos parâmetros.
            var produtos = await _unitOfWork.ProdutoRepository.GetProdutosParameter(produtosParameters);

            // Verifica se não foram encontrados produtos.
            if (produtos is null)
                return NotFound("Produtos não encontrados");

            // Cria um objeto de metadados para incluir informações de paginação na resposta.
            var metadata = new
            {
                produtos.TotalCount,
                produtos.PageSize,
                produtos.CurrentPage,
                produtos.TotalPages,
                produtos.HasNext,
                produtos.HasPrevious
            };

            // Adiciona os metadados aos cabeçalhos da resposta.
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metadata));

            // Converte a lista de produtos para ProdutoDTO usando AutoMapper.
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);

            // Retorna a lista paginada de produtos no formato ProdutoDTO.
            return produtosDto;
        }

        // /api/produtos
        [HttpGet]
        [Route("GetProdutosSemPaginacao")]
        public async Task<ActionResult<ProdutoDTO>> GetProdutos()
        {
            var produtos = _unitOfWork.ProdutoRepository.Get().ToListAsync();

            if (produtos == null)
                return NotFound("Produto não encontrado!");

            ProdutoDTO produtoDTO = new ProdutoDTO();

            foreach (var produto in await produtos)
            {
                //Mapeamento de um DTO de forma manual
                produtoDTO.ProdutoId = produto.ProdutoId;
                produtoDTO.Nome = produto.Nome;
                produtoDTO.Preco = produto.Preco;
                produtoDTO.ImagemUrl = produto.ImagemUrl;
                produtoDTO.Descricao = produto.Descricao;
                produtoDTO.CategoriaId = produto.CategoriaId;
            }

            return produtoDTO;
        }

        // /api/produtos/id
        [HttpGet("{id}")]
        [ActionName(nameof(GetProdutoComUOWById))]
        public async Task<ActionResult<ProdutoDTO>> GetProdutoComUOWById(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

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
        public async Task<ActionResult> Post(ProdutoDTO produtoDto)
        {
            if (produtoDto is null)
                return BadRequest("Dados Invalidos.");

            Produto produto = _mapper.Map<Produto>(produtoDto);

            _unitOfWork.ProdutoRepository.Add(produto);
            await _unitOfWork.Commit();

            ProdutoDTO produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            var rota = new { id = produtoDto.ProdutoId };

            // Use CreatedAtAction para criar a resposta 201 Created
            return CreatedAtAction(nameof(GetProdutoComUOWById), rota, produtoDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
                return BadRequest($"Produto com Id : {id} errado");

            var produto = _mapper.Map<Produto>(produtoDto);

            _unitOfWork.ProdutoRepository.Update(produto);
            await _unitOfWork.Commit();

            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(produtoDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<ProdutoDTO>> Delete(int id)
        {
            var produto = await _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            _unitOfWork.ProdutoRepository.Delete(produto);
            await _unitOfWork.Commit();

            var ProdutoDTO = _mapper.Map<ProdutoDTO>(produto);

            return Ok(ProdutoDTO);
        }
    }
}
