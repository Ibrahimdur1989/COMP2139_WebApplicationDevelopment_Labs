using COMP2139_Labs.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_Labs.Controllers;

public class ProjectController : Controller
{
    /// <summary>
    /// Index action will retrieve a listing of projects (Database)
    /// </summary>
    /// <returns></returns>

    [HttpGet]
    public IActionResult Index()
    {
        //Databased --> Retrieve all project from database
        var projects = new List<Project>()
        {
            new Project { ProjectId = 1, Name = "Project 1", Description = "First Project 1" }
            //Feel free to add more projects here
        };
        return View(projects);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        //Databased --> Persist new project to the database
        return RedirectToAction("Index");
    }
    
    //CRUD - Create - Read - Update - Delete

    [HttpGet]
    public IActionResult Details(int id)
    {
        //Databased --> Retrieve project from database
        var project = new Project { ProjectId = id, Name = "Project 1" + id, Description = "Details of Project" + id };
        return View(project);
    }
    
}