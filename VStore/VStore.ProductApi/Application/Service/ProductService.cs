using AutoMapper;
using VStore.ProductApi.Application.Dtos;
using VStore.ProductApi.Application.Dtos.Inputs;
using VStore.ProductApi.Application.Dtos.Responses;
using VStore.ProductApi.Domain.IRepository;
using VStore.ProductApi.Domain.IService;
using VStore.ProductApi.Domain.Models;
using VStore.ProductApi.Infrastructure.Repository;

namespace VStore.ProductApi.Application.Service
{
    public class ProductService : ICRUDService<ProductResponse, ProductInput>
    {
        private readonly IRepository<Product> _repository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<List<ProductResponse>>> GetAll()
        {
            var products = await _repository.GetAll();
            var responses = _mapper.Map<List<ProductResponse>>(products);
            return ResultViewModel<List<ProductResponse>>.Success(responses);
        }

        public async Task<ResultViewModel<ProductResponse>> FindById(int id)
        {
            var product = await _repository.FindById(id);
            if (product == null)
                return ResultViewModel<ProductResponse>.Error("Product not found");

            var response = _mapper.Map<ProductResponse>(product);
            return ResultViewModel<ProductResponse>.Success(response);
        }

        public async Task<ResultViewModel<List<ProductResponse>>> GetProductsOrder(string ids)
        {
                // Converter string "1,2,3" para lista [1, 2, 3]
                var productIds = ids.Split(',')
                    .Select(id => int.Parse(id.Trim()))
                    .Distinct()
                    .ToList();

                // Buscar produtos do banco
                var products = await _repository.GetProductsByIdsAsync(productIds);

                // Converter para DTO
                var productDtos = products.Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = p.Catergory.Name
                }).ToList();

                return ResultViewModel<List<ProductResponse>>.Success(productDtos);

        }

        public async Task<ResultViewModel<List<ProductResponse>>> FindByText(string query)
        {
            var products = await _repository.FindByText(query);
            var responses = _mapper.Map<List<ProductResponse>>(products);
            return ResultViewModel<List<ProductResponse>>.Success(responses);
        }

        public async Task<ResultViewModel<ProductResponse>> Create(ProductInput create)
        {
            var product = _mapper.Map<Product>(create);
            var result = await _repository.Create(product);
            var response = _mapper.Map<ProductResponse>(result);
            return ResultViewModel<ProductResponse>.Success(response);
        }

        public async Task<ResultViewModel<ProductResponse>> Update(int id, ProductInput update)
        {
            var existingProduct = await _repository.FindById(id);
            if (existingProduct == null)
                return ResultViewModel<ProductResponse>.Error("Product not found");

            _mapper.Map(update, existingProduct);
            await _repository.Update(existingProduct);
            var response = _mapper.Map<ProductResponse>(existingProduct);
            return ResultViewModel<ProductResponse>.Success(response);
        }

        public async Task<ResultViewModel<bool>> Delete(int id)
        {
            var product = await _repository.FindById(id);
            if (product == null)
                return ResultViewModel<bool>.Error("Product not found");

            await _repository.Delete(id);
            return ResultViewModel<bool>.Success(true);
        }
    }
}
