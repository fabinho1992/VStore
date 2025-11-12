using AutoMapper;
using UserApi.Application.Dtos;
using UserApi.Domain.Interfaces.IRepository;
using UserApi.Domain.Interfaces.IService;
using VStore.OrderApi.Apllication_Order.Dtos;

namespace UserApi.Application.Services
{
    public class UserService : ICRUD
    {
        private readonly IRepositoryUser _repositoryUser;
        private readonly IMapper _mapper;

        public UserService( IRepositoryUser repositoryUser, IMapper mapper)
        {
            _repositoryUser = repositoryUser;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<bool>> DeleteAsync(string email)
        {
            var user = await _repositoryUser.GetByEmail(email);
            if (user is null)
            {
                return ResultViewModel<bool>.Error("User not found");
            }
            await _repositoryUser.Delete(user);
            return ResultViewModel<bool>.Success(true);
        }

        public async Task<ResultViewModel<List<UserResponse>>> GetAllAsync()
        {
            var users = await _repositoryUser.GetAll();
            if (users is null || !users.Any())
            {
               return ResultViewModel<List<UserResponse>>.Error("No users found");
            }
            var userResponses = _mapper.Map<List<UserResponse>>(users);
            return ResultViewModel<List<UserResponse>>.Success(userResponses);
        }

        public async Task<ResultViewModel<UserResponse>> GetByEmail(string email)
        {
            var user = await _repositoryUser.GetByEmail(email);
            if (user is null)
            {
                return ResultViewModel<UserResponse>.Error("User not found");
            }

            var userResponse = _mapper.Map<UserResponse>(user);
            return ResultViewModel<UserResponse>.Success(userResponse);
        }

        public async Task<ResultViewModel<UserResponse>> GetByIdAsync(Guid id)
        {
            var user = await _repositoryUser.GetById(id);
            if (user is null)
            {
                return ResultViewModel<UserResponse>.Error("User not found");
            }
            var userResponse = _mapper.Map<UserResponse>(user);
            return ResultViewModel<UserResponse>.Success(userResponse);
        }
    }
}
