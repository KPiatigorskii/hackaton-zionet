﻿using System;
using System.Collections.Generic;

namespace ZionetCompetition.Models;

public partial class TaskParticipant
{
    public int Id { get; set; }

    public int TeamTaskId { get; set; }

    public int ParticipantUserId { get; set; }

    public int StatusId { get; set; }

    public virtual EventParticipantTeam ParticipantUser { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual TeamTask TeamTask { get; set; } = null!;
}
