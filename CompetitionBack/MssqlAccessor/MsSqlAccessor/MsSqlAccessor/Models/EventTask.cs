using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class EventTask
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TaskId { get; set; }

    public int StatusId { get; set; }

    public virtual Event Event { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<EventTaskEvaluateUser> EventTaskEvaluateUsers { get; } = new List<EventTaskEvaluateUser>();

    public virtual Task Task { get; set; } = null!;
}
