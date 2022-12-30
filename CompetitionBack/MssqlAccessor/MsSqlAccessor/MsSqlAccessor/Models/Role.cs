using System;
using System.Collections.Generic;

namespace MsSqlAccessor.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    /// <summary>
    /// GETDATE()
    /// </summary>
    public DateTime CreateDate { get; set; }

    /// <summary>
    /// GETDATE()
    /// </summary>
    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }
/*
    public virtual User CreateUser { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

    public virtual ICollection<User> Users { get; } = new List<User>();*/
}
