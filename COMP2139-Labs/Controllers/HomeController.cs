using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP2139_Labs.Models;

namespace COMP2139_Labs.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    // Lab 6 - General Search for Projects or ProjectTasks 
    // Redirects users to the appropriate search function
    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        // Ensure searchType is not null and handle case--insensitivity 
        searchType = searchType?.Trim().ToLower();

        // Ensure the search string is not empty 
        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            // Redirect back to home if the search is empty
            return RedirectToAction(nameof(Index), "Home");   
        }

        // Determine where to redirect based on search type 
        if (searchType == "project")
        {
            // Redirect to Project search
            return RedirectToAction("Search", "Project", new { searchString });
        }    
        else if (searchType == "tasks")
        {
            // Redirect to ProjectTask search
            return RedirectToAction("Search", "ProjectTask", new { searchString });
                
        }
        
        return RedirectToAction("Index", "Home");
        
    }
}