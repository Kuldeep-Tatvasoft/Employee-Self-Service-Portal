namespace Employee_Self_Service_BAL.Interface;

public interface IEmailService
{
    Task SendEmailAsync(string toemail, string subject, string message);
}
