using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class TeamParticipant
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int ParticipantId { get; set; }

    public bool IsLeader { get; set; }

    public virtual User Participant { get; set; } = null!;

    public virtual ICollection<TaskParticipant> TaskParticipants { get; } = new List<TaskParticipant>();

    public virtual Team Team { get; set; } = null!;
}
