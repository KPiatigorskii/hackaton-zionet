using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StatusId { get; set; }

    /*public virtual ICollection<Event> Events { get; } = new List<Event>();

    public virtual ICollection<Role> Roles { get; } = new List<Role>();

    public virtual ICollection<TaskCategory> TaskCategories { get; } = new List<TaskCategory>();

    public virtual ICollection<TaskParticipant> TaskParticipants { get; } = new List<TaskParticipant>();

    public virtual ICollection<Task> Tasks { get; } = new List<Task>();

    public virtual ICollection<TeamTask> TeamTasks { get; } = new List<TeamTask>();

    public virtual ICollection<Team> Teams { get; } = new List<Team>();

    public virtual ICollection<User> Users { get; } = new List<User>();*/
}
