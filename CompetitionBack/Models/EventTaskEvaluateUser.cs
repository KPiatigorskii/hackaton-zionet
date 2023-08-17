using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class EventTaskEvaluateUser : IdModel
{
    public int Id { get; set; }

    public int EventTaskId { get; set; }

    public int EvaluateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual EventManager EvaluateUser { get; set; } = null!;

    public virtual EventTask EventTask { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
