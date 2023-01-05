using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class StatusDTO : IdModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StatusId { get; set; }
}
