using System.Collections.Generic;
using VStore.ProductApi.Application.Dtos;
using VStore.ProductApi.Application.Dtos.Responses;

namespace VStore.ProductApi.Domain.IService
{
    public interface ICRUDService<T, U> where T : class where U : class
    {
        Task<ResultViewModel<List<T>>> GetAll(); 
        Task<ResultViewModel<T>> FindById(int id); 
        Task<ResultViewModel<List<T>>> FindByText(string query);
        Task<ResultViewModel<T>> Create(U create);
        Task<ResultViewModel<T>> Update(int id, U update);
        Task<ResultViewModel<List<T>>>GetProductsOrder(string ids);
        Task<ResultViewModel<bool>> Delete(int id);
    }
}
