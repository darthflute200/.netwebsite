using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmreSarıyer.Controllers;

public class BlogcreateController : Controller{
    private readonly DataContext _context;
    public BlogcreateController(DataContext context){
        _context = context;
    }
    public IActionResult Index(){
        if(HttpContext.Session.GetString("UserRole") != "2"){
            TempData["ErrorMessage"] = "Bu sayfaya erişmek için giriş yapınız";
            return RedirectToAction("Index","Login");
        }
        return View("Blogcreate");
    }
    [HttpPost]
    public async Task<IActionResult> BlogCreate(BlogModel blog, IFormFile image){
        if(!ModelState.IsValid){
            var errorMessage = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
            TempData["ErrorMessage"] = errorMessage;
            return RedirectToAction("Index" , "Blogcreate");
        }
        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
        var filePath = Path.Combine(uploadDir, uniqueFileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        blog.Picture = "/images/" + uniqueFileName;
        _context.Blog.Add(blog);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "");
    }
    
}