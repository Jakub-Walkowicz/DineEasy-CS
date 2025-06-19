using DineEasy.Domain.Entities;

namespace DineEasy.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<Client?> GetByIdAsync(int id);
    Task AddAsync(Client client);
    void Update(Client client);
    void Delete(Client client);
    
    Task<Client?> GetByEmailAsync(string email);
    Task<Client?> GetByPhoneNumberAsync(string phoneNumber);
}