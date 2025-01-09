using Microsoft.EntityFrameworkCore;
using ShopApi.Core.Dtos;
using ShopApi.Core.Entities;
using ShopApi.Core.Interfaces;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ShopDbContext _context;

        public CustomerService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    DateOfBirth = c.DateOfBirth,
                    RegistrationDate = c.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                DateOfBirth = customer.DateOfBirth,
                RegistrationDate = customer.RegistrationDate
            };
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
        {
            var customer = new Customer
            {
                FullName = customerDto.FullName,
                DateOfBirth = customerDto.DateOfBirth,
                RegistrationDate = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customerDto.Id = customer.Id;
            return customerDto;
        }

        public async Task<CustomerDto?> UpdateCustomerAsync(int id, CustomerDto customerDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            customer.FullName = customerDto.FullName ?? customer.FullName;
            customer.DateOfBirth = customerDto.DateOfBirth;

            await _context.SaveChangesAsync();
            return customerDto;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomerDto>> GetBirthdayCelebrantsAsync(DateOnly date)
        {
            return await _context.Customers
                .Where(c => c.DateOfBirth.Month == date.Month && c.DateOfBirth.Day == date.Day)
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    FullName = c.FullName,
                    DateOfBirth = c.DateOfBirth,
                    RegistrationDate = c.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerDto>> GetRecentBuyersAsync(int days)
        {
            var recentDate = DateOnly.FromDateTime(DateTime.Now.AddDays(-days));
            return await _context.Purchases
                .Where(p => p.Date >= recentDate)
                .GroupBy(p => p.Customer)
                .Select(g => new CustomerDto
                {
                    Id = g.Key.Id,
                    FullName = g.Key.FullName,
                    DateOfBirth = g.Key.DateOfBirth,
                    RegistrationDate = g.Key.RegistrationDate
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetPopularCategoriesAsync(int customerId)
        {
            return await _context.PurchaseItems
                .Where(pi => pi.Purchase.CustomerId == customerId)
                .GroupBy(pi => pi.Product.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalUnits = g.Sum(pi => pi.Quantity)
                })
                .OrderByDescending(g => g.TotalUnits)
                .Select(g => g.Category)
                .ToListAsync();
        }
    }
}