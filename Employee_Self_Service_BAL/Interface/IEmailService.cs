using Employee_Self_Service_DAL.ViewModel;

namespace Employee_Self_Service_BAL.Interface;

public interface IEmailService
{
    Task<ResponseViewModel> SendEmailAsync(string toEmail, string subject, string message);
}
