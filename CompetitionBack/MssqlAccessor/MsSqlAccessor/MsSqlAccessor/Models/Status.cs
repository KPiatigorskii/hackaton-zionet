using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    
    public int StatusId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Event> Events { get; } = new List<Event>();

    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; } = new List<Role>();

    [JsonIgnore]
    public virtual ICollection<TaskCategory> TaskCategories { get; } = new List<TaskCategory>();

    [JsonIgnore]
    public virtual ICollection<TaskParticipant> TaskParticipants { get; } = new List<TaskParticipant>();

    [JsonIgnore]
    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    [JsonIgnore]
    public virtual ICollection<TeamTask> TeamTasks { get; } = new List<TeamTask>();

    [JsonIgnore]
    public virtual ICollection<Team> Teams { get; } = new List<Team>();
    [JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
