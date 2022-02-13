using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.EmailObjects
{
    public class MailSender : IMailSender
    {

        private readonly IServiceProvider _serviceProvider;

        public MailSender(IServiceProvider serviceProvider)
        {

            _serviceProvider = serviceProvider;
        }

        public async void SendTextMail(string recepientEmail, string recepientName)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

                var email = mailer
                    .To("ismailkhanf@gmail.com")
                    .Subject(" " + "Email")
                    .Body("First Plain text message");

                await email.SendAsync();
            }
        }


        public async void SendHTMLMail(string recepientEmail, string recepientName)
        {
           using (var scope = _serviceProvider.CreateScope())
            {
                var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();
                string mypath = $"{ Directory.GetCurrentDirectory()}/EmailTemplate/EmailTemplate.cshtml";
                var email = mailer
                    .To("ismailkhanf@gmail.com")
                    .Subject(" " + "Email")
                    .UsingTemplateFromFile(mypath,
                    new {
                        Firstname = "My Test First Name", 
                        Message = "My message body" 
                    });

                await email.SendAsync();
            }
        }

        public async void SendHTMLWithAttachment(string recepientEmail, string recepientName)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mailer = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

                var email = mailer
                    .To("ismailkhanf@gmail.com")
                    .Subject(" " + "Email")
                    .AttachFromFilename($"{ Directory.GetCurrentDirectory()}/EmailTemplate/EmailTemplate.cshtml","application/pdf", "Application Form")
                    .UsingTemplateFromFile($"{ Directory.GetCurrentDirectory()}/EmailTemplate/EmailTemplate.cshtml",
                    new
                    {
                        Firstname = "My Test First Name",
                        Message = "My message body"
                    });

                await email.SendAsync();
            }
        }

       
    }
}
