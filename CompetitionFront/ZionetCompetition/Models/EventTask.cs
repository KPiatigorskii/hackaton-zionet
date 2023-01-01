
using System;
using System.Collections.Generic;

namespace ZionetCompetition.Models;

public partial class EventTask
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int TaskId { get; set; }

    public int StatusId { get; set; }

    /*    public virtual Event Event { get; set; } = null!;

        public virtual EventTaskEvaluateUser? EventTaskEvaluateUser { get; set; }

        public virtual Task Task { get; set; } = null!;*/
}
