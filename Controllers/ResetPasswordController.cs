using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace EmreSarıyer.Controllers;

public class ResetPasswordController : Controller{
    private readonly DataContext _context;

    public ResetPasswordController(DataContext context){
        _context = context;
    }
    [HttpGet]
    public IActionResult Index(){ 
        return View("ResetPassword");
    }
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordModel Newpasswordpost,string token, string userid){
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userid))
        {
            TempData["ErrorMessage"] = "Token veya kullanıcı ID boş.";
            return RedirectToAction("Index","Login");
        }
        int userId;
        if (!int.TryParse(userid, out userId))
        {
            TempData["ErrorMessage"] = "Geçersiz kullanıcı ID.";
            return RedirectToAction("Index","Login");
        }
        if(!ModelState.IsValid){
            TempData["ErrorMessage"] = "Şifreler aynı değil";
            return RedirectToAction("Index","resetpassword",token );
        }
        var user = await _context.User.FirstOrDefaultAsync(u => u.id == userId && u.ResetPasswordToken == token && u.ResetPasswordExpiration > DateTime.Now);
        if (user == null)
        {
            TempData["ErrorMessage"] = "Geçersiz token veya süresi dolmuş.";
            return RedirectToAction("Login");
        }
        user.Password = Newpasswordpost.newpassword;
        user.ResetPasswordToken = null;
        await _context.SaveChangesAsync();
        return RedirectToAction("Index","");
    }

}