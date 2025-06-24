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
                    .Select(e => new AddEventViewModel 
                    {   
                        EventId = e.EventId,
                        EventName = e.Name,
                        EventDescription = e.Description,
                        StartDate = (DateOnly)e.StartDate,
                        EndDate = (DateOnly)e.EndDate,
                        CategoryId = (int)e.CategoryId,
                        CategoryName = e.Category.Category
                        
                    });

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.ToLower();
            query = query.Where(c => c.EventName.ToString().Contains(searchQuery));
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

        // query = sortColumn switch
        // {
        //     "StartDate" => sortDirection == "asc" ? query.OrderBy(x => x.StartDate) : query.OrderByDescending(x => x.StartDate),
        //     "EndDate" => sortDirection == "asc" ? query.OrderBy(x => x.EndDate) : query.OrderByDescending(x => x.EndDate),
        //     "ActualDuration" => sortDirection == "asc" ? query.OrderBy(x => x.ActualDuration) : query.OrderByDescending(x => x.ActualDuration),
        //     "TotalDuration" => sortDirection == "asc" ? query.OrderBy(x => x.TotalDuration) : query.OrderByDescending(x => x.TotalDuration),
        //     "ReturnDate" => sortDirection == "asc" ? query.OrderBy(x => x.ReturnDate) : query.OrderByDescending(x => x.ReturnDate),
        //     "AvailableOnPhone" => sortDirection == "asc" ? query.OrderBy(x => x.AvailableOnPhone) : query.OrderByDescending(x => x.AvailableOnPhone),
        //     "ApprovedDate" => sortDirection == "asc" ? query.OrderBy(x => x.ApprovedDate) : query.OrderByDescending(x => x.ApprovedDate),
        //     "ApprovedBy" => sortDirection == "asc" ? query.OrderBy(x => x.ApprovedBy) : query.OrderByDescending(x => x.ApprovedBy),
        //     "Status" => sortDirection == "asc" ? query.OrderBy(x => x.Status) : query.OrderByDescending(x => x.Status),
        //     "AdhocLeave" => sortDirection == "asc" ? query.OrderBy(x => x.AdhocLeave) : query.OrderByDescending(x => x.AdhocLeave),
        //     _ => query.OrderBy(x => x.LeaveRequestId)
        // };

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

    public async Task<Event> GetEventDetails(long eventId)
    {
        Event? eventDetails = await _context.Events.FirstOrDefaultAsync(u => u.EventId == eventId );
        
        if (eventDetails == null)
        {
            return null;
        }
        return eventDetails;
    }

    public async Task<List<Document>> GetEventDocuments(long eventId)
    {
        List<Document>? documents = await _context.Documents
                                    .Where(d => d.EventId == eventId)
                                    .ToListAsync();
        
        return documents;
    }
}
