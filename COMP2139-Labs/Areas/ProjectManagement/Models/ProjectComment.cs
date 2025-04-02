using System.ComponentModel.DataAnnotations;

namespace COMP2139_Labs.Areas.ProjectManagement.Models;

public class ProjectComment
{
    
    public int ProjectCommentId { get; set; }
    
    [Display(Name = "Project Message")]
    [StringLength(500, ErrorMessage = "Project Message cannot be longer than 500 characters.")]
    public string? Content { get; set; }

    [DataType(DataType.DateTime)] [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    private DateTime _datePosted;
    public DateTime DatePosted
    {
        get => _datePosted; 
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    //Foreign Key to Parent Project
    public int ProjectId { get; set; }
    
    // Navigation Property
    public Project? Project { get; set; }
    
    
    
}