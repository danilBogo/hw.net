using TimMovie.Core.Classes;

namespace TimMovie.Core.Interfaces;

public interface IUserMessageService
{
    public MessageMail GenerateConfirmMessage(string userName, string address, string linkToConfirm);
    public MessageMail GenerateResetPasswordMessage(string userName, string address, string linkToConfirm);
}