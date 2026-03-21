using Application.DTOs;
using Infrastructure.Identity;

namespace Application.Helpers;

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
