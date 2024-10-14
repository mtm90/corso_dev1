using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MVCauthApp.Models;

[Authorize]
public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public AccountController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    // Adds the current user to the Admin role
    public async Task<IActionResult> AddToRoleAdmin()
    {
        var userName = User?.Identity?.Name; // Check if User.Identity.Name is null
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
        }
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> AddToRoleUser()
    {
        var userName = User?.Identity?.Name; // Check if User.Identity.Name is null
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
        }
        return RedirectToAction("Index", "Home");
    }

    // Gets the roles of the current user
    public async Task<IActionResult> GetRole()
    {
        var userName = User?.Identity?.Name; // Check if User.Identity.Name is null
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                return Content(string.Join(",", roles));
            }
        }
        return Content("User not found");
    }

    // Removes the current user from the Admin role
    public async Task<IActionResult> RemoveFromRoleAdmin()
    {
        var userName = User?.Identity?.Name; // Check if User.Identity.Name is null
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "Admin"); // Correct method
            }
        }
        return RedirectToAction("Index", "Home");
    }
    public async Task<IActionResult> RemoveFromRoleUser()
    {
        var userName = User?.Identity?.Name; // Check if User.Identity.Name is null
        if (userName != null)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                await _userManager.RemoveFromRoleAsync(user, "User"); // Correct method
            }
        }
        return RedirectToAction("Index", "Home");
    }
}
