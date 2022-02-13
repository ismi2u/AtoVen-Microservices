namespace EmailService.EmailObjects
{
    public interface IMailSender
    {
        void SendTextMail(string recepientEmail, string recepientName);
        void SendHTMLMail(string recepientEmail, string recepientName);
        void SendHTMLWithAttachment(string recepientEmail, string recepientName);
    }
}
