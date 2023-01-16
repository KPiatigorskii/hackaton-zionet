﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ZionetCompetition.Models;

public partial class TaskModel
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? TaskBody { get; set; }

    public int CategoryId { get; set; }

    public string? Language { get; set; }

    public string? Platform { get; set; }

    public int? Duration { get; set; }

    public int? Points { get; set; }

    public bool? HasBonus { get; set; } = false!;

    public int? BonusExtraTime { get; set; }

    public int? BonusPoints { get; set; }

    public DateTime CreateDate { get; set; }

    public DateTime UpdateDate { get; set; }

    public int CreateUserId { get; set; }

    public int UpdateUserId { get; set; }

    public int StatusId { get; set; }

    public virtual TaskCategory Category { get; set; } = null!;

    public virtual User CreateUser { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual User UpdateUser { get; set; } = null!;

}
