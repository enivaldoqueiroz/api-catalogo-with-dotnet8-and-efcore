using api_catalogo.DTOs;
using api_catalogo.Models;
using api_catalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace api_catalogo.Controllers
{
    [Route("api/[controller]")]//Rotas produtos
    [ApiController]
    public class ProdutosComUOWController : ControllerBase
    {
        //Usando o padrão UnitOfWork
        private readonly IUnitOfWork _unitOfWork;
        public ProdutosComUOWController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("menorpreco")]
        public ActionResult<IEnumerable<Produto>> GetProdutoPrecos()
        {
            return _unitOfWork.ProdutoRepository.GetProdutosPorPreco().ToList();
        }

        [HttpGet]
        public ActionResult<IEnumerable<Produto>> GetProdutos() 
        { 
            var produtos = _unitOfWork.ProdutoRepository.Get().ToList();

            if (produtos is null)
                return NotFound("Produtos não encontrados");

            return produtos;
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
        public ActionResult Post(Produto produto)
        {
            if (produto is null)
                return BadRequest("Dados Invalidos.");

            _unitOfWork.ProdutoRepository.Add(produto);
            _unitOfWork.Commit();

           var rota = new { id = produto.ProdutoId };

            // Use CreatedAtAction para criar a resposta 201 Created
            return CreatedAtAction(nameof(GetProdutoComUOWById), rota, produto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Produto produto)
        {
            if (id != produto.ProdutoId)
                return BadRequest($"Produto com Id : {id} errado");

            _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            return Ok(produto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            //var produto = _unitOfWork.Produtos.Find(id);
            var produto = _unitOfWork.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto is null)
                return NotFound();

            _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            return Ok(produto);
        }
    }
}
