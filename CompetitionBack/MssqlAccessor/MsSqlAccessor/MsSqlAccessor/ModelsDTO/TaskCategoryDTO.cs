using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MsSqlAccessor.Models;

public partial class TaskCategoryDTO : IdModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Color { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual User CreateUser { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;
}
