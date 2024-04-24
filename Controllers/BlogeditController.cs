using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;

namespace EmreSarıyer.Controllers;

public class BlogeditController : Controller{
    private readonly DataContext _context;
    public BlogeditController(DataContext context){
        _context = context;
    }
    public IActionResult Index(int page = 1, int pageSize = 3){
        var blogs = _context.Blog.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    
        var totalBlogs = _context.Blog.Count();
        var totalPages = (int)Math.Ceiling((double)totalBlogs / pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        return View("blogedit",blogs);
    }
    public async Task<IActionResult> deleteblog(int blogid){
        var blog = await _context.Blog.FindAsync(blogid);
        if (blog == null)
        {
            return NotFound();
        }
        var imagePath = blog.Picture;
            
        if (!string.IsNullOrEmpty(imagePath))
        {
            var imagePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/'));
            if (System.IO.File.Exists(imagePathToDelete))
            {
                System.IO.File.Delete(imagePathToDelete);
            }
        }
        _context.Blog.Remove(blog);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Blog başarıyla silindi";
        return RedirectToAction("Index","blogedit");
    }
}