using System;

using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Core.Email
{
    public class EmailService
    {
        public async Task sendEmailAsync(EmailDto request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("justF0rt3sts@outlook.com"));
            email.To.Add(MailboxAddress.Parse(request.To));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Plain) { Text = request.Body };

            var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTlsWhenAvailable);
            await smtp.AuthenticateAsync("justF0rt3sts@outlook.com", "oparola123");
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
