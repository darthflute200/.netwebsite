using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmreSarıyer.Controllers;

public class LoginController : Controller {
    private readonly DataContext _context;
    public LoginController(DataContext context){
        _context = context;
    }
    public IActionResult Index(){
        return View("Login");
    }
    [HttpPost]
    public async Task<IActionResult> Login(string email, string password){
        var user = await _context.User.FirstOrDefaultAsync(u => u.email == email && u.Password == password);
        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.id);
            HttpContext.Session.SetString("UserRole", user.UserRole);
            return RedirectToAction("Index", ""); 
        }
        else{
            TempData["ErrorMessage"] = "E-posta veya parola yanlış.";
            return RedirectToAction("Index","Login");
        }
    }
}