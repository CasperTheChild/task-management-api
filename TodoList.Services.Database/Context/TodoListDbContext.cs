using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoList.Services.Database.Entities;
using TodoList.Services.Database.Identity;

namespace TodoList.Services.Database.Context;

public class TodoListDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string,
       IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>,
       IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    public TodoListDbContext(DbContextOptions<TodoListDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoListEntity> TodoLists { get; set; }

    public DbSet<TaskEntity> Tasks { get; set; }

    public DbSet<TaskAssignmentEntity> TaskAssignments { get; set; }

    public DbSet<TagEntity> Tags { get; set; }

    public DbSet<TaskTagEntity> TaskTags { get; set; }

    public DbSet<CommentEntity> Comments { get; set; }

    public DbSet<TodoListUserEntity> TodoListUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskEntity>()
            .HasOne(t => t.TodoList)
            .WithMany(l => l.Tasks)
            .HasForeignKey(t => t.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskAssignmentEntity>(entity =>
        {
            entity.HasKey(ta => new { ta.TaskId, ta.UserId });
            entity.HasOne(ta => ta.Task)
                .WithMany(t => t.AssignedUsers)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(ta => ta.User)
                .WithMany()
                .HasForeignKey(ta => ta.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TaskTagEntity>(entity =>
        {
            entity.HasKey(tt => new { tt.TaskId, tt.TagId });
            entity.HasOne(tt => tt.Task)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(tt => tt.Tag)
                .WithMany(t => t.TaskTags)
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(tt => tt.TaskId);
            entity.HasIndex(tt => tt.TagId);
        });

        modelBuilder.Entity<TagEntity>()
            .HasIndex(t => t.TagName)
            .IsUnique();

        modelBuilder.Entity<TodoListUserEntity>(entity =>
        {
            entity.HasKey(tlu => new { tlu.TodoListId, tlu.UserId });
            entity.HasOne(tlu => tlu.TodoList)
                .WithMany(tl => tl.Members)
                .HasForeignKey(tlu => tlu.TodoListId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(tlu => tlu.User)
                .WithMany()
                .HasForeignKey(tlu => tlu.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
