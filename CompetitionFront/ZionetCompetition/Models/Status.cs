using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZionetCompetition.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StatusId { get; set; }
}
