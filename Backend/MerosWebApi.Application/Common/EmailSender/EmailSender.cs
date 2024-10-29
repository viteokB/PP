using MerosWebApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace MerosWebApi.Application.Common.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailConfiguration _configuration;

        private readonly MailboxAddress myAddress;

        public EmailSender(IEmailConfiguration configuration)
        {
            _configuration = configuration;
            myAddress = MailboxAddress.Parse(configuration.EmailAddress);
        }

        public async Task<bool> SendAsync(string toEmail, string subject, string htmlContent)
        {
            var email = new MimeMessage();
            email.From.Add(myAddress);
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = htmlContent };

            using var smtp = new SmtpClient();
            try
            {
                await smtp.ConnectAsync(_configuration.EmailHost, _configuration.EmailHostPort, 
                    SecureSocketOptions.StartTls);

                await smtp.AuthenticateAsync(_configuration.EmailAddress, _configuration.Password);

                var response = await smtp.SendAsync(email);

                if (response == null)
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Здесь можно добавить логирование ошибки, если необходимо
                return false; // В случае исключения возвращаем false
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }

            return true; // Если всё прошло успешно
        }
    }
}
