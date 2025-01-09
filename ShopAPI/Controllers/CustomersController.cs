using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;
using ShopApi.Web;
using Swashbuckle.AspNetCore.Annotations;

namespace ShopAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CustomerCreateDto> customerCreateDtoValidator;
        private readonly IValidator<CustomerUpdateDto> customerUpdateDtoValidator;

        public CustomersController(
            ICustomerService customerService,
            IValidator<CustomerCreateDto> customerCreateDtoValidator,
            IValidator<CustomerUpdateDto> customerUpdateDtoValidator)
        {
            _customerService = customerService;
            this.customerCreateDtoValidator = customerCreateDtoValidator;
            this.customerUpdateDtoValidator = customerUpdateDtoValidator;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all customers", Description = "Retrieves a list of all customers.")]
        [SwaggerResponse(200, "A list of customers", typeof(IEnumerable<CustomerDto>))]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a customer by ID", Description = "Retrieves a customer by their ID.")]
        [SwaggerResponse(200, "The customer with the specified ID", typeof(CustomerDto))]
        [SwaggerResponse(404, "Customer not found")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new customer", Description = "Creates a new customer with the provided data.")]
        [SwaggerResponse(201, "The created customer", typeof(CustomerDto))]
        [SwaggerResponse(400, "Invalid customer data")]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            var res = await customerCreateDtoValidator.ValidateAsync(customerDto);
            if (!res.IsValid) return BadRequest(res.ToValidationErrorResponse());

            var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPut()]
        [SwaggerOperation(Summary = "Updates an existing customer", Description = "Updates an existing customer with the provided data.")]
        [SwaggerResponse(200, "The updated customer", typeof(CustomerDto))]
        [SwaggerResponse(400, "Invalid customer data")]
        [SwaggerResponse(404, "Customer not found")]
        public async Task<IActionResult> UpdateCustomer([FromBody] CustomerUpdateDto customerDto)
        {
            var res = await customerUpdateDtoValidator.ValidateAsync(customerDto);
            if (!res.IsValid) return BadRequest(res.ToValidationErrorResponse());

            var updatedCustomer = await _customerService.UpdateCustomerAsync(customerDto);
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a customer by ID", Description = "Deletes a customer by their ID.")]
        [SwaggerResponse(204, "Customer deleted")]
        [SwaggerResponse(404, "Customer not found")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("birthday-celebrants")]
        [SwaggerOperation(Summary = "Gets customers with birthdays on the specified date", Description = "Retrieves a list of customers who have a birthday on the specified date.")]
        [SwaggerResponse(200, "A list of customers with birthdays on the specified date", typeof(IEnumerable<CustomerDto>))]
        public async Task<IActionResult> GetBirthdayCelebrants([FromQuery] DateOnly date)
        {
            var customers = await _customerService.GetBirthdayCelebrantsAsync(date);
            return Ok(customers);
        }

        [HttpGet("recent-buyers")]
        [SwaggerOperation(Summary = "Gets recent buyers", Description = "Retrieves a list of customers who made purchases in the last specified number of days.")]
        [SwaggerResponse(200, "A list of recent buyers", typeof(IEnumerable<CustomerDto>))]
        public async Task<IActionResult> GetRecentBuyers([FromQuery] int days)
        {
            var customers = await _customerService.GetRecentBuyersAsync(days);
            return Ok(customers);
        }

        [HttpGet("popular-categories")]
        [SwaggerOperation(Summary = "Gets popular product categories for a customer", Description = "Retrieves a list of popular product categories for a specified customer.")]
        [SwaggerResponse(200, "A list of popular product categories for the specified customer", typeof(IEnumerable<string>))]
        [SwaggerResponse(404, "Customer not found")]
        public async Task<IActionResult> GetPopularCategories([FromQuery] int customerId)
        {
            var customerExists = await _customerService.GetCustomerByIdAsync(customerId);
            if (customerExists == null) return NotFound();

            var categories = await _customerService.GetPopularCategoriesAsync(customerId);
            return Ok(categories);
        }
    }
}