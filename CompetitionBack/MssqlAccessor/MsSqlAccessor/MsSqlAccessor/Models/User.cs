using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string? Github { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual ICollection<Event> EventCreateUsers { get; } = new List<Event>();

    public virtual ICollection<EventManager> EventManagers { get; } = new List<EventManager>();

    public virtual ICollection<Event> EventUpdateUsers { get; } = new List<Event>();

    public virtual ICollection<User> InverseCreateUser { get; } = new List<User>();

    public virtual ICollection<User> InverseUpdateUser { get; } = new List<User>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Role> RoleCreateUsers { get; } = new List<Role>();

    public virtual ICollection<Role> RoleUpdateUsers { get; } = new List<Role>();

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<TaskCategory> TaskCategoryCreateUsers { get; } = new List<TaskCategory>();

    public virtual ICollection<TaskCategory> TaskCategoryUpdateUsers { get; } = new List<TaskCategory>();

    public virtual ICollection<Task> TaskCreateUsers { get; } = new List<Task>();

    public virtual ICollection<Task> TaskUpdateUsers { get; } = new List<Task>();

    public virtual ICollection<Team> TeamCreateUsers { get; } = new List<Team>();

    public virtual ICollection<TeamParticipant> TeamParticipants { get; } = new List<TeamParticipant>();

    public virtual ICollection<Team> TeamUpdateUsers { get; } = new List<Team>();

    public virtual User UpdateUser { get; set; } = null!;
}
