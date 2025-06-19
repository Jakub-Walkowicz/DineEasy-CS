// Application/Extensions/UserExtensions.cs
using DineEasy.Application.DTOs.User;
using DineEasy.Domain.Entities;

namespace DineEasy.Application.Extensions;

public static class UserExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt,
        };
    }

    public static IEnumerable<UserDto> ToDtos(this IEnumerable<User> users)
    {
        return users.Select(user => user.ToDto());
    }

    public static UserDetailsDto ToDetailsDto(this User user)
    {
        return new UserDetailsDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            CreatedAt = user.CreatedAt,
            DateOfBirth = user.UserProfile?.DateOfBirth,
            FirstName = user.UserProfile?.FirstName,
            LastName = user.UserProfile?.LastName,
            PhoneNumber = user.UserProfile?.PhoneNumber,
        };
    }
}