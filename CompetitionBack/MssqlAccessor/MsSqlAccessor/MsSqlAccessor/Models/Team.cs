using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class Team
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int EventId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<EventParticipantTeam> EventParticipantTeams { get; } = new List<EventParticipantTeam>();

    public virtual Status Status { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<TeamTask> TeamTasks { get; } = new List<TeamTask>();

    public virtual User UpdateUser { get; set; } = null!;
}
