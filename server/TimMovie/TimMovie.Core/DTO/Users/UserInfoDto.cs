using TimMovie.Core.DTO.Films;
using TimMovie.Core.Entities.Enums;

namespace TimMovie.Core.DTO.Users;

public class UserInfoDto
{
    public UserStatusEnum StatusEnum { get; set; }
    public string DisplayName { get; set; }
    public string PathToPhoto { get; set; }
    public string CountryName { get; set; }
    public DateOnly? BirthDate { get; set; }
    public FilmForStatusDto? FilmForStatusDto { get; set; }
}