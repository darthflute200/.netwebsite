using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EmreSarıyer.Controllers;

public class SigninController : Controller {
    private readonly DataContext _context;
    public SigninController(DataContext context){
        _context = context;
    }
    public IActionResult Index(){
        return View("Signin");
    }
    [HttpPost]
    public async Task<IActionResult> Create(UserModel user){
    if (!ModelState.IsValid)
    {
        var errorMessage = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
        TempData["ErrorMessage"] = errorMessage;
        TempData["UserInput"] = JsonConvert.SerializeObject(user);
        return RedirectToAction("Index" , "Signin");
    }
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Üyeliğiniz oluşturuldu. Giriş yapabilirsiniz";
        return RedirectToAction("Index", "Login");
    }
}