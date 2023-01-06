using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZionetCompetition.Data;

namespace ZionetCompetition.Models;

public partial class TeamParticipant
{
    [CustomAttribute("Details", true)]
    public int Id { get; set; }
    [CustomAttribute("Details", true)]
    public int TeamId { get; set; }
    [CustomAttribute("Details", true)]
    public int ParticipantId { get; set; }
    [CustomAttribute("Details", true)]
    public bool IsLeader { get; set; }

    public virtual User Participant { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TaskParticipant> TaskParticipants { get; } = new List<TaskParticipant>();

}
