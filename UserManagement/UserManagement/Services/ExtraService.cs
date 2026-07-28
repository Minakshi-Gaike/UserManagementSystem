using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Drawing;
using UserManagement.Models;

namespace UserManagement.Services
{
    public class ExtraService : IExtraService
    {
        EmailSetting _settings;
        public ExtraService(IOptions<EmailSetting>settings)//it is used to bind appsettings.json file to EmailSetting model and its used togecther in program.cs
        {
            _settings = settings.Value;
        }
        public async Task<string> GenerateOTP(int size)
        {
            string data = "0123456789";
            string otp = "";
            Random r = new Random();
            for (int i=0;i<size;i++)
            {
                otp += data[r.Next(0, data.Length-1)];
            }
            return otp;
        }

       

        public async Task<string> GeneratePassword(int size)
        {
            string data = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            string password = "";
            Random r = new Random();
            for (int i = 1; i <= size; i++)
            {
                password += data[r.Next(0, data.Length - 1)];
            }
            return password;
        }

        

        public async Task SendEmail(EmailModel m)
        {
            MailboxAddress fromMail = new MailboxAddress(_settings.UserName, _settings.EmailId);
            MailboxAddress tomail = new MailboxAddress(m.UserName, m.EmailId);
            MimeMessage message = new MimeMessage();
            message.From.Add(fromMail);
            message.To.Add(tomail);
            message.Subject = m.Subject;
            BodyBuilder body = new BodyBuilder();
            body.HtmlBody = m.Message;
            message.Body = body.ToMessageBody();
            SmtpClient client = new SmtpClient();
            client.Connect(_settings.Host, _settings.port, _settings.UseSSL);
            client.Authenticate(_settings.EmailId,_settings.Password);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}
