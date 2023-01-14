using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class User : IdModel
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Github { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string? Password { get; set; }

    public int RoleId { get; set; }

    public string? TwitterUserId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Event> EventCreateUsers { get; } = new List<Event>();

    [JsonIgnore]
    public virtual ICollection<EventManager> EventManagers { get; } = new List<EventManager>();

    [JsonIgnore]
    public virtual ICollection<EventParticipantTeam> EventParticipantTeams { get; } = new List<EventParticipantTeam>();

    [JsonIgnore]
    public virtual ICollection<Event> EventUpdateUsers { get; } = new List<Event>();

    public virtual Role Role { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<TaskCategory> TaskCategoryCreateUsers { get; } = new List<TaskCategory>();

    [JsonIgnore]
    public virtual ICollection<TaskCategory> TaskCategoryUpdateUsers { get; } = new List<TaskCategory>();

    [JsonIgnore]
    public virtual ICollection<Task> TaskCreateUsers { get; } = new List<Task>();

    [JsonIgnore]
    public virtual ICollection<Task> TaskUpdateUsers { get; } = new List<Task>();

    [JsonIgnore]
    public virtual ICollection<Team> TeamCreateUsers { get; } = new List<Team>();

    [JsonIgnore]
    public virtual ICollection<Team> TeamUpdateUsers { get; } = new List<Team>();
}
