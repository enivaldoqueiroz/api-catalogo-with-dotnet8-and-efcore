using Microsoft.EntityFrameworkCore;

namespace api_catalogo.Pagination
{
    // A classe PagedList<T> representa uma lista paginada de itens do tipo T.
    public class PagedList<T> : List<T>
    {
        // Propriedades públicas para informações de paginação.
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        // Propriedades calculadas para verificar a existência de páginas anteriores e próximas.
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        // Construtor da classe PagedList.
        // - items: Lista de itens do tipo T.
        // - pageNumber: Número da página atual.
        // - pageSize: Tamanho da página.
        // - totalCount: Número total de itens na lista.
        public PagedList(List<T> items, int pageNumber, int pageSize, int totalItemsCount)
        {
            // Adiciona os itens à lista.
            AddRange(items);

            // Atribui os valores fornecidos às propriedades correspondentes.
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            PageSize = pageSize;
            TotalCount = totalItemsCount;
        }

        // Método estático para criar uma instância de PagedList a partir de uma fonte IQueryable.
        // - source: Fonte IQueryable dos itens.
        // - pageNumber: Número da página desejada.
        // - pageSize: Tamanho da página.
        public async static Task<PagedList<T>> ToPagedList(IQueryable<T> source, int pageNumber, int pageSize)
        {
            // Obtém o total de itens na fonte.
            var count = source.Count();

            // Seleciona os itens da página desejada.
            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            // Retorna uma nova instância de PagedList com os itens, número total de itens, número da página e tamanho da página.
            return new PagedList<T>(items, pageNumber, pageSize, count);
        }
    }

}
