using Employee_Self_Service_DAL.Data;
using Employee_Self_Service_DAL.Interface;

namespace Employee_Self_Service_DAL.Implementation;

public class DocumentRepository : IDocumentRepository
{
    private readonly EmployeeSelfServiceContext _context;
    public DocumentRepository(EmployeeSelfServiceContext context)
    {
        _context = context;
    }
}
