using System;
using System.Collections.Generic;

namespace LibraryManagement.Models;

public partial class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Isbn { get; set; } = null!;

    public int? PublicationYear { get; set; }

    public int CopiesOwned { get; set; }

    public int AvailableCopies { get; set; }

    public int AuthorId { get; set; }

    public int CategoryId { get; set; }

    public int PublisherId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<BookReservation> BookReservations { get; set; } = new List<BookReservation>();

    public virtual ICollection<BorrowRecord> BorrowRecords { get; set; } = new List<BorrowRecord>();

    public virtual Category Category { get; set; } = null!;

    public virtual Publisher Publisher { get; set; } = null!;
}
