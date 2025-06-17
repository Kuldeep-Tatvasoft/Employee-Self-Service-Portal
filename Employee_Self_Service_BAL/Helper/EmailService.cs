using System.Net;
using System.Net.Mail;
using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.Extensions.Configuration;

namespace Employee_Self_Service_BAL.Helper;

public class EmailService : IEmailService
{
    private readonly string? _Host;
    private readonly int _port;
    private readonly string? _UserName;
    private readonly string? _Password;
    private readonly string? _FromEmail;
    private readonly string? _FromName;
    private readonly bool _EnableSsl;

    public EmailService(IConfiguration configuration)
    {   
         _Host =  configuration["SmtpSettings:Host"];
        _port = int.Parse(configuration["SmtpSettings:port"]);
        _UserName = configuration["SmtpSettings:UserName"];
        _Password = configuration["SmtpSettings:Password"];
        _FromEmail = configuration["SmtpSettings:FromEmail"];
        _FromName = configuration["SmtpSettings:FromName"];
        _EnableSsl = bool.Parse(configuration["SmtpSettings:EnableSsl"]);
    }
    public async Task<ResponseViewModel> SendEmailAsync(string toEmail, string subject, string message)
    {  
       try{
        var smtpClient = new SmtpClient(_Host, _port)
       {
           Credentials = new NetworkCredential(_UserName, _Password),
           EnableSsl = _EnableSsl
       };

       var mailMessage = new MailMessage
        {
            From = new MailAddress( _FromEmail, _FromName),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);
        await smtpClient.SendMailAsync(mailMessage);
        return new ResponseViewModel{
            success = true,
            message = "Email send successfully"
        };
       }
       catch(Exception ex)
       {
            return new ResponseViewModel{
                success = false,
                message = "Failed to sent Email"
            };
       }
       
    }
}