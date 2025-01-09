using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopAPI.Web.Controllers;

namespace ShopApi.Tests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly Mock<IValidator<CustomerCreateDto>> _customerCreateDtoValidatorMock;
        private readonly Mock<IValidator<CustomerUpdateDto>> _customerUpdateDtoValidatorMock;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _customerCreateDtoValidatorMock = new Mock<IValidator<CustomerCreateDto>>();
            _customerUpdateDtoValidatorMock = new Mock<IValidator<CustomerUpdateDto>>();
            _controller = new CustomersController(
                _customerServiceMock.Object,
                _customerCreateDtoValidatorMock.Object,
                _customerUpdateDtoValidatorMock.Object);
        }

        [Fact]
        public async Task GetAllCustomers_ReturnsOkResult_WithListOfCustomers()
        {
            // Arrange
            var customers = new List<CustomerDto>
            {
                new CustomerDto { Id = 1, FullName = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1), RegistrationDate = new DateOnly(2022, 1, 1) },
                new CustomerDto { Id = 2, FullName = "Jane Smith", DateOfBirth = new DateOnly(1985, 5, 15), RegistrationDate = new DateOnly(2022, 1, 1) }
            };
            _customerServiceMock.Setup(service => service.GetAllCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.GetAllCustomers();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<CustomerDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsOkResult_WithCustomer()
        {
            // Arrange
            var customer = new CustomerDto { Id = 1, FullName = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1), RegistrationDate = new DateOnly(2022, 1, 1) };
            _customerServiceMock.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync(customer);

            // Act
            var result = await _controller.GetCustomerById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsNotFoundResult_WhenCustomerDoesNotExist()
        {
            // Arrange
            _customerServiceMock.Setup(service => service.GetCustomerByIdAsync(1)).ReturnsAsync((CustomerDto)null);

            // Act
            var result = await _controller.GetCustomerById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCreatedAtActionResult_WithCreatedCustomer()
        {
            // Arrange
            var customerCreateDto = new CustomerCreateDto { FullName = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1) };
            var createdCustomer = new CustomerDto { Id = 1, FullName = "John Doe", DateOfBirth = new DateOnly(1990, 1, 1), RegistrationDate = new DateOnly(2022, 1, 1) };
            _customerCreateDtoValidatorMock.Setup(v => v.ValidateAsync(customerCreateDto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _customerServiceMock.Setup(service => service.CreateCustomerAsync(customerCreateDto)).ReturnsAsync(createdCustomer);

            // Act
            var result = await _controller.CreateCustomer(customerCreateDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CustomerDto>(createdAtActionResult.Value);
            Assert.Equal(1, returnValue.Id);
        }

        [Fact]
        public async Task UpdateCustomer_ReturnsOkResult_WithUpdatedCustomer()
        {
            // Arrange
            var customerUpdateDto = new CustomerUpdateDto { Id = 1, FullName = "John Doe Updated", DateOfBirth = new DateOnly(1990, 1, 1) };
            var updatedCustomer = new CustomerDto { Id = 1, FullName = "John Doe Updated", DateOfBirth = new DateOnly(1990, 1, 1), RegistrationDate = new DateOnly(2022, 1, 1) };
            _customerUpdateDtoValidatorMock.Setup(v => v.ValidateAsync(customerUpdateDto, default)).ReturnsAsync(new FluentValidation.Results.ValidationResult());
            _customerServiceMock.Setup(service => service.UpdateCustomerAsync(customerUpdateDto)).ReturnsAsync(updatedCustomer);

            // Act
            var result = await _controller.UpdateCustomer(customerUpdateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<CustomerDto>(okResult.Value);
            Assert.Equal("John Doe Updated", returnValue.FullName);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNoContentResult_WhenCustomerIsDeleted()
        {
            // Arrange
            _customerServiceMock.Setup(service => service.DeleteCustomerAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCustomer_ReturnsNotFoundResult_WhenCustomerDoesNotExist()
        {
            // Arrange
            _customerServiceMock.Setup(service => service.DeleteCustomerAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteCustomer(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}