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

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<EventCategory> EventCategories { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupCategory> GroupCategories { get; set; }

    public virtual DbSet<HelpdeskRequest> HelpdeskRequests { get; set; }

    public virtual DbSet<HelpdeskStatus> HelpdeskStatuses { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<LeaveType> LeaveTypes { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<NotificationCategory> NotificationCategories { get; set; }

    public virtual DbSet<NotificationMapping> NotificationMappings { get; set; }

    public virtual DbSet<QuickLink> QuickLinks { get; set; }

    public virtual DbSet<Reason> Reasons { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StatusHistory> StatusHistories { get; set; }

    public virtual DbSet<SubCategory> SubCategories { get; set; }

    public virtual DbSet<SubcategoryMapping> SubcategoryMappings { get; set; }

    public virtual DbSet<Widget> Widgets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("priority", new[] { "low", "medium", "high" });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("documents_pkey");

            entity.ToTable("documents");

            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.Documents)
                .HasColumnType("character varying")
                .HasColumnName("documents");
            entity.Property(e => e.EventId).HasColumnName("event_id");
            entity.Property(e => e.IsActive).HasColumnName("is_active");

            entity.HasOne(d => d.Event).WithMany(p => p.Documents)
                .HasForeignKey(d => d.EventId)
                .HasConstraintName("event_id");
        });

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
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Description)
                .HasColumnType("character varying")
                .HasColumnName("description");
            entity.Property(e => e.EndDate).HasColumnName("end_date");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.StartDate).HasColumnName("start_date");

            entity.HasOne(d => d.Category).WithMany(p => p.Events)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("event_category_id_fkey");
        });

        modelBuilder.Entity<EventCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("event_category_pkey");

            entity.ToTable("event_category");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Category)
                .HasColumnType("character varying")
                .HasColumnName("category");
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("group_pkey");

            entity.ToTable("group");

            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.GroupName)
                .HasMaxLength(20)
                .HasColumnName("group_name");
        });

        modelBuilder.Entity<GroupCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("category_pkey");

            entity.ToTable("group_category");

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("nextval('category_category_id_seq'::regclass)")
                .HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasColumnName("category_name");
            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.HasOne(d => d.Group).WithMany(p => p.GroupCategories)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("category_group_id_fkey");
        });

        modelBuilder.Entity<HelpdeskRequest>(entity =>
        {
            entity.HasKey(e => e.HelpdeskRequestId).HasName("helpdesk_pkey");

            entity.ToTable("helpdesk_request");

            entity.Property(e => e.HelpdeskRequestId)
                .HasDefaultValueSql("nextval('helpdesk_helpdesk_id_seq'::regclass)")
                .HasColumnName("helpdesk_request_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("deleted_at");
            entity.Property(e => e.GroupId).HasColumnName("group_id");
            entity.Property(e => e.InsertedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("inserted_at");
            entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");
            entity.Property(e => e.PendingAt).HasColumnName("pending_at");
            entity.Property(e => e.Priority).HasColumnName("priority");
            entity.Property(e => e.ServiceDetails)
                .HasColumnType("character varying")
                .HasColumnName("service_details");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.SubCategoryId).HasColumnName("sub_category_id");

            entity.HasOne(d => d.Category).WithMany(p => p.HelpdeskRequests)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("helpdesk_category_id_fkey");

            entity.HasOne(d => d.Group).WithMany(p => p.HelpdeskRequests)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("helpdesk_group_id_fkey");

            entity.HasOne(d => d.InsertedByNavigation).WithMany(p => p.HelpdeskRequests)
                .HasForeignKey(d => d.InsertedBy)
                .HasConstraintName("helpdesk_inserted_by_fkey");

            entity.HasOne(d => d.PendingAtNavigation).WithMany(p => p.HelpdeskRequests)
                .HasForeignKey(d => d.PendingAt)
                .HasConstraintName("helpdesk_request_pending_at_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.HelpdeskRequests)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("helpdesk_status_id_fkey");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.HelpdeskRequests)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("helpdesk_sub_catrgory_id_fkey");
        });

        modelBuilder.Entity<HelpdeskStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("helpdesk_status_pkey");

            entity.ToTable("helpdesk_status");

            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StatusName)
                .HasMaxLength(50)
                .HasColumnName("status_name");
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

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Notification1)
                .HasColumnType("character varying")
                .HasColumnName("notification");
            entity.Property(e => e.NotificationCategoryId).HasColumnName("notification_category_id");

            entity.HasOne(d => d.NotificationCategory).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.NotificationCategoryId)
                .HasConstraintName("notification_notification_category_id");
        });

        modelBuilder.Entity<NotificationCategory>(entity =>
        {
            entity.HasKey(e => e.NotificationCategoryId).HasName("notification_category_pkey");

            entity.ToTable("notification_category");

            entity.Property(e => e.NotificationCategoryId).HasColumnName("notification_category_id");
            entity.Property(e => e.Category)
                .HasColumnType("character varying")
                .HasColumnName("category");
        });

        modelBuilder.Entity<NotificationMapping>(entity =>
        {
            entity.HasKey(e => e.NotificationMappingId).HasName("notification_mapping_pkey");

            entity.ToTable("notification_mapping");

            entity.Property(e => e.NotificationMappingId).HasColumnName("notification_mapping_id");
            entity.Property(e => e.NotificationId).HasColumnName("notification_id");
            entity.Property(e => e.ReadMark)
                .HasDefaultValueSql("false")
                .HasColumnName("read_mark");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Notification).WithMany(p => p.NotificationMappings)
                .HasForeignKey(d => d.NotificationId)
                .HasConstraintName("notification_mapping_notification_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.NotificationMappings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notification_mapping_employee_id_fkey");
        });

        modelBuilder.Entity<QuickLink>(entity =>
        {
            entity.HasKey(e => e.QuickLinkId).HasName("quick_link_pkey");

            entity.ToTable("quick_link");

            entity.Property(e => e.QuickLinkId).HasColumnName("quick_link_id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
            entity.Property(e => e.Url)
                .HasColumnType("character varying")
                .HasColumnName("url");
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

        modelBuilder.Entity<StatusHistory>(entity =>
        {
            entity.HasKey(e => e.StatusHistoryId).HasName("status_history_pkey");

            entity.ToTable("status_history");

            entity.Property(e => e.StatusHistoryId).HasColumnName("status_history_id");
            entity.Property(e => e.Comment)
                .HasColumnType("character varying")
                .HasColumnName("comment");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.StatusChnageDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("status_chnage_date");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

            entity.HasOne(d => d.Request).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("status_history_request_id_fkey");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.Status)
                .HasConstraintName("status_history_status_fkey");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.StatusHistories)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("status_history_updated_by_fkey");
        });

        modelBuilder.Entity<SubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId).HasName("sub_category_pkey");

            entity.ToTable("sub_category");

            entity.Property(e => e.SubCategoryId).HasColumnName("sub_category_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.SubCategoryName)
                .HasMaxLength(50)
                .HasColumnName("sub_category_name");

            entity.HasOne(d => d.Category).WithMany(p => p.SubCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("sub_category_category_id_fkey");
        });

        modelBuilder.Entity<SubcategoryMapping>(entity =>
        {
            entity.HasKey(e => e.SubcategoryMappingId).HasName("subcategory_mapping_pkey");

            entity.ToTable("subcategory_mapping");

            entity.Property(e => e.SubcategoryMappingId).HasColumnName("subcategory_mapping_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.RequestId).HasColumnName("request_id");
            entity.Property(e => e.SubCategoryId).HasColumnName("sub_category_id");

            entity.HasOne(d => d.Category).WithMany(p => p.SubcategoryMappings)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("subcategory_mapping_category_id_fkey");

            entity.HasOne(d => d.Request).WithMany(p => p.SubcategoryMappings)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("subcategory_mapping_request_id_fkey");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.SubcategoryMappings)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("subcategory_mapping_sub_category_id_fkey");
        });

        modelBuilder.Entity<Widget>(entity =>
        {
            entity.HasKey(e => e.WidgetId).HasName("widget_pkey");

            entity.ToTable("widget");

            entity.Property(e => e.WidgetId).HasColumnName("widget_id");
            entity.Property(e => e.CardName)
                .HasColumnType("character varying")
                .HasColumnName("card_name");
            entity.Property(e => e.IsVisible).HasColumnName("is_visible");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
