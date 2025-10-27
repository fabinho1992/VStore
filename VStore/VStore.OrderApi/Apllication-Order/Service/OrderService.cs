using AutoMapper;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using VStore.OrderApi.Apllication_Order.Dtos;
using VStore.OrderApi.Apllication_Order.Dtos.Inputs;
using VStore.OrderApi.Apllication_Order.Dtos.Response;
using VStore.OrderApi.Apllication_Order.ProductHttpClient;
using VStore.OrderApi.Domain.IRepository;
using VStore.OrderApi.Domain.IService;
using VStore.OrderApi.Domain.Models;
using VStore.OrderApi.Infrastructure.Repository;

namespace VStore.OrderApi.Apllication_Order.Service
{
    public class OrderService : ICRUDService<OrderResponse, OrderInput>
    {
        private readonly IRepositoryOrder<Order> _orderRepository;
        private readonly IHttpGetProducts _getProducts;
        private readonly IMapper _mapper;

        public OrderService(IRepositoryOrder<Order> repository, IHttpGetProducts getProducts, IMapper mapper)
        {
            _orderRepository = repository;
            _getProducts = getProducts;
            _mapper = mapper;
        }

        public async Task<ResultViewModel<OrderResponse>> Create(OrderInput create)
        {
            // 1. VALIDAR produtos e estoque
            var productIds = create.Items.Select(i => i.ProductId).Distinct().ToList();
            var products = await _getProducts.ProductsForOrder(productIds);

            // 2. Validar estoque de forma concisa
            var outOfStock = products
                .Where(p => p.Stock < create.Items
                    .First(i => i.ProductId == p.Id).Quantity)
                .Select(p => $"{p.Name} (stock: {p.Stock})")
                .ToList();

            if (outOfStock.Any())
            {
                return ResultViewModel<OrderResponse>.Error(
                    $"Insufficient stock for: {string.Join(", ", outOfStock)}");
            }

            // 3. Criar e salvar order
            var order = CreateOrderWithItems(create.CustomerId, create.Items, products);
            await _orderRepository.Create(order);

            var orderResponse = _mapper.Map<OrderResponse>(order);

            return ResultViewModel<OrderResponse>.Success(orderResponse);
        }

        public Task<ResultViewModel<bool>> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultViewModel<OrderResponse>> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultViewModel<List<OrderResponse>>> FindByText(string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ResultViewModel<List<OrderResponse>>> GetAll()
        {
            var orders = await _orderRepository.GetAll();
            var orderResponses = _mapper.Map<List<OrderResponse>>(orders);
            return ResultViewModel<List<OrderResponse>>.Success(orderResponses);
        }

        public Task<ResultViewModel<OrderResponse>> Update(int id, OrderInput update)
        {
            throw new NotImplementedException();
        }

        private Order CreateOrderWithItems(int customerId, List<OrderItemInput> items, List<ProductConsumerDto> products)
        {
            var order = new Order(customerId);

            foreach (var item in items)
            {
                var product = products.First(p => p.Id == item.ProductId);
                order.AddItem(item.ProductId, product.Name, item.Quantity, product.Price);
            }

            return order;
        }

    }
}
