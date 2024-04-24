using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;

namespace EmreSarıyer.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;
    public HomeController(DataContext context){
        _context = context;
    }
    public IActionResult Index(int page = 1, int pageSize = 3)
    {
        var blogs = _context.Blog.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    
        var totalBlogs = _context.Blog.Count();
        var totalPages = (int)Math.Ceiling((double)totalBlogs / pageSize);

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        return View(blogs);
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
