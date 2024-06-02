using System.Net;
using System.Net.Mail;

namespace MailServer
{
    public class Program
    {
        public record MailData(string User, string Password, string Recipient, string Subject, string Body);
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.MapPost("/sendMail", (MailData data) =>
            {
                if(data == null)
                    throw new BadHttpRequestException("Dados incorretos.");

                if (String.IsNullOrEmpty(data.User) || String.IsNullOrEmpty(data.Password) ||
                String.IsNullOrEmpty(data.Recipient) || String.IsNullOrEmpty(data.Subject) ||
                String.IsNullOrEmpty(data.Body)) 
                {
                    throw new BadHttpRequestException("Dados nulos ou vazios");
                }
                
                var user = data.User;
                var pass = data.Password;
                var recipient = data.Recipient;
                var subject = data.Subject;
                var body = data.Body;
                try
                {
                    var smtpClient = new SmtpClient("smtp.office365.com", 587)
                    {
                        Credentials = new NetworkCredential(user, pass),
                        EnableSsl = true
                    };

                    smtpClient.Send((string)user, (string)recipient, (string)subject, (string)body);
                }
                catch (Exception)
                {
                    throw new Exception("Algo errado aconteceu ao enviar seu email, verifique as informações e tente novamente!");
                }

            });

            app.Run();
        }
    }
}
