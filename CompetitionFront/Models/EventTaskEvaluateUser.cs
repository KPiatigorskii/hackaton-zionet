﻿using System;
using System.Collections.Generic;

namespace ZionetCompetition.Models;

public partial class EventTaskEvaluateUser
{
    public int Id { get; set; }

    public int EventTaskId { get; set; }

    public int EvaluateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual EventManager EvaluateUser { get; set; } = null!;

    public virtual EventTask EventTask { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}