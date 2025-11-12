

using AutoMapper;
using Moq;
using VStore.ProductApi.Application.Dtos.Inputs;
using VStore.ProductApi.Application.Dtos.Responses;
using VStore.ProductApi.Application.Service;
using VStore.ProductApi.Domain.IRepository;
using VStore.ProductApi.Domain.Models;

namespace ProductApi.Tests.Application;

public class ProductServiceTests
{
    private readonly Mock<IRepository<Product>> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ProductService _productService;

    public ProductServiceTests()
    {
        _repositoryMock = new Mock<IRepository<Product>>();
        _mapperMock = new Mock<IMapper>();
        _productService = new ProductService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAll_WhenProductsExist_ShouldReturnProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product (id:1, name:"teste",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:1),
            new Product (id:2, name:"teste",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:2)

        };

        var productResponses = new List<ProductResponse>
        {
            new ProductResponse { Id = 1, Name = "Product 1", Price = 10.0m, Stock = 5 },
            new ProductResponse { Id = 2, Name = "Product 2", Price = 20.0m, Stock = 10 }
        };

        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync(products);
        _mapperMock.Setup(x => x.Map<List<ProductResponse>>(products)).Returns(productResponses);

        // Act
        var result = await _productService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
        _repositoryMock.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAll_WhenNoProducts_ShouldReturnEmptyList()
    {
        // Arrange
        var emptyProducts = new List<Product>();
        var emptyResponses = new List<ProductResponse>();

        _repositoryMock.Setup(x => x.GetAll()).ReturnsAsync(emptyProducts);
        _mapperMock.Setup(x => x.Map<List<ProductResponse>>(emptyProducts)).Returns(emptyResponses);

        // Act
        var result = await _productService.GetAll();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task FindById_WhenProductExists_ShouldReturnProduct()
    {
        // Arrange
        var productId = 1;
        var product = new Product(id: productId, name: "teste", price: 10, description: "teste", stock: 10, imageUrl: "teste", categoryId: 1);
        var productResponse = new ProductResponse { Id = productId, Name = "Product 1", Price = 10.0m, Stock = 5 };

        _repositoryMock.Setup(x => x.FindById(productId)).ReturnsAsync(product);
        _mapperMock.Setup(x => x.Map<ProductResponse>(product)).Returns(productResponse);

        // Act
        var result = await _productService.FindById(productId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(productId, result.Data.Id);
        Assert.Equal("Product 1", result.Data.Name);
        _repositoryMock.Verify(x => x.FindById(productId), Times.Once);
    }

    [Fact]
    public async Task FindById_WhenProductNotExists_ShouldReturnError()
    {
        // Arrange
        var productId = 999;
        _repositoryMock.Setup(x => x.FindById(productId)).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.FindById(productId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Product not found", result.Message);
        _repositoryMock.Verify(x => x.FindById(productId), Times.Once);
    }

    [Fact]
    public async Task GetProductsOrder_WithValidIds_ShouldReturnProducts()
    {
        // Arrange
        var ids = "1,2,3";
        var productIds = new List<int> { 1, 2, 3 };

        var products = new List<Product>
        {
            new Product (id:1, name:"teste",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:1),
            new Product (id:2, name:"teste",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:2)
        };

        _repositoryMock.Setup(x => x.GetProductsByIdsAsync(productIds)).ReturnsAsync(products);

        // Act
        var result = await _productService.GetProductsOrder(ids);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
        Assert.Equal("Product 1", result.Data[0].Name);
        Assert.Equal("Category 1", result.Data[0].CategoryName);
        _repositoryMock.Verify(x => x.GetProductsByIdsAsync(productIds), Times.Once);
    }

    [Fact]
    public async Task GetProductsOrder_WithDuplicateIds_ShouldReturnDistinctProducts()
    {
        // Arrange
        var ids = "1,1,2,2,3";
        var expectedIds = new List<int> { 1, 2, 3 }; // IDs distintos

        var products = new List<Product>
        {
            new Product (id:1, name:"teste",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:1),
            new Product (id:2, name:"teste",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:2)
        };

        _repositoryMock.Setup(x => x.GetProductsByIdsAsync(expectedIds)).ReturnsAsync(products);

        // Act
        var result = await _productService.GetProductsOrder(ids);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetProductsOrder_WithEmptyIds_ShouldReturnEmptyList()
    {
        // Arrange
        var ids = "";
        var expectedIds = new List<int>();

        _repositoryMock.Setup(x => x.GetProductsByIdsAsync(expectedIds)).ReturnsAsync(new List<Product>());

        // Act
        var result = await _productService.GetProductsOrder(ids);

        // Assert
        //await Assert.ThrowsAsync<FormatException>(() =>
        //_productService.GetProductsOrder(ids));
        Assert.Equal("Ids não encontrados", result.Message);
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
    }

    [Fact]
    public async Task FindByText_WithMatchingProducts_ShouldReturnProducts()
    {
        // Arrange
        var query = "laptop";
        var products = new List<Product>
        {
            new Product (id:1, name:"Gaming Laptop",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:1),
            new Product (id:2, name:"Ultrabook Laptop",  price:10, description:"teste",stock: 10,  imageUrl:"teste", categoryId:2)
        };

        var productResponses = new List<ProductResponse>
        {
            new ProductResponse { Id = 1, Name = "Gaming Laptop", Price = 1000.0m, Stock = 3 },
            new ProductResponse { Id = 2, Name = "Ultrabook Laptop", Price = 800.0m, Stock = 5 }
        };

        _repositoryMock.Setup(x => x.FindByText(query)).ReturnsAsync(products);
        _mapperMock.Setup(x => x.Map<List<ProductResponse>>(products)).Returns(productResponses);

        // Act
        var result = await _productService.FindByText(query);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
        _repositoryMock.Verify(x => x.FindByText(query), Times.Once);
    }

    [Fact]
    public async Task Create_WithValidProduct_ShouldReturnCreatedProduct()
    {
        // Arrange
        var productInput = new ProductInput(name: "Teste", price: 10, description: "Teste", stock: 10, imageUrl: "teste", categoryId: 2);


        var product = new Product(id: 1, name: "teste", price: 10, description: "teste", stock: 10, imageUrl: "teste", categoryId: 1);
        var productResponse = new ProductResponse { Id = 1, Name = "New Product", Price = 15.0m, Stock = 20 };

        _mapperMock.Setup(x => x.Map<Product>(productInput)).Returns(product);
        _repositoryMock.Setup(x => x.Create(product)).ReturnsAsync(product);
        _mapperMock.Setup(x => x.Map<ProductResponse>(product)).Returns(productResponse);

        // Act
        var result = await _productService.Create(productInput);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Data.Id);
        Assert.Equal("New Product", result.Data.Name);
        _repositoryMock.Verify(x => x.Create(product), Times.Once);
    }

    [Fact]
    public async Task Update_WhenProductExists_ShouldReturnUpdatedProduct()
    {
        // Arrange
        var productId = 1;
        var productInput = new ProductInput(name: "Teste", price: 10, description: "Teste", stock: 10, imageUrl: "teste", categoryId: 2);


        var existingProduct = new Product(id: productId, name: "teste", price: 10, description: "teste", stock: 10, imageUrl: "teste", categoryId: 1);
        var updatedProductResponse = new ProductResponse { Id = productId, Name = "Updated Product", Price = 25.0m, Stock = 15 };

        _repositoryMock.Setup(x => x.FindById(productId)).ReturnsAsync(existingProduct);
        _repositoryMock.Setup(x => x.Update(existingProduct)).ReturnsAsync(existingProduct);
        _mapperMock.Setup(x => x.Map<ProductResponse>(existingProduct)).Returns(updatedProductResponse);

        // Act
        var result = await _productService.Update(productId, productInput);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated Product", result.Data.Name);
        Assert.Equal(25.0m, result.Data.Price);
        _repositoryMock.Verify(x => x.FindById(productId), Times.Once);
        _repositoryMock.Verify(x => x.Update(existingProduct), Times.Once);
    }

    [Fact]
    public async Task Update_WhenProductNotExists_ShouldReturnError()
    {
        // Arrange
        var productId = 999;
        var productInput = new ProductInput(name: "Teste", price: 10, description: "Teste", stock: 10, imageUrl: "teste", categoryId: 2);

        _repositoryMock.Setup(x => x.FindById(productId)).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.Update(productId, productInput);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Product not found", result.Message);
        _repositoryMock.Verify(x => x.Update(It.IsAny<Product>()), Times.Never);
    }

    [Fact]
    public async Task Delete_WhenProductExists_ShouldReturnSuccess()
    {
        // Arrange
        var productId = 1;
        var product = new Product(id: productId, name: "teste", price: 10, description: "teste", stock: 10, imageUrl: "teste", categoryId: 1);

        _repositoryMock.Setup(x => x.FindById(productId)).ReturnsAsync(product);
        _repositoryMock.Setup(x => x.Delete(productId)).ReturnsAsync(true);

        // Act
        var result = await _productService.Delete(productId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Data);
        _repositoryMock.Verify(x => x.Delete(productId), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenProductNotExists_ShouldReturnError()
    {
        // Arrange
        var productId = 999;
        _repositoryMock.Setup(x => x.FindById(productId)).ReturnsAsync((Product)null);

        // Act
        var result = await _productService.Delete(productId);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Product not found", result.Message);
        _repositoryMock.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
    }
}