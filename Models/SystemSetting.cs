using System;
using System.ComponentModel.DataAnnotations;

public class SystemSetting
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(100)]
    public string Key { get; set; }

    [Required, MaxLength(500)]
    public string Value { get; set; }

    [MaxLength(300)]
    public string Description { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
