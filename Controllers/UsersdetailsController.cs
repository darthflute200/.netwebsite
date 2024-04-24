using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmreSarıyer.Controllers;

public class UserdetailsController : Controller{
    private readonly DataContext _context;
    public UserdetailsController(DataContext context){
        _context = context;
    }
    [HttpGet("usersdetails/{userid}")]
    public async Task<IActionResult> Index(int userid){
        if(HttpContext.Session.GetString("UserRole") != "2"){
            return RedirectToAction("Index","");
        }
        var finduser = await _context.User.FindAsync(userid);
        if(finduser == null){
            return NotFound();
        }
        return View("usersdetails",finduser);
    }
    [HttpPost("usersdetails/{userid}")]
    public async Task<IActionResult> UserUpdate(UserModel updatedUser){

        var user = await _context.User.FirstOrDefaultAsync(u => u.id == updatedUser.id);   
        if (user != null)
        {
            user.Name = updatedUser.Name;
            user.Surname = updatedUser.Surname;
            user.email = updatedUser.email;
            await _context.SaveChangesAsync(); 
            return RedirectToAction("Index","users");
        }

        TempData["ErrorMessage"] = "Lütfen bir daha deneyin.";
        return RedirectToAction("Index", "users", new { userid = updatedUser.id });
    }

}