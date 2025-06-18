using DineEasy.Application.DTOs.Customer;

namespace DineEasy.Application.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
}