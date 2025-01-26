using COMP2139_Labs.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Labs.Data;

public class ApplicationDbContext : DbContext
{


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Project> Projects { get; set; }
    
    
}