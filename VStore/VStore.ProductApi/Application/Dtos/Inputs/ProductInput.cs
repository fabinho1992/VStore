using System.ComponentModel.DataAnnotations;
using VStore.ProductApi.Domain.Models;

namespace VStore.ProductApi.Application.Dtos.Inputs
{
    public class ProductInput
    {
        public ProductInput(string name, decimal price, string description, long stock, string imageUrl, int categoryId)
        {
            Name = name;
            Price = price;
            Description = description;
            Stock = stock;
            ImageUrl = imageUrl;
            CategoryId = categoryId;
        }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Name { get; private set; }
        [Required(ErrorMessage = "O preço é obrigatório")]
        [Range(0.01, 100000.00, ErrorMessage = "O preço deve estar entre 0,01 e 10.000,00")]
        [DataType(DataType.Currency)]
        public decimal Price { get; private set; }
        [StringLength(500, ErrorMessage = "A descrição pode ter no máximo 500 caracteres")]
        public string Description { get; private set; }
        [Required(ErrorMessage = "O estoque é obrigatório")]
        [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo")]
        public long Stock { get; private set; }
        [StringLength(300, ErrorMessage = "A URL da imagem pode ter no máximo 300 caracteres")]
        public string ImageUrl { get; private set; }
        [Required(ErrorMessage = "O ID da categoria é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "O ID da categoria deve ser maior que 0")]
        [Display(Name = "ID da Categoria")]
        public int CategoryId { get; private set; }
    }
}
