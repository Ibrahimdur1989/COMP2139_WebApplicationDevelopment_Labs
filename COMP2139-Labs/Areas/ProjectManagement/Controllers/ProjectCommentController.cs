using COMP2139_Labs.Areas.ProjectManagement.Models;
using COMP2139_Labs.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Areas.ProjectManagement.Controllers;


[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectCommentController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectCommentController(ApplicationDbContext context)
    {
        _context = context;
    }



    [HttpGet]
    public async Task<IActionResult> GetComments(int projectId)
    {
        var comments = await _context.ProjectComments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToListAsync();
        
        //return the comments as a JSON response
        return Json(comments);
    }

    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
    {

        if (ModelState.IsValid)
        {
            //Set the current data and time for the DatePosted property ( date received )
            comment.DatePosted = DateTime.Now;
            
            //save the ProjectComment to the database
            await _context.ProjectComments.AddAsync(comment);
            
            //commit the ProjectComment to the database
            await _context.SaveChangesAsync();
            
            //return a success message in JSON format
            return Json(new{success = true, message = "Comment added successfully"});
            
        }
        
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        return Json(new{success = false, message = "Invalid comment data", errors = errors});
        
        
        
    }
    
    
    
    
}