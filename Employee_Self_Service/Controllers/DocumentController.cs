using Employee_Self_Service_BAL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Self_Service.Controllers;

public class DocumentController : Controller
{
    private readonly IDocumentService _documentService;

    public DocumentController(IDocumentService documentService)
    {
        _documentService = documentService;
    }

}
