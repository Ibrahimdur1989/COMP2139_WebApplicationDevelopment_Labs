using COMP2139_Labs.Data;
using COMP2139_Labs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Controllers;

/// <summary>
/// Controller for managing projects in the application.
/// Provides CRUD operations (Create, Read, Update, Delete) for the Project entity.
/// </summary>

public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;
    
    /// <summary>
    /// Constructor that initializes the controller with a database context. 
    /// </summary>
    /// <param name="context">The database context for accessing project data.</param>
    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Display a list of all projects.
    /// </summary>
    /// <returns>The Index view with a list of projects.</returns>

    [HttpGet]
    public IActionResult Index()
    {
        //Databased --> Retrieve a list of projects from database 
        var projects = _context.Projects.ToList();
        return View(projects);
    }
    
    /// <summary>
    /// Displays the form to create a new project
    /// </summary>
    /// <returns>The Create view.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles the submission of the create form to add a new project
    /// </summary>
    /// <param name="project">The project data submitted by the user</param> 
    /// <returns>Redirects to Index if successful; otherwise, redisplays the form.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]  // need to have it in every post request even in the Assignments  
    public IActionResult Create(Project project)
    {
        // [ValidateAntiForgeryToken] helps prevent Cross-Site Request Forgery (CSRF) attacks 
        if (ModelState.IsValid) // Ensures the data is valid based on model annotations.
        {
            _context.Projects.Add(project);   // Adds the new project to the database. 
            _context.SaveChanges();           // Save the changes to the database
            return RedirectToAction("Index"); // Redirects to the Index action
        }
        return View(project);   // Redisplays the form with validation errors.
    }
    
    //CRUD - Create - Read - Update - Delete

    /// <summary>
    /// Displays the details of a specific project.
    /// </summary>
    /// <param name="id">The ID of the project to view</param>
    /// <returns>The Details view if the project exists; otherwise, NotFound.</returns>
    [HttpGet]
    public IActionResult Details(int id)
    {
        // Retrieve the project with the specified ID or returns null if not found 
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();     // Returns a 404 error if the project doesn't exist.
        }
        return View(project);
    }

    /// <summary>
    /// Displays the form to edit an existing project.
    /// </summary>
    /// <param name="id">The ID of the project to edit</param>
    /// <returns>The edit view if the project exists; otherwise, NotFound</returns>
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // Finds the project by its primary key. 
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();     // Returns a 404 error if the project doesn't exist.
        }
        return View(project);
    }

    
    /// <summary>
    /// Handles the submission of the Edit form to update a project.
    /// </summary>
    /// <param name="id">The ID of the project being edited</param>
    /// <param name="project">The updated project data submitted by the user.</param>
    /// <returns>Redirects to Index if successful; otherwise, redisplays the form.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]  
    public IActionResult Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
    {
        // [Bind] ensures only the specified properties are updated for security reasons.
        if (id != project.ProjectId)
        {
            return NotFound();  // Ensures the ID in the route matches the ID in the model.
        } 

        if (ModelState.IsValid)
        {
            try
            {
                _context.Projects.Update(project);    // Update the project in the database.
                _context.SaveChanges();      // Saves the changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handles concurrency issues where another process (or user) modifies the project simultaneously
                if (!ProjectExists(project.ProjectId))
                {
                  return NotFound();  // Returns a 404 error if the project no longer exists.
                }
                else
                {
                    throw;     // Re-throws the exception if the issue is unknown
                }
            }
            return RedirectToAction("Index");
        }
        return View(project);    // re-display the form with validation errors 
        
    }

    
    /// <summary>
    /// Checks if a project exists in the database.
    /// </summary>
    /// <param name="id">The ID of the project to check.</param>
    /// <returns>True if the project exists; otherwise, false.</returns>
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }

    /// <summary>
    /// Displays the confirmation page to delete a project.
    /// </summary>
    /// <param name="id">The ID of the project to delete.</param>
    /// <returns>The Delete view if the project exists; otherwise, NotFound</returns>
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
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    
    public IActionResult DeleteConfirmed(int ProjectId)
    {
        // Retrieve the project with the specified ID or returns null if not found 
        var project = _context.Projects.Find(ProjectId);
        if (project != null)
        {
            _context.Projects.Remove(project); // Remove project from database 
            _context.SaveChanges();            // Commit changes to database 
            return RedirectToAction("Index");
        }
        return View(project);
    }
    
    
}