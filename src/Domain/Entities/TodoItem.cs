namespace WebAppStarter.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using WebAppStarter.Domain.Common;

public class TodoItem : BaseAuditableEntity
{
    [Required]
    public Guid Owner { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;
}
