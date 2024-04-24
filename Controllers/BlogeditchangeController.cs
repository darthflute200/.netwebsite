using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EmreSarıyer.Controllers;

public class BlogeditchangeController : Controller{
    private readonly DataContext _context;
    public BlogeditchangeController(DataContext context){
        _context = context;
    }
    [HttpGet("Blogeditchange/{blogid}")]
    public async Task<IActionResult> Getblog(int blogid){
        var blog = await _context.Blog.FindAsync(blogid);
        if(blog == null){
            TempData["ErrorMessage"] = "Blog bulunamadı";
            return View("blogeditchange");
        }
        return View("blogeditchange",blog);
    }
    [HttpPost]
    public async Task<IActionResult> Blogupdate(BlogModel blog, IFormFile? Newimage){
        var Currentblog = await _context.Blog.FindAsync(blog.Blogid);
        if (Currentblog == null)
        {
            return NotFound();
        }
        Currentblog.BlogTitle = blog.BlogTitle;
        Currentblog.BlogSubtitle = blog.BlogSubtitle;
        Currentblog.Text = blog.Text;
        Currentblog.Youtubelink = blog.Youtubelink;
        if(Newimage != null){
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Newimage.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Currentblog.Picture.TrimStart('/'))))
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Currentblog.Picture.TrimStart('/')));
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await Newimage.CopyToAsync(stream);
            }
            Currentblog.Picture = "/" + Path.Combine("images", uniqueFileName);
        }
        _context.SaveChanges();
        TempData["SuccessMessage"] = "Değişiklikler kaydedildi";
        return RedirectToAction("Index","Blogedit");
    }
}