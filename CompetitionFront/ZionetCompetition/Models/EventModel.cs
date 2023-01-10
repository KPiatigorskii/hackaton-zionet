using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using ZionetCompetition.Data;
using System.Linq;
using System.Reflection;

namespace ZionetCompetition.Models;

public partial class Event
{
    public int? Id { get; set; }

    [CustomAttribute("Details", true)]
    public string Title { get; set; } = null!;

    [CustomAttribute("Details", true)]
    public string? Address { get; set; }

    [CustomAttribute("Details", true)]
    public DateTime? StartTime { get; set; }
    
    [CustomAttribute("Details", true)]
    public DateTime? EndTime { get; set; }

    [CustomAttribute("Details", true)]
    public int? NumberParticipantsInTeam { get; set; }

    [CustomAttribute("Details", true)]
    public int? NumberConcurrentTasks { get; set; }

    [CustomAttribute("Details", true)]
    public string? Hashcode { get; set; }

    [CustomAttribute("Details", true)]
    public int EventStatusId { get; set; }

    [CustomAttribute("Details", true)]
    public bool IsEnable { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    [CustomAttribute("Details", true)]
    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual EventStatus EventStatus { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

}
