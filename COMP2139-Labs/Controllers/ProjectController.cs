using COMP2139_Labs.Data;
using COMP2139_Labs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Controllers;

public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;
    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Index action will retrieve a listing of projects (Database)
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult Index()
    {
        //Databased --> Retrieve a list of projects from database 
        var projects = _context.Projects.ToList();
        return View(projects);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]  // need to have it in every post request even in the Assignments  
    public IActionResult Create(Project project)
    {
        if (ModelState.IsValid)
        {
            _context.Projects.Add(project);   // Project added to database (memory)
            _context.SaveChanges();           // Persists project to the database
            return RedirectToAction("Index");
        }
        return View(project);
    }
    
    //CRUD - Create - Read - Update - Delete

    [HttpGet]
    public IActionResult Details(int id)
    {
        // Retrieve the project with the specified ID or returns null if not found 
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();     // 404 not found error
        }
        return View(project);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Retrieve the project with the specified ID or returns null if not found 
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();     // 404 not found error
        }
        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]  
    public IActionResult Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
    {
        // [Bind] ensures only the specified properties are updated (security)
        if (id != project.ProjectId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);    // Update the project with new values
                _context.SaveChanges();      // Commit the changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                //Handles concurrency issues where another process (or user) modifies the project simultaneously
                if (!ProjectExists(project.ProjectId))
                {
                  return NotFound();  
                }
                else
                {
                    throw;     // Returns the original exception back to the caller
                }
            }
            return RedirectToAction("Index");
        }
        return View(project);    // re-display the form with validation errors 
        
    }

    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();     // 404 not found error
        }
        return View(project);
    }
    
}