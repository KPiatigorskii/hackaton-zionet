using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZionetCompetition.Models;

public partial class EventStatus
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StatusId { get; set; }

    public virtual Status Status { get; set; } = null!;
}
