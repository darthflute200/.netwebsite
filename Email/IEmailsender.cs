using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string message);
}

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _emailSettings;

    public EmailSender(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using (var client = new SmtpClient())
        {
            var credentials = new NetworkCredential
            {
                UserName = _emailSettings.SmtpUsername,
                Password = _emailSettings.SmtpPassword
            };

            client.Credentials = credentials;
            client.Host = _emailSettings.SmtpServer;
            client.Port = _emailSettings.SmtpPort;
            client.EnableSsl = true;

            var mail = new MailMessage
            {
                From = new MailAddress(_emailSettings.SmtpUsername, "Game Pulse Hub")
            };

            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;

            await client.SendMailAsync(mail);
        }
    }
}

public class EmailSettings
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; }
    public string SmtpPassword { get; set; }
}
