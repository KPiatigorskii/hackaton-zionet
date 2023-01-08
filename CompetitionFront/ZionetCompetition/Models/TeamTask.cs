using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZionetCompetition.Models;

public partial class TeamTask
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

    public virtual TaskModel Task { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TaskParticipant> TaskParticipants { get; } = new List<TaskParticipant>();

}
