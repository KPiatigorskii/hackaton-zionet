using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class EventParticipantTeamDTO : IdModel
{
    public new int Id { get; set; }

    public int ParticipantId { get; set; }

    public int EventId { get; set; }

    public int TeamId { get; set; }

    public bool IsLeader { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User Participant { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
