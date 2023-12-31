﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class EventManager : IdModel
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int UserId { get; set; }

    public int StatusId { get; set; }

    public virtual Event Event { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<EventTaskEvaluateUser> EventTaskEvaluateUsers { get; } = new List<EventTaskEvaluateUser>();

    public virtual Status Status { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
