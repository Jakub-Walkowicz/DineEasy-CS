using DineEasy.Application.DTOs.Customer;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Extensions;

public static class CustomerExtensions
{
    public static ClientDto ToDto(this Client client)
    {
        return new ClientDto
        {
            Id = client.Id,
            FirstName = client.FirstName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber,
            CreatedAt = client.CreatedAt,
        };
    }
    
    public static Client ToEntity(this ClientDto client)
    {
        return new Client
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber
        };
    }
    
    public static Client ToEntity(this CreateClientDto client)
    {
        return new Client
        {
            FirstName = client.FirstName,
            LastName = client.LastName,
            PhoneNumber = client.PhoneNumber
        };
    }
    
}