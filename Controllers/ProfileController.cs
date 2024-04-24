using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EmreSarıyer.Controllers;

public class ProfileController : Controller{

        private readonly DataContext _context;
        private readonly IEmailSender _emailSender;

        public ProfileController(DataContext context,IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _context = context;
        }    
        public IActionResult Index()
        {
        if (HttpContext.Session.GetString("UserId") == null){
            TempData["ErrorMessage"] = "Bu sayfaya erişmek için giriş yapınız";
            return RedirectToAction("Index","Login");
        }
        else{
            int userId = HttpContext.Session.GetInt32("UserId").Value;
            var user = _context.User.FirstOrDefault(u => u.id == userId);
            if (user != null)
            {
                return View("Profile",user);
            }
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> ProfileUpdate(UserModel updatedUser){
        int userId = HttpContext.Session.GetInt32("UserId").Value;
        var user = await _context.User.FirstOrDefaultAsync(u => u.id == userId);   
        if (user != null)
        {
            user.Name = updatedUser.Name;
            user.Surname = updatedUser.Surname;
            user.email = updatedUser.email;
            _context.SaveChanges();
            return RedirectToAction("Index","");
        }

    TempData["ErrorMessage"] = "Lütfen bir daha deneyin.";
    return RedirectToAction("");
    }
    [HttpPost]
    public async Task<IActionResult> deleteUser(int Userid){
        var user = await _context.User.FindAsync(Userid);
        if(user == null){
            return NotFound();
        }
        _context.User.Remove(user);
        await _context.SaveChangesAsync();
        HttpContext.Session.Clear();
        return RedirectToAction("Index","");
    }
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email){
        var user = await _context.User.FirstOrDefaultAsync(u => u.email == email);
        if (user == null){
            TempData["ErrorMessage"] = "E-posta adresi bulunamadı.";
            return RedirectToAction("Login"); 
        }
        var token = Guid.NewGuid().ToString();
        var userid = user.id;
        user.ResetPasswordToken = token;
        user.ResetPasswordExpiration = DateTime.Now.AddHours(1);
        await _context.SaveChangesAsync();
        var resetLink = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Resetpassword?token={token}&userid={userid}";
        var message = $"Parolanızı sıfırlamak için <a href='{resetLink}'>buraya tıklayın</a>:";
        var subject = "Parola Sıfırlama";
        try
        {
            await _emailSender.SendEmailAsync(email, subject, message);
            TempData["SuccessMessage"] = "Email gönderildi lütfen kontrol ediniz.";
            return RedirectToAction("Index","Profile");
        }
        catch
        {
            TempData["ErrorMessage"] = "Hata oluştu. Lütfen tekrar deneyiniz.";
            return RedirectToAction("Index","Profile");
        }
    }
    
}