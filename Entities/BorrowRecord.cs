using System;
using System.Collections.Generic;

namespace LibraryManagement.Entities;

public partial class BorrowRecord
{
    public int Id { get; set; }

    public int BookId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime DueDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string Status { get; set; } = null!;

    public int MemberId { get; set; }

    public virtual Book Book { get; set; } = null!;

    public virtual ICollection<Fine> Fines { get; set; } = new List<Fine>();

    public virtual Member Member { get; set; } = null!;
}
