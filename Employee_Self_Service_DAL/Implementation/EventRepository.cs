using System.IO.Enumeration;
using Employee_Self_Service_BAL.Helpers;
using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;
using Employee_Self_Service_DAL.Models;
using Employee_Self_Service_DAL.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Implementation;

public class EventRepository : IEventRepository
{
    private readonly EmployeeSelfServiceContext _context;


    public EventRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }

    public async Task<EventPaginationViewModel> GetPaginatedEvent(int pageSize, int pageNumber, string searchQuery, string sortColumn, string sortDirection, string eventFromDate, string eventToDate, string eventCategory)
    {
        var query = _context.Events
                    .Include(e => e.Category)
                    .Where(e => !e.IsDeleted)
                    .Select(e => new EventDetailsViewModel 
                    {   
                        EventId = e.EventId,
                        EventName = e.Name,
                        StartDate = (DateOnly)e.StartDate,
                        EndDate = (DateOnly)e.EndDate,
                        CategoryId = (int)e.CategoryId,
                        CategoryName = e.Category.Category
                    });

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.EventName.ToLower().Contains(searchQuery));
        }
        
        if(!string.IsNullOrEmpty(eventFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(eventFromDate);
            query = query.Where(x => x.StartDate >= fromDate);
        }

        if(!string.IsNullOrEmpty(eventToDate))
        {
            DateOnly toDate = DateOnly.Parse(eventToDate);
            query = query.Where(x => x.StartDate <= toDate);
        }
        
        if(!string.IsNullOrEmpty(eventCategory) && !eventCategory.Equals("0"))
        {
            if (int.TryParse(eventCategory, out int categoryId))
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
        }

        query = sortColumn switch
        {
            "StartDate" => sortDirection == "asc" ? query.OrderBy(x => x.StartDate) : query.OrderByDescending(x => x.StartDate),
            "EndDate" => sortDirection == "asc" ? query.OrderBy(x => x.EndDate) : query.OrderByDescending(x => x.EndDate),
            "EventName" => sortDirection == "asc" ? query.OrderBy(x => x.EventName) : query.OrderByDescending(x => x.EventName),
            "CategoryName" => sortDirection == "asc" ? query.OrderBy(x => x.CategoryName) : query.OrderByDescending(x => x.CategoryName),
            _ => query.OrderBy(x => x.EventId)
        };

        var totalRecords = await query.CountAsync();

        var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

        EventPaginationViewModel model = new()
        {
            Page = new(),
            EventList = list
        };
        model.Page.SetPagination(totalRecords, pageSize, pageNumber);

        return model;
    }
   
    public async Task<List<EventCategory>> GetCategories()
    {
        return _context.EventCategories.ToList();
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
            
            document.Documents = $"/uploads/events/{fileName}";
            document.EventId = newEvent.EventId;
            document.IsActive = true;
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

    public async Task<ResponseViewModel> AddNotification (Notification addNotification)
    {   try{
        addNotification.CategoryId = (int?)((await _context.Events.OrderByDescending(e => e.EventId).FirstOrDefaultAsync())?.EventId);
        await _context.AddAsync(addNotification);
        await _context.SaveChangesAsync(); 

        
        List<Employee> employee = await _context.Employees.Include(u => u.Role).Where(u => u.RoleId == 2).ToListAsync();
        foreach (var user in employee)
        {
                
        NotificationMapping mapping = new NotificationMapping
        {
            NotificationId = addNotification.NotificationId,
            UserId = user.EmployeeId
        };
        await _context.AddAsync(mapping);
        await _context.SaveChangesAsync();
        }
        

        return new ResponseViewModel
        {
            success = true
        };
        }
        catch(Exception ex)
        {
            return new ResponseViewModel{
                success = false

            };
        }
        
    }

    public async Task<Event> GetEventDetails(long eventId)
    {
        Event? eventDetails = await _context.Events
                                    .Include(e => e.Documents.Where(d => d.IsActive == true))
                                    .Where(e => e.EventId == eventId )
                                    .FirstOrDefaultAsync();
        return eventDetails;
    }

    public async Task<ResponseViewModel> EditEvent(Event update, List<IFormFile> Documents)
    {
        try
        {
            _context.Events.Update(update);
            await _context.SaveChangesAsync();

            List<Document>? documents = await _context.Documents
                                .Where(d => d.EventId == update.EventId)
                                .ToListAsync();

            foreach (var doc in documents)
            {
                doc.IsActive = false;
                _context.Documents.Update(doc);
            }

            foreach (var file in Documents)
            {
                bool fileExistsInDatabase = false;

                foreach (var doc in documents)
                {
                    if (doc.Documents.Split('_').Last() == file.FileName)
                    {
                        doc.IsActive = true; 
                        _context.Documents.Update(doc);
                        fileExistsInDatabase = true;
                        break;
                    }
                }

                if (!fileExistsInDatabase)
                {
                    Document newDocument = new();
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

                    newDocument.Documents = $"/uploads/events/{fileName}";
                    newDocument.IsActive = true;
                    newDocument.EventId = update.EventId;
                    await _context.Documents.AddAsync(newDocument);
                }
            }
            await _context.SaveChangesAsync();

            return new ResponseViewModel
            {
                success = true,
                message = "Event updated successfully."
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel
            {
                success = false,
                message = "Error updating event: " + ex.Message
            };
        }
    }

    public async Task<byte []> GetEventDataToExport (int pageSize, int pageNumber, string searchQuery,string eventFromDate, string eventToDate, string eventCategory)
    {

        var query = _context.Events
                    .Include(e => e.Category)
                    .Where(e => !e.IsDeleted)
                    .Select(e => new EventDetailsViewModel 
                    {   
                        EventId = e.EventId,
                        EventName = e.Name,
                        StartDate = (DateOnly)e.StartDate,
                        EndDate = (DateOnly)e.EndDate,
                        CategoryId = (int)e.CategoryId,
                        CategoryName = e.Category.Category
                    });
        
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.EventName.ToLower().Contains(searchQuery));
        }
        
        if(!string.IsNullOrEmpty(eventFromDate))
        {
            DateOnly fromDate = DateOnly.Parse(eventFromDate);
            query = query.Where(x => x.StartDate >= fromDate);
        }

        if(!string.IsNullOrEmpty(eventToDate))
        {
            DateOnly toDate = DateOnly.Parse(eventToDate);
            query = query.Where(x => x.StartDate <= toDate);
        }
        
        if(!string.IsNullOrEmpty(eventCategory) && !eventCategory.Equals("All"))
        {
            
            query = query.Where(x => x.CategoryName == eventCategory);

        }

        var list = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    
                    .ToListAsync();

        List<EventDetailsViewModel> model = list;
        
        
        var excelExporter = new Excel.ExportExcel();
        return excelExporter.ExportToExcel(model, "LeaveRequest",string.IsNullOrEmpty(eventCategory) ? "All" : eventCategory,searchQuery);
    }

    
}
