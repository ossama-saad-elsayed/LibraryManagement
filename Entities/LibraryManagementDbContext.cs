using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Entities;

public partial class LibraryManagementDbContext : DbContext
{
    public LibraryManagementDbContext()
    {
    }

    public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowRecord> BorrowRecords { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Fine> Fines { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>(entity =>
        {
            entity.ToTable("AUTHORS");

            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.ToTable("BOOKS");

            entity.HasIndex(e => e.AuthorId, "IX_BOOKS_AuthorId");

            entity.HasIndex(e => e.CategoryId, "IX_BOOKS_CategoryId");

            entity.HasIndex(e => e.PublisherId, "IX_BOOKS_PublisherId");

            entity.HasIndex(e => e.Title, "IX_BOOKS_Title");

            entity.HasIndex(e => e.Isbn, "UQ_BOOKS_ISBN").IsUnique();

            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKS_AUTHORS");

            entity.HasOne(d => d.Category).WithMany(p => p.Books)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKS_CATEGORIES");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BOOKS_PUBLISHERS");
        });

        modelBuilder.Entity<BorrowRecord>(entity =>
        {
            entity.ToTable("BORROW_RECORDS");

            entity.HasIndex(e => e.BookId, "IX_BORROW_RECORDS_BookId");

            entity.HasIndex(e => e.Status, "IX_BORROW_RECORDS_Status");

            entity.Property(e => e.BorrowDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("Borrowed");

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowRecords)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BORROW_RECORDS_BOOKS");

            entity.HasOne(d => d.Member).WithMany(p => p.BorrowRecords)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BORROW_RECORDS_MEMBERS");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("CATEGORIES");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Fine>(entity =>
        {
            entity.ToTable("FINES");

            entity.HasIndex(e => e.BorrowRecordId, "IX_FINES_BorrowRecordId");

            entity.HasIndex(e => e.IsPaid, "IX_FINES_IsPaid");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PaidDate).HasColumnType("datetime");

            entity.HasOne(d => d.BorrowRecord).WithMany(p => p.Fines)
                .HasForeignKey(d => d.BorrowRecordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FINES_BORROW_RECORDS");
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MEMBERS__3214EC07BDD5B666");

            entity.ToTable("MEMBERS");

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(200);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.ToTable("PUBLISHERS");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(30);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");

            entity.HasIndex(e => e.Email, "UQ_USERS_Email").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("Member");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
