using TimMovie.Core.Classes;
using TimMovie.SharedKernel.Classes;

namespace TimMovie.Core.Interfaces;

public interface IMailService
{
    public Task<Result> SendMessageAsync(MessageMail message);
}