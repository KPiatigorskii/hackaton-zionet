using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MsSqlAccessor.Models;

public partial class CompetitionBdTestContext : DbContext
{
    public CompetitionBdTestContext()
    {
    }

    public CompetitionBdTestContext(DbContextOptions<CompetitionBdTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventManager> EventManagers { get; set; }

    public virtual DbSet<EventParticipantTeam> EventParticipantTeams { get; set; }

    public virtual DbSet<EventStatus> EventStatuses { get; set; }

    public virtual DbSet<EventTask> EventTasks { get; set; }

    public virtual DbSet<EventTaskEvaluateUser> EventTaskEvaluateUsers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TaskCategory> TaskCategories { get; set; }

    public virtual DbSet<TaskParticipant> TaskParticipants { get; set; }

    public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamTask> TeamTasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=CompetitionBD-test;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("event");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.EventStatusId).HasColumnName("event_status_id");
            entity.Property(e => e.Hashcode)
                .HasMaxLength(200)
                .HasColumnName("hashcode");
            entity.Property(e => e.IsEnable).HasColumnName("is_enable");
            entity.Property(e => e.NumberConcurrentTasks).HasColumnName("number_concurrent_tasks");
            entity.Property(e => e.NumberParticipantsInTeam).HasColumnName("number_participants_in_team");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.EventCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_user_create");

            entity.HasOne(d => d.EventStatus).WithMany(p => p.Events)
                .HasForeignKey(d => d.EventStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_event_status");

            entity.HasOne(d => d.Status).WithMany(p => p.Events)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.EventUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_user_update");
        });

        modelBuilder.Entity<EventManager>(entity =>
        {
            entity.ToTable("event_manager");

            entity.HasIndex(e => new { e.EventId, e.UserId }, "IX_event_manager").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Event).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_manager_event");

            entity.HasOne(d => d.Status).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_manager_status");

            entity.HasOne(d => d.User).WithMany(p => p.EventManagers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_manager_user");
        });

        modelBuilder.Entity<EventParticipantTeam>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_event-participant-team_1");

            entity.ToTable("event-participant-team");

            entity.HasIndex(e => new { e.ParticipantId, e.EventId }, "IX_event-participant-team").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.IsLeader).HasColumnName("is_leader");
            entity.Property(e => e.ParticipantId).HasColumnName("participant_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Event).WithMany(p => p.EventParticipantTeams)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-participant_event");

            entity.HasOne(d => d.Participant).WithMany(p => p.EventParticipantTeams)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-participant_user");

            entity.HasOne(d => d.Status).WithMany(p => p.EventParticipantTeams)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event-participant-team_status");

            entity.HasOne(d => d.Team).WithMany(p => p.EventParticipantTeams)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-participant_team");
        });

        modelBuilder.Entity<EventStatus>(entity =>
        {
            entity.ToTable("event_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Status).WithMany(p => p.EventStatuses)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_status_status");
        });

        modelBuilder.Entity<EventTask>(entity =>
        {
            entity.ToTable("event_task");

            entity.HasIndex(e => new { e.EventId, e.TaskId }, "IX_event_task").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Event).WithMany(p => p.EventTasks)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_task_event");

            entity.HasOne(d => d.Status).WithMany(p => p.EventTasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_task_status");

            entity.HasOne(d => d.Task).WithMany(p => p.EventTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_task_task");
        });

        modelBuilder.Entity<EventTaskEvaluateUser>(entity =>
        {
            entity.ToTable("event_task_evaluate_user");

            entity.HasIndex(e => new { e.EventTaskId, e.EvaluateUserId }, "IX_event_task_evaluate_user").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EvaluateUserId).HasColumnName("evaluate_user_id");
            entity.Property(e => e.EventTaskId).HasColumnName("event_task_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.EvaluateUser).WithMany(p => p.EventTaskEvaluateUsers)
                .HasForeignKey(d => d.EvaluateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_task_evaluate_user_event_manager");

            entity.HasOne(d => d.EventTask).WithMany(p => p.EventTaskEvaluateUsers)
                .HasForeignKey(d => d.EventTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_task_evaluate_user_event_task");

            entity.HasOne(d => d.Status).WithMany(p => p.EventTaskEvaluateUsers)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_event_task_evaluate_user_status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusId)
                .HasDefaultValueSql("((1))")
                .HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");

            entity.HasOne(d => d.Status).WithMany(p => p.Roles)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_role_status");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.ToTable("task");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BonusExtraTime).HasColumnName("bonus_extra_time");
            entity.Property(e => e.BonusPoints).HasColumnName("bonus_points");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.HasBonus).HasColumnName("has_bonus");
            entity.Property(e => e.Language)
                .HasMaxLength(50)
                .HasColumnName("language");
            entity.Property(e => e.Platform)
                .HasMaxLength(50)
                .HasColumnName("platform");
            entity.Property(e => e.Points).HasColumnName("points");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskBody)
                .HasMaxLength(4000)
                .HasColumnName("task_body");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasColumnName("title");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_task_category");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.TaskCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_user_create");

            entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.TaskUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_user_update");
        });

        modelBuilder.Entity<TaskCategory>(entity =>
        {
            entity.ToTable("task_category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(50)
                .HasColumnName("color");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.TaskCategoryCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_category_user_create");

            entity.HasOne(d => d.Status).WithMany(p => p.TaskCategories)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_category_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.TaskCategoryUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_category_user_update");
        });

        modelBuilder.Entity<TaskParticipant>(entity =>
        {
            entity.ToTable("task_participant");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ParticipantUserId).HasColumnName("participant_user_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TeamTaskId).HasColumnName("team_task_id");

            entity.HasOne(d => d.ParticipantUser).WithMany(p => p.TaskParticipants)
                .HasForeignKey(d => d.ParticipantUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_participant_team-participant");

            entity.HasOne(d => d.Status).WithMany(p => p.TaskParticipants)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_participant_status");

            entity.HasOne(d => d.TeamTask).WithMany(p => p.TaskParticipants)
                .HasForeignKey(d => d.TeamTaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_task_participant_team-task");
        });

        modelBuilder.Entity<TaskStatus>(entity =>
        {
            entity.ToTable("task_status");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.ToTable("team");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

            entity.HasOne(d => d.CreateUser).WithMany(p => p.TeamCreateUsers)
                .HasForeignKey(d => d.CreateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team_user_create");

            entity.HasOne(d => d.Event).WithMany(p => p.Teams)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team_event");

            entity.HasOne(d => d.Status).WithMany(p => p.Teams)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team_status");

            entity.HasOne(d => d.UpdateUser).WithMany(p => p.TeamUpdateUsers)
                .HasForeignKey(d => d.UpdateUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team_user_update");
        });

        modelBuilder.Entity<TeamTask>(entity =>
        {
            entity.ToTable("team-task");

            entity.HasIndex(e => new { e.TeamId, e.TaskId }, "IX_team-task").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime)
                .HasColumnType("datetime")
                .HasColumnName("end_time");
            entity.Property(e => e.StartTime)
                .HasColumnType("datetime")
                .HasColumnName("start_time");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");
            entity.Property(e => e.TaskScore).HasColumnName("task_score");
            entity.Property(e => e.TaskStatus).HasColumnName("task_status");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Status).WithMany(p => p.TeamTasks)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-task_status");

            entity.HasOne(d => d.Task).WithMany(p => p.TeamTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-task_task");

            entity.HasOne(d => d.TaskStatusNavigation).WithMany(p => p.TeamTasks)
                .HasForeignKey(d => d.TaskStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-task_task_status");

            entity.HasOne(d => d.Team).WithMany(p => p.TeamTasks)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_team-task_team");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("user");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreateDate)
                .HasColumnType("datetime")
                .HasColumnName("create_date");
            entity.Property(e => e.CreateUserId).HasColumnName("create_user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.Github)
                .HasMaxLength(50)
                .HasColumnName("github");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime")
                .HasColumnName("update_date");
            entity.Property(e => e.UpdateUserId).HasColumnName("update_user_id");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_role");

            entity.HasOne(d => d.Status).WithMany(p => p.Users)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_status");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
