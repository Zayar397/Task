using System;
using System.Collections.Generic;

namespace Task.Data.Models;

public partial class TaskItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public bool DeleteFlag { get; set; }
}
