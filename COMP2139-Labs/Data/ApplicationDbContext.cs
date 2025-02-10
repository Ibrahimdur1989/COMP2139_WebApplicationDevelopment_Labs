using COMP2139_Labs.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Data;

public class ApplicationDbContext : DbContext
{


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Define One-To-Many relationship between Project and ProjectTask
        modelBuilder.Entity<Project>()
            .HasMany(p => p.Tasks)                 // One project has (potentially) many Tasks
            .WithOne(t => t.Project)            // Each ProjectTask belongs to on Project
            .HasForeignKey(t => t.ProjectId)   // Foreign Key in ProjectTask table
            .OnDelete(DeleteBehavior.Cascade);           // Cascade delete ProjectTasks when Project is deleted
        
        
        //Seed the database
        modelBuilder.Entity<Project>().HasData(
            new Project { ProjectId = 1, Name = "Assignment 1", Description = "Comp2139 - Assignment 1" },
            new Project { ProjectId = 2, Name = "Assignment 2", Description = "Comp2139 - Assignment 2" }
        );
        
    }
    
    
}