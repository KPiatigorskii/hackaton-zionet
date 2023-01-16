using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class EventParticipantTeamDTO : IdModel
{
    public int Id { get; set; }

    public int ParticipantId { get; set; }

    public int EventId { get; set; }

    public int? TeamId { get; set; }

    public bool? IsLeader { get; set; }

    public bool? IsTwitt { get; set; }

    public int StatusId { get; set; }

	public bool IsActive { get; set; } = false;

	public bool IsApplied { get; set; } = false;

	public virtual Event Event { get; set; } = null!;

    public virtual User Participant { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual Team? Team { get; set; }
}
