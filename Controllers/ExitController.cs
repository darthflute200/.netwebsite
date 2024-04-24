using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EmreSarıyer.Models;

namespace EmreSarıyer.Controllers;

public class ExitController : Controller{
    public IActionResult Index(){
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "");
    }
}