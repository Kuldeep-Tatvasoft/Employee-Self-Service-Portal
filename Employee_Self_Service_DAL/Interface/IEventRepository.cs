using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_DAL.Interface;

public interface IEventRepository
{
    Task<ResponseViewModel> AddEvent(Event newEvent, List<IFormFile> Documents);
    
}
