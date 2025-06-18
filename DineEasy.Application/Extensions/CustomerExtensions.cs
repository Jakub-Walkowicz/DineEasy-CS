using DineEasy.Application.DTOs.Customer;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Extensions;

public static class CustomerExtensions
{
    public static CustomerDto ToDto(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            CreatedAt = customer.CreatedAt,
        };
    }
    
    public static Customer ToEntity(this CustomerDto customer)
    {
        return new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }
    
    public static Customer ToEntity(this CreateCustomerDto customer)
    {
        return new Customer
        {
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber
        };
    }
    
}