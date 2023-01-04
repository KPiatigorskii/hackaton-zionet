﻿using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class TeamTask
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int TaskId { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int StatusId { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;

    public virtual ICollection<TaskParticipant> TaskParticipants { get; } = new List<TaskParticipant>();

    public virtual Team Team { get; set; } = null!;
}
