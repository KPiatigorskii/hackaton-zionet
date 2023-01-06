using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class EventTaskDTO : IdModel
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TaskId { get; set; }

    public int StatusId { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
