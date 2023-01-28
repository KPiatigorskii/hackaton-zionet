using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZionetCompetition.Models;

public partial class EventTask
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TaskId { get; set; }

    public int StatusId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual TaskModel Task { get; set; } = null!;
}
