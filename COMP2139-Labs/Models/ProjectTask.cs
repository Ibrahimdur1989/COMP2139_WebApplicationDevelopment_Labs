using System.ComponentModel.DataAnnotations;

namespace COMP2139_Labs.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    //Foreign Key
    public int ProjectId { get; set; }
    
    //Navigation property
    //This allows for easy access to the related Project entity
    public Project? Project { get; set; }
    
    
    
    
    
}