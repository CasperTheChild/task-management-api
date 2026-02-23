using Application.DTOs;
using Infrastructure.Identity;

namespace Infrastructure.Helpers;

public static class UserMapper
{
    public static UserSummaryModel ToModel(ApplicationUser entity)
    {
        return new UserSummaryModel
        {
            Id = entity.Id,
            Email = entity.Email!,
        };
    }
}
