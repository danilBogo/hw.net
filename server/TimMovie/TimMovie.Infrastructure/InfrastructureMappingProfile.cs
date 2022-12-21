using AutoMapper;
using TimMovie.Core.DTO.Users;
using TimMovie.Core.Entities;

namespace TimMovie.Infrastructure;

public class InfrastructureMappingProfile: Profile
{
    public InfrastructureMappingProfile()
    {
        CreateMap<User, UserInfoDto>()
            .ForMember(
                dto => dto.StatusEnum,
                expression => expression.MapFrom(user => user.Status.UserStatusEnum));
        CreateMap<User, UserInfoForChatDto>();
    }
}