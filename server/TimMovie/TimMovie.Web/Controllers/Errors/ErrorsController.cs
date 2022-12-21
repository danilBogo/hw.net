using Microsoft.AspNetCore.Mvc;

namespace TimMovie.Web.Controllers.Errors;

public class ErrorsController : Controller
{
    public IActionResult PageForBannedUser()
    {
        return View();
    }
}

