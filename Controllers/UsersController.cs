using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmreSarıyer.Controllers;

public class UsersController : Controller{
    private readonly DataContext _context;
    public UsersController(DataContext context){
        _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1, int pageSize = 3){
        if(HttpContext.Session.GetString("UserRole") != "2"){
            return RedirectToAction("Index","");
        }
        var users = _context.User.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    
        var totalBlogs = _context.User.Count();
        var totalPages = (int)Math.Ceiling((double)totalBlogs / pageSize);

        ViewBag.UserCurrentPage = page;
        ViewBag.UserTotalPages = totalPages;
        return View("users", users);
    }
    [HttpPost]
    public async Task<IActionResult> UserAdminDelete(int Userid){
        var finduser = await _context.User.FindAsync(Userid);
        if(finduser == null){
            return NotFound();
        }
        _context.User.Remove(finduser);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index","users");
    }
}