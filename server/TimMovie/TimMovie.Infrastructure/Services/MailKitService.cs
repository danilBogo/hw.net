using MimeKit;
using TimMovie.Core.Classes;
using TimMovie.Core.Interfaces;
using TimMovie.SharedKernel.Classes;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TimMovie.Infrastructure.Services;

public class MailKitService : IMailService
{
    public MailKitService(MailSetup mailSetup)
    {
        this.mailSetup = mailSetup;
    }

    private readonly MailSetup mailSetup;

    public async Task<Result> SendMessageAsync(MessageMail message)
    {
        try
        {
            var mimeMessage = CreateMimeMessage(message);
            using var client = new SmtpClient();
            await client.ConnectAsync(mailSetup.Host, mailSetup.Port, true);
            await client.AuthenticateAsync(mailSetup.FromCompanyAddress, mailSetup.Password);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
            return Result.Ok();
        }
        catch (Exception e)
        {
            return Result.Fail(e.Message);
        }
    }
    private MimeMessage CreateMimeMessage(MessageMail message)
    {
        return new MimeMessage
        {
            From = { new MailboxAddress(mailSetup.FromCompanyName, mailSetup.FromCompanyAddress) },
            To = { MailboxAddress.Parse(message.Address) },
            Subject = message.Subject,
            Body = new BodyBuilder { HtmlBody = message.Content }.ToMessageBody()
        };
    }
}