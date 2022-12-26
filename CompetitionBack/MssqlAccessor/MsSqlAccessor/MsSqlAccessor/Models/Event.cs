using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string DateTime { get; set; } = null!;

    public int NumberParticipantsInTeam { get; set; }

    public int NumberConcurrentTasks { get; set; }

    public string Hashcode { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual ICollection<EventManager> EventManagers { get; } = new List<EventManager>();

    public virtual ICollection<EventTask> EventTasks { get; } = new List<EventTask>();

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Team> Teams { get; } = new List<Team>();

    public virtual User UpdateUser { get; set; } = null!;
}
