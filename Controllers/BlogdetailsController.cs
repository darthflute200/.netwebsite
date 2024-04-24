using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EmreSarıyer.Controllers;
public class BlogdetailsController : Controller{
    private readonly DataContext _context;
    public BlogdetailsController(DataContext context){
        _context = context;
    }
    [HttpGet("blogdetails/{blogid}")]
    public async Task<IActionResult> BlogGet(int blogid){
        var blog = await _context.Blog.FindAsync(blogid);
        BlogdetailModel viewModel = new BlogdetailModel
        {
            Blog = blog
        };
        if (HttpContext.Session.GetInt32("UserId") != null){
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _context.User.FindAsync(userId);
        if (user != null)
        {
            viewModel.User = user;
        }
    }
    var comments = await _context.Comment.Where(c => c.Blogid == blogid).ToListAsync();

    if (comments.Any())
    {
        foreach (var comment in comments)
    {
        var user = await _context.User.FindAsync(comment.Userid);
        if (user != null)
        {
            viewModel.commentUsers.Add(user);
        }
    }
        viewModel.Comments = comments;
    }
        return View("blogdetails",viewModel);
    }
    [HttpPost]
    public async Task<IActionResult> CommentPost(CommentModel newcomment){
        _context.Comment.Add(newcomment);
        await _context.SaveChangesAsync();
        return RedirectToAction("BlogGet", "Blogdetails", new { blogid = newcomment.Blogid });
    }
    [HttpPost]
    public async Task<IActionResult> CommentDelete(int commentid){
        var comment = await _context.Comment.FindAsync(commentid);
    
        if(comment == null){
            return NotFound(); 
        }

        _context.Comment.Remove(comment);
        await _context.SaveChangesAsync();
    
        return RedirectToAction("BlogGet", "Blogdetails", new { blogid = comment.Blogid });
    }
}