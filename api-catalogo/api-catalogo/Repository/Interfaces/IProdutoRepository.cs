﻿using api_catalogo.Models;
using api_catalogo.Pagination;

namespace api_catalogo.Repository.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<PagedList<Produto>> GetProdutosParameter(ProdutosParameters produtosParameters);

        Task<IEnumerable<Produto>> GetProdutosPorPreco();
    }
}
