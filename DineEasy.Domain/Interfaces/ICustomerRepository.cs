using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(long id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    void Update(Customer customer);
    void Delete(Customer customer);
    
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByPhoneNumberAsync(string phoneNumber);
}