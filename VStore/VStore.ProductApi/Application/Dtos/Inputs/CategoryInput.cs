using System.ComponentModel.DataAnnotations;

namespace VStore.ProductApi.Application.Dtos.Inputs
{
    public class CategoryInput
    {
        public CategoryInput(string name)
        {
            Name = name;
        }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 100 caracteres")]
        public string Name { get; private set; }
    }
}
