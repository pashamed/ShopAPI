using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Core.Dtos;
using ShopApi.Core.Interfaces;

namespace ShopAPI.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CustomerDto> validator;

        public CustomerController(ICustomerService customerService, IValidator<CustomerDto> validator)
        {
            _customerService = customerService;
            this.validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            var res = await validator.ValidateAsync(customerDto);
            if (!res.IsValid) return BadRequest(res.Errors);
            var createdCustomer = await _customerService.CreateCustomerAsync(customerDto);
            return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, createdCustomer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDto customerDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updatedCustomer = await _customerService.UpdateCustomerAsync(id, customerDto);
            if (updatedCustomer == null) return NotFound();
            return Ok(updatedCustomer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpGet("birthday-celebrants")]
        public async Task<IActionResult> GetBirthdayCelebrants([FromQuery] DateOnly date)
        {
            var customers = await _customerService.GetBirthdayCelebrantsAsync(date);
            return Ok(customers);
        }

        [HttpGet("recent-buyers")]
        public async Task<IActionResult> GetRecentBuyers([FromQuery] int days)
        {
            var customers = await _customerService.GetRecentBuyersAsync(days);
            return Ok(customers);
        }

        [HttpGet("popular-categories")]
        public async Task<IActionResult> GetPopularCategories([FromQuery] int customerId)
        {
            var customerExists = await _customerService.GetCustomerByIdAsync(customerId);
            if (customerExists == null) return NotFound("Invalid Customer Id");
            var categories = await _customerService.GetPopularCategoriesAsync(customerId);
            return Ok(categories);
        }
    }
}
