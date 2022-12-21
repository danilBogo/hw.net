using Microsoft.AspNetCore.Identity;

namespace TimMovie.Infrastructure.Identity;

public class RussianErrorDescriber : IdentityErrorDescriber
{
    public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Возникла неизвестная ошибка. ОБратитесь в поддержку." }; }
    public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Ошибка параллелизации. Объект уже изменен." }; }
    public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Неверный пароль." }; }
    public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Неверный токен." }; }
    public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Пользователь с таким логином уже существует." }; }
    public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Имя пользователя '{userName}' не соответсвует правилам. Можно использовать только буквы и цифры." }; }
    public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Почта '{email}' неверная."  }; }
    public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Имя пользователя '{userName}' уже занято."  }; }
    public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Почта '{email}' уже занята."  }; }
    public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Неверное имя роли '{role}'."  }; }
    public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Имя роли '{role}' уже занято."  }; }
    public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "У пользователя уже установлен пароль." }; }
    public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "У пользователя не подключена блокировка при неверном пароле" }; }
    public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"Пользователь уже имеет роль '{role}'."  }; }
    public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"У пользователя отсутсвует роль '{role}'."  }; }
    public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Пароль должен содержать как минимум {length} символов."  }; }
    public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Пароль должно содержать как минимум 1 небуквенно-циферный символ" }; }
    public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Пароль должен содержать как минимум 1 цифру" }; }
    public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Пароль должен содержать как минимум 1 букву в нижнем регистре" }; }
    public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Пароль должен содержать как минимум 1 букву в верхнем регистре" }; }
}