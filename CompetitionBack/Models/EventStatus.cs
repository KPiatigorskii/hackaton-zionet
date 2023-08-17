using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class EventStatus : IdModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StatusId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Event> Events { get; } = new List<Event>();

    public virtual Status Status { get; set; } = null!;
}
