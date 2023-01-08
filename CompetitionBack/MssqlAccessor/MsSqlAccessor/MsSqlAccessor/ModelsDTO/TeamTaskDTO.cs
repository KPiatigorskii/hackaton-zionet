using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class TeamTaskDTO : IdModel
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int TaskId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int TaskStatus { get; set; }

    public int? TaskScore { get; set; }

    public int StatusId { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;

    public virtual TaskStatus TaskStatusNavigation { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
