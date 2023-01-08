using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZionetCompetition.Models;

public partial class EventParticipantTeam
{
    public int Id { get; set; }

    public int ParticipantId { get; set; }

    public int EventId { get; set; }

    public int TeamId { get; set; }

    public bool IsLeader { get; set; }

    public virtual EventModel Event { get; set; } = null!;

    public virtual User Participant { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
