using api_catalogo.Models;

namespace api_catalogo.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }
        public string? Nome { get; set; }
        public string? ImagemUrl { get; set; }
        public ICollection<ProdutoDTO>? Produtos { get; set; }
    }
}
