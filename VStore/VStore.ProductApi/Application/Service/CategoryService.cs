using AutoMapper;
using VStore.ProductApi.Application.Dtos;
using VStore.ProductApi.Application.Dtos.Inputs;
using VStore.ProductApi.Application.Dtos.Responses;
using VStore.ProductApi.Domain.IService;
using VStore.ProductApi.Infrastructure.Repository;
using VStore.ProductApi.Domain.Models;
using VStore.ProductApi.Domain.IRepository;

namespace VStore.ProductApi.Application.Service
{
    public class CategoryService : ICRUDService<CategoryResponse, CategoryInput>
    {
        private readonly IRepository<Category> _repository;
        private readonly IMapper _mapper;

        public CategoryService(IRepository<Category> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<CategoryResponse>> Create(CategoryInput create)
        {
            var category = _mapper.Map<Category>(create);
            var result = await _repository.Create(category);

            var response = _mapper.Map<CategoryResponse>(result);
            return ResultViewModel<CategoryResponse>.Success(response);
        }

        public async Task<ResultViewModel<bool>> Delete(int id)
        {
            var categoryDb = await _repository.FindById(id);
            if(categoryDb is null)
            {
                return ResultViewModel<bool>.Error("Category not found");
            }
            var result = await _repository.Delete(id);
            return ResultViewModel<bool>.Success(result);
        }

        public async Task<ResultViewModel<CategoryResponse>> FindById(int id)
        {
            var category = await _repository.FindById(id);
            if (category == null)
            {
                return ResultViewModel<CategoryResponse>.Error("Category not found");
            }

            var response = _mapper.Map<CategoryResponse>(category);
            return ResultViewModel<CategoryResponse>.Success(response);

        }

        public async Task<ResultViewModel<List<CategoryResponse>>> FindByText(string query)
        {
            var categories = await _repository.FindByText(query);
            if(categories is null)
            {
                return ResultViewModel<List<CategoryResponse>>.Error("No categories found");
            }
            var responses = _mapper.Map<List<CategoryResponse>>(categories);
            return ResultViewModel<List<CategoryResponse>>.Success(responses);
        }

        public async Task<ResultViewModel<List<CategoryResponse>>> GetAll()
        {
            var categories = await _repository.GetAll();
            if(categories is null)
            {
               return ResultViewModel<List<CategoryResponse>>.Error("No categories found");
            }
            var responses = _mapper.Map<List<CategoryResponse>>(categories);
            return ResultViewModel<List<CategoryResponse>>.Success(responses);
        }

        public async Task<ResultViewModel<CategoryResponse>> Update(int id, CategoryInput update)
        {
            var categoryDb =  await _repository.FindById(id);
            if(categoryDb is null)
            {
                return ResultViewModel<CategoryResponse>.Error("Category not found");
            }
            _mapper.Map(update, categoryDb);
            var result = await _repository.Update(categoryDb);
            var response = _mapper.Map<CategoryResponse>(result);
            return ResultViewModel<CategoryResponse>.Success(response);
        }
    }

}
