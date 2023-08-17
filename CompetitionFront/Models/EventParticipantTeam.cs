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

    public int? TeamId { get; set; }

    public bool IsLeader { get; set; }

	public bool IsTwitt { get; set; }

    public int StatusId { get; set; }

	public bool IsActive { get; set; }

	public bool IsApplied { get; set; }

	public virtual Event Event { get; set; } = null!;

    public virtual User Participant { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual Team? Team { get; set; }
}
