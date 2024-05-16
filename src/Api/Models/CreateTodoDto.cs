namespace TodoApi.Models;

using System.ComponentModel.DataAnnotations;

public class CreateToDoDto
{
    public Guid? Owner { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;
}