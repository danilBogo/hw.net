using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.Entities;

namespace TimMovie.Web.Controllers.Navbar;

public class UserController : Controller
{
    private readonly SignInManager<User> _signInManager;

    public UserController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Exit()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("MainPage", "MainPage");
    }

}