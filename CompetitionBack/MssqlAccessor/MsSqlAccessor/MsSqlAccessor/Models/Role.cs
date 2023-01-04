using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int StatusId { get; set; }

    public virtual Status Status { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<User> Users { get; } = new List<User>();
}
