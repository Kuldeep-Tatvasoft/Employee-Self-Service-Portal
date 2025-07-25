using Employee_Self_Service_BAL.Interface;
using Employee_Self_Service_DAL.Interface;

namespace Employee_Self_Service_BAL.Implementation;

public class DocumentService : IDocumentService
{
    private readonly IDocumentRepository _documentRepository;

    public DocumentService(IDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }
}
