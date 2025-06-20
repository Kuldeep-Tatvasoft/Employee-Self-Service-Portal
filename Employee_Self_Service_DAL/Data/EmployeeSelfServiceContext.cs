using System;
using System.Collections.Generic;
using Employee_Self_Service_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Employee_Self_Service_DAL.Data;

public partial class EmployeeSelfServiceContext : DbContext
{
    public EmployeeSelfServiceContext(DbContextOptions<EmployeeSelfServiceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<Reason> Reasons { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.HasIndex(e => e.Email, "employee_email_key").IsUnique();

            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.AnyDiseases)
                .HasColumnType("character varying")
                .HasColumnName("any_diseases");
            entity.Property(e => e.BloodGroup)
                .HasColumnType("character varying")
                .HasColumnName("blood_group");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DateOfBirth).HasColumnName("date_of_birth");
            entity.Property(e => e.DeletedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Department)
                .HasMaxLength(50)
                .HasColumnName("department");
            entity.Property(e => e.Designation)
                .HasColumnType("character varying")
                .HasColumnName("designation");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Gender)
                .HasColumnType("character varying")
                .HasColumnName("gender");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("true")
                .HasColumnName("is_active");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValueSql("false")
                .HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.ProfileImage)
                .HasColumnType("character varying")
                .HasColumnName("profile_image");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("employee_role_id");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("event_pkey");

            entity.ToTable("event");

            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.LeaveRequestId).HasName("leave_request_pkey");

            entity.ToTable("leave_request");

            entity.Property(e => e.LeaveRequestId).HasColumnName("leave_request_id");
            entity.Property(e => e.ActualLeaveDuration).HasColumnName("actual_leave_duration");
            entity.Property(e => e.AdhocLeave).HasColumnName("adhoc_leave");
            entity.Property(e => e.AlternatePhoneMo)
                .HasColumnType("character varying")
                .HasColumnName("alternate_phone_mo");
            entity.Property(e => e.ApprovedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("approved_at");
            entity.Property(e => e.ApprovedBy).HasColumnName("approved_by");
            entity.Property(e => e.AvailableOnPhone).HasColumnName("available_on_phone");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.EmployeeId).HasColumnName("employee_id");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.EndLeaveType).HasColumnName("end_leave_type");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ReasonDescription)
                .HasColumnType("character varying")
                .HasColumnName("reason_description");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.StartDate).HasColumnName("start_date");
            entity.Property(e => e.StartLeaveType).HasColumnName("start_leave_type");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.TotalLeaveDuration).HasColumnName("total_leave_duration");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.ApprovedByNavigation).WithMany(p => p.LeaveRequestApprovedByNavigations)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("leave_request_approved_by_fkey");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequestEmployees)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("leave_request_employee_id_fkey");

            entity.HasOne(d => d.EndLeaveTypeNavigation).WithMany(p => p.LeaveRequestEndLeaveTypeNavigations)
                .HasForeignKey(d => d.EndLeaveType)
                .HasConstraintName("leave_request_end_leave_type_fkey");

            entity.HasOne(d => d.ReasonNavigation).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.Reason)
                .HasConstraintName("leave_request_reason_fkey");

            entity.HasOne(d => d.StartLeaveTypeNavigation).WithMany(p => p.LeaveRequestStartLeaveTypeNavigations)
                .HasForeignKey(d => d.StartLeaveType)
                .HasConstraintName("leave_request_start_leave_type_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("leave_request_status_id_fkey");
        });

        modelBuilder.Entity<LeaveType>(entity =>
        {
            entity.HasKey(e => e.TypeId).HasName("leave_type_pkey");

            entity.ToTable("leave_type");

            entity.Property(e => e.TypeId).HasColumnName("type_id");
            entity.Property(e => e.Type)
                .HasColumnType("character varying")
                .HasColumnName("type");
        });

        modelBuilder.Entity<Reason>(entity =>
        {
            entity.HasKey(e => e.ReasonId).HasName("reason_pkey");

            entity.ToTable("reason");

            entity.Property(e => e.ReasonId).HasColumnName("reason_id");
            entity.Property(e => e.Reason1)
                .HasColumnType("character varying")
                .HasColumnName("reason");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("request_status_pkey");

            entity.ToTable("request_status");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("role_pkey");

            entity.ToTable("role");

            entity.Property(e => e.RoleId)
                .ValueGeneratedNever()
                .HasColumnName("role_id");
            entity.Property(e => e.Role1)
                .HasMaxLength(16)
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
