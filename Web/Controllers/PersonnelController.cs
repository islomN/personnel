using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class PersonnelController(IPersonnelService personnelService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var items = await personnelService.Select(HttpContext.RequestAborted);
        ViewData["items"] = items;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file)
    {
        var items = await personnelService.Import(
            file,
            HttpContext.RequestAborted);
        return Ok(items);
    }
}