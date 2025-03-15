using COMP2139_Labs.Data;
using COMP2139_Labs.Models;
using COMP2139_Labs.Areas.ProjectManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Areas.ProjectManagement.Controllers;

/// <summary>
/// Controller for managing projects in the application.
/// Provides CRUD operations (Create, Read, Update, Delete) for the Project entity.
/// </summary>
[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
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

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        //Databased --> Retrieve a list of projects from database 
        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }
    
    /// <summary>
    /// Displays the form to create a new project
    /// </summary>
    /// <returns>The Create view.</returns>
    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles the submission of the create form to add a new project
    /// </summary>
    /// <param name="project">The project data submitted by the user</param> 
    /// <returns>Redirects to Index if successful; otherwise, redisplays the form.</returns>
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]  // need to have it in every post request even in the Assignments  
    public async Task<IActionResult> Create(Project project)
    {
        // [ValidateAntiForgeryToken] helps prevent Cross-Site Request Forgery (CSRF) attacks 
        if (ModelState.IsValid) // Ensures the data is valid based on model annotations.
        {
            _context.Projects.Add(project);   // Adds the new project to the database. 
            await _context.SaveChangesAsync();           // Save the changes to the database
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
    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        // Retrieve the project with the specified ID or returns null if not found 
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
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
    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        // Finds the project by its primary key. 
        var project = await _context.Projects.FindAsync(id);
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
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]  
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
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
                await _context.SaveChangesAsync();      // Saves the changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handles concurrency issues where another process (or user) modifies the project simultaneously
                if (!await ProjectExists(project.ProjectId))
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
    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }

    /// <summary>
    /// Displays the confirmation page to delete a project.
    /// </summary>
    /// <param name="id">The ID of the project to delete.</param>
    /// <returns>The Delete view if the project exists; otherwise, NotFound</returns>
    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();     // 404 not found error
        }
        return View(project);
    }
    
    [HttpPost("Delete/{ProjectId:int}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    
    public async Task<IActionResult> DeleteConfirmed(int ProjectId)
    {
        // Retrieve the project with the specified ID or returns null if not found 
        var project = await _context.Projects.FindAsync(ProjectId);
        if (project != null)
        {
            _context.Projects.Remove(project); // Remove project from database 
            await _context.SaveChangesAsync();            // Commit changes to database 
            return RedirectToAction("Index");
        }
        return NotFound();
    }


    [HttpGet("Search/{searchString?}")]
    public async Task<IActionResult> Search(string searchString)
    {
        //Fetch all projects from the database as Queryable, this allows us to execute filters
        // before executing the database query.
        var projectsQuery = _context.Projects.AsQueryable();
        
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (searchPerformed)
        {
            // Convert searchString to Lowercase to make the search case-insensitive 
            searchString = searchString.ToLower();
            
            projectsQuery = projectsQuery.Where(p => p.Name.ToLower().Contains(searchString) || 
                                                p.Description.ToLower().Contains(searchString));
        }

        // Execute the query asynchronously using 'ToListAsync()'
        var projects = await projectsQuery.ToListAsync();

        // Store search metadata for the view
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;

        // Return the filtered list to the Index View (reusing existing UI)
        return View("Index", projects);

    }
    
    
}