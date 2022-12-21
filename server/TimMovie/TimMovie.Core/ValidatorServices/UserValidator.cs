using TimMovie.Core.DTO.Users;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.ValidatorServices;

public class UserValidator
{
    private static readonly DateTime MinBirthDate = new DateTime(1920, 1, 1);
    
    public Result ValidateUserInfo(ShortUserInfoDto userInfo)
    {
        if (string.IsNullOrEmpty(userInfo.DisplayName))
        {
            return Result.Fail("Никнейм не может быть пустым");
        }

        var birthDate = userInfo.BirthDate;
        if (birthDate is not null && (MinBirthDate > birthDate.Value || birthDate.Value > DateTime.Now))
        {
            return Result.Fail("Дата рождения не может быть больше н.в.");
        }
        
        return Result.Ok();
    }
}