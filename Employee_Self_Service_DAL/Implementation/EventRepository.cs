using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;

namespace Employee_Self_Service_DAL.Implementation;

public class EventRepository : IEventRepository
{
    private readonly EmployeeSelfServiceContext _context;

    public EventRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    public async Task<ResponseViewModel> AddEvent(Event newEvent, List<IFormFile> Documents)
    {
        try
        {
            await _context.Events.AddAsync(newEvent);
            await _context.SaveChangesAsync();
            
            foreach (var file in Documents)
            {
            if (file.Length > 0)
            {
            Document document = new ();
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/events");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            
            document.Documents = $"/uploads/events{fileName}";
            document.EventId = newEvent.EventId;
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            }
            
            };
            
            

            return new ResponseViewModel
            {
                success = true,
                message = "Event added successfully"
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error occur while adding event:" + ex.Message
            };
        }
    }
}
