using API.Contracts;
using System.Net.Mail;

namespace API.Utilities.Handler;

public class EmailHandler : IEmailHandler
{
    private readonly string _smtpServer;
    private readonly int _smptPortServer;
    private readonly string _fromemailAddress;

    public EmailHandler(string smtpServer, int smptPortServer, string fromemailAddress)
    {
        _smtpServer = smtpServer;
        _smptPortServer = smptPortServer;
        _fromemailAddress = fromemailAddress;
    }

    public void SendEmail(string toEmail, string subject, string htmlMessage)
    {
        var message = new MailMessage
        {
            From = new MailAddress(_fromemailAddress),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = true
        };

        message.To.Add(new MailAddress(toEmail));

        using var client = new SmtpClient(_smtpServer, _smptPortServer);
        client.Send(message);
    }
}
