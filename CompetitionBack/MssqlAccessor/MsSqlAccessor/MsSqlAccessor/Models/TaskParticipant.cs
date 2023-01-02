using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class TaskParticipant
{
    public int TeamTaskId { get; set; }

    public int ParticipantUserId { get; set; }

    public int StatusId { get; set; }

    public virtual TeamParticipant ParticipantUser { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual TeamTask TeamTask { get; set; } = null!;
}
