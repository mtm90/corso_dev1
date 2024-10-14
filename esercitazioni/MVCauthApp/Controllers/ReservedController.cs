using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MVCauthApp.Models;

namespace MVCauthApp.Controllers;

public class ReservedController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    [Authorize(Roles = "Admin")]
    public IActionResult Admin()
    {
        return View();
    }
    [Authorize(Roles = "User")]
    public IActionResult User()
    {
        return View();
    }
    
}

