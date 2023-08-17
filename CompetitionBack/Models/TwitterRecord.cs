using MsSqlAccessor.Models;
using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class TwitterRecord : IdModel
{
    public int Id { get; set; }

    public int AuthorId { get; set; }

    public int ParticipantId { get; set; }

    public string EventName { get; set; } = null!;

    public string TeamName { get; set; } = null!;

    public int EnginePort { get; set; }

    public string EngineCronUuid { get; set; }

    public bool IsSearching { get; set; }

    public bool AlreadyFound { get; set; }

    public int StatusId { get; set; }

    public virtual User Participant { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;
}
