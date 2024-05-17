namespace WebAppStarter.Domain.Entities;

using System;
using System.ComponentModel.DataAnnotations;
using WebAppStarter.Domain.Common;

// TODO: inherit from BaseAuditableEntity
public class TodoItem : BaseEntity
{
    [Required]
    public Guid Owner { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;
}